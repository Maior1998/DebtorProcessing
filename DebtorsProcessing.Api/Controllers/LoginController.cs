
using DebtorsProcessing.Api.Configuration;
using DebtorsProcessing.Api.Model;
using DebtorsProcessing.Api.Model.Dtos.Responses;
using DebtorsProcessing.Api.Repositories;
using DebtorsProcessing.DatabaseModel.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly IDebtorsRepository repository;
        /// <summary>
        /// Настройки обработки JWToken'ов. Заполнятся Service Controller'ом
        /// </summary>
        private readonly JwtConfig jwtConfig;
        /// <summary>
        /// Параметры валиадации токена JWT, необходимые для генерации новых токенов. Заполнятся Service Controller'ом
        /// </summary>
        private readonly LoginTokenValidationParameters loginTokenValidationParameters;

        /// <summary>
        /// Инициализирует новый контроллер управления пользователями.
        /// </summary>
        /// <param name="optionsMonitor">Монитор настроек <see cref="JwtConfig"/>.</param>
        /// <param name="loginTokenValidationParameters">Параметры валидации токена JWT, которые будут использоваться при создании новых токенов JWT.</param>
        public LoginController(
            IDebtorsRepository repository,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            LoginTokenValidationParameters loginTokenValidationParameters)
        {
            this.loginTokenValidationParameters = loginTokenValidationParameters;
            jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost]
        [Route(nameof(Login))]
        public async Task<IActionResult> Login()
        {
            return Ok();
        }

        

        /// <summary>
        /// Производит генерацию и сохранение нового токена для пользователя.
        /// </summary>
        /// <param name="user">Пользователь, для которого необходимо сгенрировать пару "токен + токен обновления"</param>
        /// <returns>Результат генерации токена.</returns>
        private async Task<AuthResult> GenerateLoginJwt(User user)
        {
            JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(jwtConfig.LoginSecret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(nameof(DatabaseModel.Entities.User.Id),user.Id.ToString()),
                    new Claim(nameof(DatabaseModel.Entities.User.Login), user.Login),
                    new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToShortDateString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),

            };

            SecurityToken token = jwtTokenHandler.CreateToken(tokenDescriptor);
            string jwtToken = jwtTokenHandler.WriteToken(token);

            LoginRefreshToken refreshToken = new LoginRefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevoked = false,
                UserId = user.Id,
                CreatedOn = DateTime.UtcNow,
                ExpiryTime = DateTime.UtcNow.AddMonths(1),
                Token = $"{Guid.NewGuid()}-{Guid.NewGuid()}"
            };

            await repository.AddRefreshTokenAsync(refreshToken);

            return new AuthResult()
            {
                Success = true,
                Token = jwtToken,
                RefreshToken = refreshToken.Token
            };
        }
    }
}
