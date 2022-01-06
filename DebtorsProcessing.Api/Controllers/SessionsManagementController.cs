using DebtorsProcessing.Api.Attributes;
using DebtorsProcessing.Api.Configuration;
using DebtorsProcessing.Api.Helpers;
using DebtorsProcessing.Api.Model.Dtos.Responses;
using DebtorsProcessing.Api.Repositories;
using DebtorsProcessing.Api.Repositories.RefreshTokensRepositories;
using DebtorsProcessing.Api.Repositories.RolesRepositories;
using DebtorsProcessing.Api.Repositories.SessionsRepositories;
using DebtorsProcessing.DatabaseModel.Entities;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DebtorsProcessing.Api.Repositories.UsersRepositories;
using Microsoft.Extensions.Options;

namespace DebtorsProcessing.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [RequiresLoginAuthorize]
    public class SessionsManagementController : ControllerBase
    {
        private readonly Guid userId;

        private readonly IRolesRepository userRolesRepository;
        private readonly ISessionsRepository sessionsRepository;
        private readonly IRefreshTokensRepository refreshTokensRepository;
        private readonly IUsersRepository usersRepository;
        private readonly JwtConfig jwtConfig;
        public SessionsManagementController(
            IOptionsMonitor<JwtConfig> jwtConfig,
            IRolesRepository userRolesRepository,
            ISessionsRepository sessionsRepository,
            IRefreshTokensRepository refreshTokensRepository,
            IUsersRepository usersRepository,
            IHttpContextAccessor accessor)
        {
            this.usersRepository = usersRepository;
            this.refreshTokensRepository = refreshTokensRepository;
            this.jwtConfig = jwtConfig.CurrentValue;
            this.userRolesRepository = userRolesRepository;
            this.sessionsRepository = sessionsRepository;
            userId = ((User)accessor.HttpContext.Items["User"]).Id;
        }

        [HttpGet]
        [Route("MyRoles")]
        public async Task<ActionResult<RolesArrayDto>> GetOwnedRoles()
        {
            IEnumerable<UserRole> result = await userRolesRepository.GetRolesOfUser(userId);
            return Ok(new RolesArrayDto() { Roles = result.Select(x => x.ToDto()) });
        }

        [HttpGet]
        [Route("GetSessionInfo")]
        public async Task<ActionResult<ChooseUserSessionDto>> GetSessionInfo(Guid sessionId)
        {
            ChooseUserSessionDto[] sessions = await GetActiveSessionsInternal();
            ChooseUserSessionDto session = sessions.SingleOrDefault(x => x.Id == sessionId);
            if (session == null)
                return NotFound();
            return session;
        }

        [HttpGet]
        [Route("MySessions")]
        public async Task<ActionResult<UserActiveSessionsDto>> GetActiveSessions()
        {
            ChooseUserSessionDto[] result = await GetActiveSessionsInternal();
            return Ok(new UserActiveSessionsDto() { Sessions = result });
        }

        private async Task<ChooseUserSessionDto[]> GetActiveSessionsInternal()
        {
            IEnumerable<UserSession> sessionsEnum = await sessionsRepository.GetActiveSessionsOfUser(userId);
            UserSession[] sessions = sessionsEnum.OrderBy(x => x.StartDate).ToArray();
            ChooseUserSessionDto[] result = new ChooseUserSessionDto[sessions.Length];
            for (int i = 0; i < sessions.Length; i++)
            {
                result[i] = new()
                {
                    Id = sessions[i].Id,
                    StartSessionTime = sessions[i].StartDate,
                };
                IEnumerable<UserRole> rolesInSession = await userRolesRepository.GetRolesInSession(sessions[i].Id);
                result[i].RolesInSession = rolesInSession.Select(x => x.ToDto()).ToArray();
            }
            return result;
        }

        [HttpPost]
        [Route(nameof(SelectSession))]
        public async Task<IActionResult> SelectSession(Guid sessionId)
        {
            IEnumerable<UserSession> sessionsEnum = await sessionsRepository.GetActiveSessionsOfUser(userId);
            UserSession session = sessionsEnum.SingleOrDefault(x => x.Id == sessionId);
            if (session==null)
                return BadRequest();
            await usersRepository.SetUserActiveSession(userId, session.Id);
            SelectedSessionResult result = await GenerateSessionJwt(session);
            return Ok(result);
        }

        [HttpPost]
        [Route(nameof(DropSession))]
        public async Task<ActionResult> DropSession()
        {
            Guid? currentSessionId = await usersRepository.GetUserActiveSession(userId);
            if (currentSessionId == null)
                return BadRequest();
            await usersRepository.SetUserActiveSession(userId, null);
            return Ok();
        }
        

        [HttpPost]
        [Route(nameof(CreateSession))]
        public async Task<ActionResult> CreateSession([FromBody] CreateSessionDto request)
        {
            //проверка на правильность состояния и не-пустоту массива ролей
            if (!ModelState.IsValid || request.Roles.Length == 0)
                return BadRequest();

            //Проверка на отсутвие дубликатов
            if (request.Roles.Distinct().Count() != request.Roles.Length)
                return BadRequest();

            IEnumerable<UserRole> userRoles = await userRolesRepository.GetRolesOfUser(userId);
            string[] userRolesAsStrings = userRoles.Select(x => x.Name).ToArray();
            UserSession session = new()
                {
                    UserId = userId,
                    CreatedOn = DateTime.Now
                };
            foreach (string roleFromRequest in request.Roles)
            {
                if (!userRolesAsStrings.Contains(roleFromRequest))
                    return Forbid("User hasn't one or more of specified roles");
                UserRole userRole = userRoles.Single(x => x.Name == roleFromRequest);
                session.Roles.Add(userRole);
            }

            await sessionsRepository.AddEntity(session);
            Guid id = session.Id;
            return Created(nameof(GetSessionInfo), id);
        }

        /// <summary>
        /// Создает новый токен сессии пользователя и помещает его в базу данных.
        /// </summary>
        /// <param name="session">Сессия пользователя, для которой необходимо сгенрировать пару "токен + токен обновления"</param>
        /// <returns>Результат генерации токена.</returns>
        private async Task<SelectedSessionResult> GenerateSessionJwt(UserSession session)
        {
            JwtSecurityTokenHandler jwtTokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(jwtConfig.SessionSecret);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new(new[]
                {
                    new Claim(nameof(DatabaseModel.Entities.UserSession.Id),session.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToShortDateString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),

            };

            SecurityToken token = jwtTokenHandler.CreateToken(tokenDescriptor);
            string jwtToken = jwtTokenHandler.WriteToken(token);

            SessionRefreshToken refreshToken = new()
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevoked = false,
                RecordId = session.Id,
                CreatedOn = DateTime.UtcNow,
                ExpiryTime = DateTime.UtcNow.AddMonths(1),
                Token = $"{Guid.NewGuid()}-{Guid.NewGuid()}"
            };

            await refreshTokensRepository.AddSessionRefreshTokenAsync(refreshToken);

            return new()
            {
                Success = true,
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
            };
        }

        public record CreateSessionDto
        {
            [Required]
            public string[] Roles { get; set; }
        }
    }
}
