using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MVA.Business.Interface;
using MVA.Business.Service;
using MVA.Business.Shedular.Extensions;
using MVA.Business.Worker;
using MVA.Database.Context;
using MVA.Library.Models.Account;
using MVA.Portal.Services;
using System.ComponentModel.Design;
using System.Security.Claims;
using System.Text;

namespace MVA.Portal.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomIdentity(this IServiceCollection services)
        {
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationContext>();

            //services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationContext>();
        }

        public static void AddCustomAuthentication(this IServiceCollection services, WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(50);
                options.LoginPath = "/account/login";
            });
        }


        public static void AddCustomScopedServices(this IServiceCollection services)
        {
            services.AddHostedService<AuthenticateHostedService>();

            services.AddScoped<IPortalService, PortalService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBatchService, BatchService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<ILoanService, LoanService>();
            services.AddScoped<IWorkerService, WorkerService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
            services.AddScoped<IAdminService, AdminService>();

            services.AddScoped<AuthService>();
            services.AddScoped<HelperService>();
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<IDatabaseService, DatabaseService>();

            //services.AddHostedService<LoanStatusMonitorHostedService>();
            //services.AddHostedService<LoanInterestMonitorHostedService>();
            //services.AddHostedService<LoanReconBatchMonitorHostedService>();

            //services.AddCronJob<LoanReconBatchMonitorHostedService>(c => { c.CronExpression = "0 0 * * *"; });
            //services.AddCronJob<LoanStatusMonitorHostedService>(c => { c.CronExpression = "30 0 * * *"; });
            //services.AddCronJob<LoanInterestMonitorHostedService>(c => { c.CronExpression = "0 1 * * *"; });
       
        }

    }
}
