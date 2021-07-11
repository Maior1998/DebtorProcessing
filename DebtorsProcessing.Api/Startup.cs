using DebtorsProcessing.Api.Configuration;
using DebtorsProcessing.Api.Model;
using DebtorsProcessing.Api.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<DebtorsDbModel.DebtorsContext>();
            services.AddSingleton<IDebtorsRepository,DebtorsProcessing.Api.>


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DebtorsProcessing.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DebtorsProcessing.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }





        private void AddTokenValidationParams(IServiceCollection services)
        {
            byte[] key = Encoding.ASCII.GetBytes(Configuration[$"{nameof(JwtConfig)}:{nameof(JwtConfig.LoginSecret)}"]);
            LoginTokenValidationParameters loginTokenValidationParams = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false,
            };
            services.AddSingleton(loginTokenValidationParams);


            key = Encoding.ASCII.GetBytes(Configuration[$"{nameof(JwtConfig)}:{nameof(JwtConfig.SessionSecret)}"]);
            SessionTokenValidationParameters sessionTokenValidationParams = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false,
            };
            services.AddSingleton(sessionTokenValidationParams);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = ConfigurationCostants.LoginAuthenticationScheme;
                options.DefaultScheme = ConfigurationCostants.LoginAuthenticationScheme;
                options.DefaultChallengeScheme = ConfigurationCostants.LoginAuthenticationScheme;
            })
                .AddJwtBearer(ConfigurationCostants.LoginAuthenticationScheme, jwt =>
                {
                    jwt.SaveToken = true;
                    jwt.TokenValidationParameters = loginTokenValidationParams;
                })
                .AddJwtBearer(ConfigurationCostants.SessionAuthenticationScheme, jwt =>
                {
                    jwt.SaveToken = true;
                    jwt.TokenValidationParameters = sessionTokenValidationParams;
                });



            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<DebtorsDbModel.DebtorsContext>();
        }
    }
}
