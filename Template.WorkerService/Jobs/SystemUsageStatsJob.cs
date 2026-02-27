using Coravel.Invocable;
using Microsoft.EntityFrameworkCore;
using Template.Database.Context;
using Template.Library.Enums;
using Template.Library.Tables.Job;

namespace Template.WorkerService.Jobs;

public class SystemUsageStatsJob : IInvocable
{
    private readonly ILogger<SystemUsageStatsJob> _logger;
    private readonly ApplicationContext _context;

    public SystemUsageStatsJob(ILogger<SystemUsageStatsJob> logger, ApplicationContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task Invoke()
    {
        var history = await BeginJobHistoryAsync(nameof(SystemUsageStatsJob));

        try
        {
            var now = DateTime.UtcNow;

            var today = now.Date;

            var totalUsers = await _context.Users.CountAsync();

            var confirmedUsers = await _context.Users.CountAsync(u => u.EmailConfirmed);

            var lockedUsers = await _context.Users.CountAsync(u => u.LockoutEnd != null && u.LockoutEnd > now);

            var pendingEmails = await _context.TblEmailQueue!.CountAsync(e => e.Status == Status.Pending);

            var failedEmails24h = await _context.TblEmailQueue!.CountAsync(e => e.Status == Status.Failed && e.LastUpdatedDate >= today);

            var jobsRun24h = await _context.TblJobHistory!.CountAsync(j => j.StartedAt >= today);

            var failedJobs24h = await _context.TblJobHistory!.CountAsync(j => j.Status == Status.Failed && j.StartedAt >= today);

            var stats = new TblSystemUsageStats
            {
                Id = Guid.NewGuid(),
                SnapshotDate = now,
                TotalUsers = totalUsers,
                ConfirmedUsers = confirmedUsers,
                LockedUsers = lockedUsers,
                PendingEmails = pendingEmails,
                FailedEmails24h = failedEmails24h,
                JobsRun24h = jobsRun24h,
                FailedJobs24h = failedJobs24h,
                CreatedDate = now,
                LastUpdatedDate = now,
            };

            await _context.TblSystemUsageStats!.AddAsync(stats);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "SystemUsageStats | Users: {Total} (confirmed: {Confirmed}, locked: {Locked}) | " +
                "Emails pending: {PendingEmails}, failed 24h: {FailedEmails} | " +
                "Jobs 24h: {JobsRun}, failed: {FailedJobs}",
                totalUsers, confirmedUsers, lockedUsers,
                pendingEmails, failedEmails24h,
                jobsRun24h, failedJobs24h);

            await EndJobHistoryAsync(history, Status.Success, 1, "Stats snapshot saved");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SystemUsageStatsJob failed");
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
