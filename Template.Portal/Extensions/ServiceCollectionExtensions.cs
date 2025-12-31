using System.ComponentModel.Design;
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
        public static void AddCustomScopedServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            ////init POCO from appsettings
            services.Configure<PortalSettings>(builder.Configuration.GetSection(nameof(PortalSettings)));


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
                    policy.RequireRole(SystemRoles.Admin);
                });
            });
        }
    }
}
