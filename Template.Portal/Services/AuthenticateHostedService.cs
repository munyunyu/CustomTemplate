using MVA.Business.Interface;
using MVA.Business.Service;
using MVA.Library.Constants;
using MVA.Library.Enums;
using MVA.Library.Extensions;
using MVA.Library.Models;
using MVA.Library.Models.Account;

namespace MVA.Portal.Services
{
    public class AuthenticateHostedService : IHostedService, IDisposable
    {
        private readonly IConfiguration configuration;
        private readonly HttpService httpService;
        private Timer? _timer;

        public AuthenticateHostedService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.httpService = new HttpService();
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            //logger.LogInformation("Timed Hosted Service running. {time}", DateTime.Now);

            var minutes = configuration.GetValue<int>("AuthenticateService:FromMinutes");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(minutes));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            try
            {
                var model = new
                {
                    Password = configuration["AuthenticateService:Password"],
                    email = configuration["AuthenticateService:Username"],
                };

                  var response = httpService.HttpPost<Response<ServiceLoginResponse>>("/api/Account/Login", model);

                if (response.Code == Status.Success)
                {
                    Config.IveriToken = response?.Payload?.Token ?? string.Empty;

                    Config.IveriToken.LogInfo(header: "Access token");
                }
                else
                {
                    Config.IveriToken = string.Empty;

                    response?.Message?.LogInfo(header: "Failed to get access token");
                }


                //Check if service is allowed to run

                var ServiceRun = configuration.GetValue<bool>("AuthenticateService:IsRunning");

                if (ServiceRun == false)
                {
                    StopAsync(new CancellationToken());
                }
            }
            catch (Exception ex)
            {
                ex.LogInfo("Failed to get access token");
            }

        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            string message = "Timed Hosted Service is stopping";

            message.LogInfo("Auth service stopping");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
