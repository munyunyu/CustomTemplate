using Microsoft.AspNetCore.Authentication.Cookies;
using Template.Library.Constants;
using Template.Library.Interface;
using Template.Library.Models.POCO;
using Template.Library.Service;
using Template.Portal.Interface;
using Template.Portal.Interface.System;
using Template.Portal.Services;
using Template.Portal.Services.System;

namespace Template.Portal.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static void AddCustomAuthentication(this IServiceCollection services, WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(50);
                options.LoginPath = "/account/login";
            });
        }


        public static void AddCustomScopedServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            //init POCO from appsettings
            services.Configure<ApplicationSettings>(builder.Configuration.GetSection(nameof(ApplicationSettings)));

            
            services.AddScoped<AuthService>();
            services.AddScoped<HelperService>();
            services.AddScoped<IHttpService, HttpService>();

            services.AddScoped<IPortalService, PortalService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAdminService, AdminService>();


            //services.AddHostedService<AuthenticateHostedService>();
        }

        public static void AddCustomAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PortalPolicy.ViewUsersPolicy, policy =>
                {
                    policy
                    .RequireRole(SystemRoles.Admin);
                });
            });
        }

    }
}
