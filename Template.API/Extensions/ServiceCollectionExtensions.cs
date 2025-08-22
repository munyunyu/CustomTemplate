using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.OpenTelemetry;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Template.Business.Interfaces;
using Template.Business.Interfaces.Profile;
using Template.Business.Interfaces.System;
using Template.Business.Services;
using Template.Business.Services.Hosted;
using Template.Business.Services.Profile;
using Template.Business.Services.System;
using Template.Database.Context;

namespace Template.Service.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(
               option =>
               {
                   option.Password.RequireDigit = true;
                   option.Password.RequiredLength = 6;
                   option.Password.RequireNonAlphanumeric = true;
                   option.Password.RequireUppercase = true;
                   option.Password.RequireLowercase = true;
                   option.User.RequireUniqueEmail = true;

                   // Require email confirmation
                   option.SignIn.RequireConfirmedEmail = true;

                   //added
                   option.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.Sub;

               })
               .AddEntityFrameworkStores<ApplicationContext>()
               .AddDefaultTokenProviders();
        }

        public static void AddCustomAuthentication(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"] ?? string.Empty));

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    //what to validate
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    //setup validate data
                    ValidAudience = builder.Configuration["Jwt:Site"],
                    ValidIssuer = builder.Configuration["Jwt:Site"],
                    IssuerSigningKey = symmetricSecurityKey
                };
            });
        }

        public static void AddCustomScopedServices(this IServiceCollection services)
        {
            //System
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICommunicationService, CommunicationService>();
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<IAdminService, AdminService>();

            //Other
            services.AddScoped<IPortalService, PortalService>();            
            
            
            //Profile
            services.AddScoped<IProfileService, ProfileService>();
          
            //services.AddHostedService<AdminUsersHostedService>();

        }

        public static void AddCustomSeriLogging(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                //.MinimumLevel.Information()
                .WriteTo.OpenTelemetry(x =>
                {
                    x.Endpoint = configuration["Serilog:OpenTelemetry:Endpoint"];
                    x.Protocol = (OtlpProtocol)Enum.Parse(typeof(OtlpProtocol), configuration["Serilog:OpenTelemetry:Protocol"] ?? "Protocol N/A");
                    x.Headers = new Dictionary<string, string>
                    {
                        ["X-Seq-ApiKey"] = configuration["Serilog:OpenTelemetry:ApiKey"] ?? "X-Seq-ApiKey N/A"
                    };
                    x.ResourceAttributes = new Dictionary<string, object>
                    {
                        ["service"] = configuration["Serilog:Service"] ?? "ServiceName N/A",
                        ["environment"] = builder.Environment.EnvironmentName,
                    };
                })
                .CreateLogger();

            builder.Services.AddSerilog();

            //builder.Logging.ClearProviders();
            ////builder.Logging.AddOpenTelemetry(x => x.AddConsoleExporter());
            //builder.Logging.AddOpenTelemetry(x => x.AddOtlpExporter(a =>
            //{
            //    x.SetResourceBuilder(ResourceBuilder.CreateEmpty()
            //        .AddService("ZivoService")
            //        .AddAttributes(new Dictionary<string, object>()
            //        {
            //            ["deployment.environment"] = builder.Environment.EnvironmentName,
            //        }));

            //    x.IncludeScopes = true;
            //    x.IncludeFormattedMessage = true;

            //    a.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/logs");
            //    a.Protocol = OtlpExportProtocol.HttpProtobuf;
            //    a.Headers = "X-Seq-ApiKey=INmmu3OOmpBAYxD4tzFk";
            //}));
        }

        public static void AddCustomHealthChecks(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddHealthChecks()
                .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!, name: "Sql Health")
                .AddCheck<CustomHealthCheck>("CustomHealthCheck");

            //builder.Services.AddHealthChecksUI()
        }
    }
}
