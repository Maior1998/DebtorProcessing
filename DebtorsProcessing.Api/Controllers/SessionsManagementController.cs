using DebtorsProcessing.Api.Attributes;
using DebtorsProcessing.Api.Helpers;
using DebtorsProcessing.Api.Model.Dtos.Responses;
using DebtorsProcessing.Api.Repositories;
using DebtorsProcessing.Api.Repositories.RolesRepositories;
using DebtorsProcessing.Api.Repositories.SessionsRepositories;
using DebtorsProcessing.DatabaseModel.Entities;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public SessionsManagementController(
            IRolesRepository userRolesRepository,
            ISessionsRepository sessionsRepository)
        {
            this.userRolesRepository = userRolesRepository;
            this.sessionsRepository = sessionsRepository;
            userId = ((User)HttpContext.Items["User"]).Id;
        }

        [HttpGet]
        [Route("MyRoles")]
        public async Task<ActionResult<RolesArrayDto>> GetOwnedRoles()
        {
            IEnumerable<UserRole> result = await userRolesRepository.GetRolesOfUser(userId);
            return Ok(new RolesArrayDto() { Roles = result.Select(x => x.ToDto()) });
        }

        [HttpGet]
        [Route("MySessions")]
        public async Task<ActionResult<UserActiveSessionsDto>> GetActiveSessions()
        {
            IEnumerable<UserSession> sessionsEnum = await sessionsRepository.GetActiveSessionsOfUser(userId);
            UserSession[] sessions = sessionsEnum.OrderBy(x => x.StartDate).ToArray();
            ChooseUserSessionDto[] result = new ChooseUserSessionDto[sessions.Length];
            for (int i = 0; i < sessions.Length; i++)
            {
                result[i] = new()
                {
                    Index = i,
                    StartSessionTime = sessions[i].StartDate,
                };
                IEnumerable<UserRole> rolesInSession = await userRolesRepository.GetRolesInSession(sessions[i].Id);
                result[i].RolesInSession = rolesInSession.Select(x => x.ToDto()).ToArray();
            }
            return Ok(new UserActiveSessionsDto() { Sessions = result });
        }

        [HttpPost]
        [Route("SelectSession/{sessionOrderIndex}")]
        public async Task<ActionResult> SelectSession(int sessionOrderIndex)
        {
            if (sessionOrderIndex < 0)
                return BadRequest("Index cannot be less then zero.");
            IEnumerable<UserSession> sessionsEnum = await sessionsRepository.GetActiveSessionsOfUser(userId);
            UserSession[] sessions = sessionsEnum.OrderBy(x => x.StartDate).ToArray();
            if (sessionOrderIndex >= sessions.Length)
                return BadRequest("Index cannot be greater or equal then sessions count");
            return Ok(sessions[sessionOrderIndex]);
        }
    }
}
