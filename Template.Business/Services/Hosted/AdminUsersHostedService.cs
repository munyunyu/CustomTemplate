using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Business.Interfaces.System;
using Template.Library.Constants;
using Template.Library.Tables.Views;

namespace Template.Business.Services.Hosted
{
    public class AdminUsersHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<AdminUsersHostedService> logger;
        private readonly IConfiguration configuration;
        private readonly IServiceScopeFactory scopeFactory;
        private Timer _timer;

        public AdminUsersHostedService(ILogger<AdminUsersHostedService> logger, IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            //logger.LogInformation("Timed Hosted Service running. {time}", DateTime.Now);

            //var minutes = configuration.GetValue<int>("AuthenticateService:FromMinutes");

            //_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));

            // Start the timer with a callback to an async method
            _timer = new Timer(async (x) => await DoWorkAsync(), null, TimeSpan.Zero, TimeSpan.FromMinutes(5));

            return Task.CompletedTask;
        }

        private async Task DoWorkAsync()
        {
            try
            {
                using var scope = scopeFactory.CreateScope();

                var database = scope.ServiceProvider.GetRequiredService<IDatabaseService>();

                var admin_users = await database.GetAllAsync<ViewApplicationUser>();

                SystemUsers.Admins = admin_users;

            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine($"Exception in timer: {ex.Message}");
            }
        }

        //private void DoWork(object state)
        //{

        //    var model = new RequestLogin
        //    {
        //        Password = configuration["Loyalty:Password"],
        //        StoreId = configuration["Loyalty:StoreId"],
        //        Username = configuration["Loyalty:Username"],
        //    };

        //    var request = Loyalty.LoginAsync(model).Result;

        //    Tokens.LoyaltyToken = request?.AccessToken;

        //    if (string.IsNullOrEmpty(Tokens.LoyaltyToken))
        //    {
        //        //logit
        //    }
        //}

        public Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Timed Hosted Service is stopping. {time}", DateTime.Now);

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
