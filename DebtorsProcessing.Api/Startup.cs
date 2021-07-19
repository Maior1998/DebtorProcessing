using DebtorsProcessing.Api.Configuration;
using DebtorsProcessing.Api.EntitySecurityManagers.DebtorsSecurityManagers;
using DebtorsProcessing.Api.EntitySecurityManagers.SecurityJournalEventsSecurityManager;
using DebtorsProcessing.Api.EntitySecurityManagers.UserRolesSecurityManagers;
using DebtorsProcessing.Api.EntitySecurityManagers.UserSessionsSecurityManagers;
using DebtorsProcessing.Api.Helpers;
using DebtorsProcessing.Api.Middleware;
using DebtorsProcessing.Api.Model;
using DebtorsProcessing.Api.Repositories.DebtorsRepositories;
using DebtorsProcessing.Api.Repositories.RefreshTokensRepositories;
using DebtorsProcessing.Api.Repositories.RolesRepositories;
using DebtorsProcessing.Api.Repositories.SecurityJournalEventsRepositories;
using DebtorsProcessing.Api.Repositories.SecurityObjectRepositories;
using DebtorsProcessing.Api.Repositories.SessionsRepositories;
using DebtorsProcessing.Api.Repositories.UsersRepositories;
using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;

using System;
using System.Text;

namespace DebtorsProcessing.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddSingleton<IDebtorsRepository, SqlLiteDebtorsRepository>();
            services.AddSingleton<IRefreshTokensRepository, SqlLiteRefreshTokensRepository>();
            services.AddSingleton<IRolesRepository, SqlLiteRolesRepository>();
            services.AddSingleton<ISessionsRepository, SqlLiteSessionsRepository>();
            services.AddSingleton<IUsersRepository, SqlLiteUsersRepository>();
            services.AddSingleton<ISecurityJournalEventsRepository, SqlLiteSecurityJournalEventsRepository>();
            services.AddSingleton<ISecurityObjectsRepository, SqlLiteSecurityObjectsRepository>();
        }

        private static void AddSecurityManagers(IServiceCollection services)
        {
            services.AddSingleton<IDebtorsSecurityManager, RoleBasedDebtorSecurityManager>();
            services.AddSingleton<IUserRolesSecurityManager, RoleBasedUserRolesSecurityManager>();
            services.AddSingleton<IUserSessionSecurityManager, RoleBasedUserSessionsSecurityManager>();
            services.AddSingleton<ISecurityJournalEventsSecurityManager, RoleBasedSecurityJournalEventsSecurityManager>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<JwtConfig>(Configuration.GetSection(nameof(JwtConfig)));
            AddRepositories(services);
            AddSecurityManagers(services);

            services.AddHttpContextAccessor();
            services.AddDbContext<DebtorsContext>();
            AddTokenValidationParams(services);
            ConfigureSwagger(services);
            services
                .AddControllers()
                .AddOData(opt =>
                {
                    opt.AddRouteComponents("odata", GetEdmModel()).EnableQueryFeatures(5);
                    opt.RouteOptions.EnableKeyInParenthesis = true;
                    opt.RouteOptions.EnableKeyAsSegment = true;
                });
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "DebtorsProcessing.Api",
                        Version = "v1"
                    });
                });
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition(nameof(ConfigurationCostants.LoginAuthenticationScheme), new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Description = "Please provide a login token",
                    Name = ConfigurationCostants.LoginAuthenticationScheme
                });

                c.AddSecurityDefinition(nameof(ConfigurationCostants.SessionAuthenticationScheme), new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Description = "Please provide a session token",
                    Name = ConfigurationCostants.SessionAuthenticationScheme
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "bearer"
                            }
                        },
                        Array.Empty<string>()
                    },

                });

            });
        }

        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new();
            EntitySetConfiguration<User> usersEntitySet = builder.EntitySet<User>("Users");
            usersEntitySet.EntityType.Ignore(x => x.PasswordHash);
            usersEntitySet.EntityType.Ignore(x => x.Salt);
            usersEntitySet.EntityType.Ignore(x => x.LoginRefreshTokens);

            EntitySetConfiguration<UserRole> userRolesSet = builder.EntitySet<UserRole>("UserRoles");

            EntitySetConfiguration<UserSession> userSessionsSet = builder.EntitySet<UserSession>("UserSessions");
            userSessionsSet.EntityType.Ignore(x => x.User);
            userSessionsSet.EntityType.Ignore(x => x.SessionRefreshTokens);


            EntitySetConfiguration<Debtor> debtorsSet = builder.EntitySet<Debtor>("Debtors");
            debtorsSet.EntityType.Ignore(x => x.Responsible);
            EntitySetConfiguration<DebtorPayment> debtorPaymentsSet = builder.EntitySet<DebtorPayment>("DebtorPayments");
            debtorPaymentsSet.EntityType.Ignore(x => x.Debtor);
            builder.EntitySet<SecurityObject>("SecurityObjects");
            builder.EntitySet<SecurityJournalEvent>("SecurityJournalEvents");
            builder.EntitySet<SecurityJournalEventType>("SecurityJournalEventTypes");
            return builder.GetEdmModel();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseODataRouteDebug();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DebtorsProcessing.Api v1"));
            }


            app.UseMiddleware<UserExtractorMiddleware>();
            app.UseMiddleware<SessionExtractorMiddleware>();



            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            StartInitHelper.Reset();
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
                .AddEntityFrameworkStores<DatabaseModel.DebtorsContext>();
        }
    }
}
