using Microsoft.Extensions.DependencyInjection;
using Template.Business.Interfaces.System;
using Template.Library.Enums;
using Template.Library.Models.Queue;
using Template.Library.Tables.Notification;

namespace Template.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBackgroundTaskQueue<EmailQueueMessage> _emailQueue;
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(
            ILogger<Worker> logger,
            IBackgroundTaskQueue<EmailQueueMessage> emailQueue,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _emailQueue = emailQueue;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Email queue worker started");

            await foreach (var message in _emailQueue.DequeueAllAsync(stoppingToken))
            {
                try
                {
                    _logger.LogInformation("Processing email queue item: {EmailQueueId}", message.EmailQueueId);

                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<IDatabaseService>();

                    var emailEntry = await db.GetAsync<TblEmailQueue>(x => x.Id == message.EmailQueueId);
                    if (emailEntry == null)
                    {
                        _logger.LogWarning("Email queue entry {EmailQueueId} not found", message.EmailQueueId);
                        continue;
                    }

                    // TODO: Implement actual SMTP sending via MailKit using TblEmailConfig
                    emailEntry.SendAttempts++;
                    emailEntry.Status = Status.Success;
                    emailEntry.LastUpdatedDate = DateTime.UtcNow;

                    await db.UpdateAsync(emailEntry);

                    _logger.LogInformation("Email queue item {EmailQueueId} processed successfully", message.EmailQueueId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process email queue item: {EmailQueueId}", message.EmailQueueId);
                }
            }
        }
    }
}
