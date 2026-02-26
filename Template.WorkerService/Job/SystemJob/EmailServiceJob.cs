using Microsoft.Extensions.DependencyInjection;
using Template.Business.Interfaces.System;
using Template.Library.Enums;
using Template.Library.Models.Shedular;
using Template.Library.Tables.Notification;
using Template.WorkerService.Job.Base;

namespace Template.WorkerService.Job.SystemJob
{
    public class EmailServiceJob : CronJobService
    {
        private readonly ILogger<EmailServiceJob> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public EmailServiceJob(
            IScheduleConfig<EmailServiceJob> config,
            ILogger<EmailServiceJob> logger,
            IServiceScopeFactory scopeFactory)
            : base(config.CronExpression, config.TimeZoneInfo, logger, scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public override async Task<JobResult> DoWork(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<IDatabaseService>();

            var pending = await database.GetAllAsync<TblEmailQueue>(
                x => x.Status == Status.Pending,
                count: 50);

            var processed = 0;

            foreach (var email in pending)
            {
                if (cancellationToken.IsCancellationRequested) break;

                try
                {
                    // TODO: inject and call actual SMTP/send logic here
                    _logger.LogInformation("EmailServiceJob: processing email {id} to {to}", email.Id, email.ToEmailAddresses);

                    email.Status = Status.Success;
                    email.SendAttempts += 1;
                    email.LastUpdatedDate = DateTime.UtcNow;

                    await database.UpdateAsync(email);
                    processed++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "EmailServiceJob: failed to process email {id}", email.Id);

                    email.Status = Status.Failed;
                    email.SendAttempts += 1;
                    email.LastUpdatedDate = DateTime.UtcNow;
                    await database.UpdateAsync(email);
                }
            }

            return JobResult.WithRecords(processed, $"Processed {processed} pending email(s).");
        }
    }
}
