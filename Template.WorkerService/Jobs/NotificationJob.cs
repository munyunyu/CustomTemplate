using Coravel.Invocable;
using Template.Business.Interfaces.System;
using Template.Library.Enums;
using Template.Library.Tables.Notification;

namespace Template.WorkerService.Jobs;

public class NotificationJob : IInvocable
{
    private readonly ILogger<NotificationJob> _logger;
    private readonly IDatabaseService _database;

    public NotificationJob(ILogger<NotificationJob> logger, IDatabaseService database)
    {
        _logger = logger;
        _database = database;
    }

    public async Task Invoke()
    {
        _logger.LogInformation("NotificationJob completed. No issues found");

        await Task.CompletedTask;
    }
}
