using Coravel.Invocable;
using Microsoft.EntityFrameworkCore;
using Template.Database.Context;

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
        var now = DateTime.UtcNow;

        var today = now.Date;

        var totalUsers = await _context.Users.CountAsync();

        var confirmedUsers = await _context.Users.CountAsync(u => u.EmailConfirmed);

        var lockedUsers = await _context.Users.CountAsync(u => u.LockoutEnd != null && u.LockoutEnd > now);

        var pendingEmails = await _context.TblEmailQueue!.CountAsync(e => e.Status == Library.Enums.Status.Pending);

        var failedEmails24h = await _context.TblEmailQueue!.CountAsync(e => e.Status == Library.Enums.Status.Failed && e.LastUpdatedDate >= today);

        var jobsRun24h = await _context.TblJobHistory!.CountAsync(j => j.StartedAt >= today);

        var failedJobs24h = await _context.TblJobHistory!.CountAsync(j => j.Status == Library.Enums.Status.Failed && j.StartedAt >= today);

        _logger.LogInformation(
            "SystemUsageStats | Users: {Total} (confirmed: {Confirmed}, locked: {Locked}) | " +
            "Emails pending: {PendingEmails}, failed 24h: {FailedEmails} | " +
            "Jobs 24h: {JobsRun}, failed: {FailedJobs}",
            totalUsers, confirmedUsers, lockedUsers,
            pendingEmails, failedEmails24h,
            jobsRun24h, failedJobs24h);

        // TODO: optionally persist to a TblSystemUsageStats table for dashboard queries
        await Task.CompletedTask;
    }
}
