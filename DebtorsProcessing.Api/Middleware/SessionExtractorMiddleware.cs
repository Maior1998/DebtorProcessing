using DebtorsProcessing.Api.Configuration;
using DebtorsProcessing.Api.Repositories;
using DebtorsProcessing.Api.Repositories.SessionsRepositories;
using DebtorsProcessing.DatabaseModel.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebtorsProcessing.Api.Repositories.UsersRepositories;

namespace DebtorsProcessing.Api.Middleware
{
    public class SessionExtractorMiddleware
    {
        private readonly RequestDelegate next;
        private readonly JwtConfig jwtConfig;

        public SessionExtractorMiddleware(RequestDelegate next, IOptions<JwtConfig> appSettings)
        {
            this.next = next;
            jwtConfig = appSettings.Value;
        }

        public async Task Invoke(HttpContext context,IUsersRepository usersRepository, ISessionsRepository sessionsRepository)
        {
            string token = context.Request.Headers[ConfigurationCostants.SessionAuthenticationScheme].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await AttachSessionToContext(context, sessionsRepository,usersRepository, token);

            await next(context);
        }

        private async Task AttachSessionToContext(
            HttpContext context,
            ISessionsRepository sessionsRepository,
            IUsersRepository usersRepository,
            string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new();
                byte[] key = Encoding.ASCII.GetBytes(jwtConfig.SessionSecret);
                tokenHandler.ValidateToken(token, new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                Guid sessionId = Guid.Parse(jwtToken.Claims.First(x => x.Type == nameof(UserSession.Id)).Value);
                UserSession session = await sessionsRepository.GetEntityById(sessionId);
                if (session.UserId == null) return;
                User user = await usersRepository.GetEntityById(session.UserId.Value);
                if(user.ActiveSessionId == null || user.ActiveSessionId != sessionId) return;
                // attach user to context on successful jwt validation
                context.Items["Session"] = session;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
