using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Library.Models.Shedular;
using Template.WorkerService.Job.Base;

namespace Template.WorkerService.Job
{
    public class EmailServiceJob : CronJobService
    {
        private readonly ILogger<EmailServiceJob> _logger;
        private readonly IServiceProvider _serviceProvider;
        public EmailServiceJob(IScheduleConfig<EmailServiceJob> config, ILogger<EmailServiceJob> logger, IServiceProvider serviceProvider) : base(config.CronExpression, config.TimeZoneInfo, logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            //if (config.GetType().GenericTypeArguments[0].Name != GetType().Name)throw new ArgumentException("Incorrect JobType name for IScheduleConfig.");

            //if (logger.GetType().GenericTypeArguments[0].Name != GetType().Name)throw new ArgumentException("Incorrect JobType name for ILogger.");

        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{now} CronJob 1 is working.", DateTime.Now.ToString("T"));
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
