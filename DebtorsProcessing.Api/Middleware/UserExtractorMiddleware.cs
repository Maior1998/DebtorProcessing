using DebtorsProcessing.Api.Configuration;
using DebtorsProcessing.Api.Repositories.UsersRepositories;
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

namespace DebtorsProcessing.Api.Middleware
{
    public class UserExtractorMiddleware
    {
        private readonly RequestDelegate next;
        private readonly JwtConfig jwtConfig;

        public UserExtractorMiddleware(RequestDelegate next, IOptions<JwtConfig> appSettings)
        {
            this.next = next;
            jwtConfig = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUsersRepository userService)
        {
            string token = context.Request.Headers[ConfigurationCostants.LoginAuthenticationScheme].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await AttachUserToContext(context, userService, token);

            await next(context);
        }

        private async Task AttachUserToContext(HttpContext context, IUsersRepository userService, string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new();
                byte[] key = Encoding.ASCII.GetBytes(jwtConfig.LoginSecret);
                TokenValidationParameters tvp =
                    new()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),

                        ValidateIssuer = false,
                        ValidateAudience = false,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero
                    };
                tokenHandler.ValidateToken(token, tvp, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                Guid userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == nameof(User.Id)).Value);

                // attach user to context on successful jwt validation
                context.Items["User"] = await userService.GetEntityById(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
