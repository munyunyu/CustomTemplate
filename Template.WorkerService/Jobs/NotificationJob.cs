using Coravel.Invocable;
using Microsoft.EntityFrameworkCore;
using Template.Database.Context;
using Template.Library.Enums;
using Template.Library.Tables.Job;

namespace Template.WorkerService.Jobs;

public class NotificationJob : IInvocable
{
    private readonly ILogger<NotificationJob> _logger;
    private readonly ApplicationContext _context;

    public NotificationJob(ILogger<NotificationJob> logger, ApplicationContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task Invoke()
    {
        var history = await BeginJobHistoryAsync(nameof(NotificationJob));

        try
        {
            // TODO: add notification delivery logic (SignalR / push / SMS)
            _logger.LogInformation("NotificationJob completed. No issues found");

            await EndJobHistoryAsync(history, Status.Success, 0, "No pending notifications");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "NotificationJob failed");
            await EndJobHistoryAsync(history, Status.Failed, 0, ex.Message);
        }
    }

    private async Task<TblJobHistory?> BeginJobHistoryAsync(string jobName)
    {
        try
        {
            var schedule = await _context.TblJobSchedule!.FirstOrDefaultAsync(x => x.JobName == jobName);

            if (schedule == null) return null;

            schedule.LastRunTime = DateTime.UtcNow;
            schedule.LastRunStatus = Status.InProgress;
            schedule.LastUpdatedDate = DateTime.UtcNow;

            var history = new TblJobHistory
            {
                Id = Guid.NewGuid(),
                JobScheduleId = schedule.Id,
                JobName = jobName,
                StartedAt = DateTime.UtcNow,
                Status = Status.InProgress,
                CreatedDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow,
            };

            await _context.TblJobHistory!.AddAsync(history);
            await _context.SaveChangesAsync();

            return history;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to begin job history for {Job}", jobName);
            return null;
        }
    }

    private async Task EndJobHistoryAsync(TblJobHistory? history, Status status, int affected, string? notes)
    {
        if (history == null) return;

        try
        {
            history.FinishedAt = DateTime.UtcNow;
            history.Status = status;
            history.AffectedRecords = affected;
            history.Notes = notes;
            history.LastUpdatedDate = DateTime.UtcNow;

            var schedule = await _context.TblJobSchedule!.FindAsync(history.JobScheduleId);

            if (schedule != null)
            {
                schedule.LastRunStatus = status;
                schedule.LastUpdatedDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to end job history for {Job}", history.JobName);
        }
    }
}
