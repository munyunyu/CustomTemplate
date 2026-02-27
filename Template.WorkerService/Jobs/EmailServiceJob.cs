using Coravel.Invocable;
using Template.Business.Interfaces.System;
using Template.Library.Enums;
using Template.Library.Tables.Notification;

namespace Template.WorkerService.Jobs;

public class EmailServiceJob : IInvocable
{
    private readonly ILogger<EmailServiceJob> _logger;
    private readonly IDatabaseService _database;

    public EmailServiceJob(ILogger<EmailServiceJob> logger, IDatabaseService database)
    {
        _logger = logger;

        _database = database;
    }

    public async Task Invoke()
    {
        var pending = await _database.GetAllAsync<TblEmailQueue>(x => x.Status == Status.Pending, count: 50);

        var processed = 0;

        foreach (var email in pending)
        {
            try
            {
                // TODO: inject and call actual SMTP/send logic here
                _logger.LogInformation("Processing email {Id} to {To}", email.Id, email.ToEmailAddresses);

                email.Status = Status.Success;
                email.SendAttempts += 1;
                email.LastUpdatedDate = DateTime.UtcNow;

                await _database.UpdateAsync(email);

                processed++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email {Id}", email.Id);

                email.Status = Status.Failed;
                email.SendAttempts += 1;
                email.LastUpdatedDate = DateTime.UtcNow;

                await _database.UpdateAsync(email);
            }
        }

        if (processed > 0) _logger.LogInformation("EmailServiceJob completed. Processed {Count} email(s)", processed);
    }
}
