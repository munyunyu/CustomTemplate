using Coravel.Invocable;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Template.Database.Context;
using Template.Library.Enums;
using Template.Library.Tables.Job;
using Template.Library.Tables.Notification;

namespace Template.WorkerService.Jobs;

public class EmailServiceJob : IInvocable
{
    private const int MaxSendAttempts = 3;

    private readonly ILogger<EmailServiceJob> _logger;
    private readonly ApplicationContext _context;

    public EmailServiceJob(ILogger<EmailServiceJob> logger, ApplicationContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task Invoke()
    {
        var history = await BeginJobHistoryAsync(nameof(EmailServiceJob));

        try
        {
            var pending = await _context.TblEmailQueue!
                .Where(x => x.Status == Status.Pending || (x.Status == Status.Failed && x.SendAttempts < MaxSendAttempts))
                .OrderBy(x => x.CreatedDate)
                .Take(50)
                .ToListAsync();

            if (pending.Count == 0)
            {
                await EndJobHistoryAsync(history, Status.Success, 0, "No pending emails");
                return;
            }

            var config = await _context.TblEmailConfig!.FirstOrDefaultAsync();

            if (config == null)
            {
                _logger.LogError("EmailServiceJob: no email config found in TblEmailConfig");
                await EndJobHistoryAsync(history, Status.Failed, 0, "No email config found");
                return;
            }

            var processed = 0;

            foreach (var email in pending)
            {
                try
                {
                    await SendEmailAsync(config, email);

                    email.Status = Status.Success;
                    email.SendAttempts += 1;
                    email.LastUpdatedDate = DateTime.UtcNow;

                    processed++;

                    _logger.LogInformation("Sent email {Id} to {To}", email.Id, email.ToEmailAddresses);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send email {Id} (attempt {Attempt})", email.Id, email.SendAttempts + 1);

                    email.SendAttempts += 1;
                    email.Status = email.SendAttempts >= MaxSendAttempts ? Status.Failed : Status.Pending;
                    email.LastUpdatedDate = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();

            await EndJobHistoryAsync(history, Status.Success, processed, $"Processed {processed}/{pending.Count} email(s)");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "EmailServiceJob failed");
            await EndJobHistoryAsync(history, Status.Failed, 0, ex.Message);
        }
    }

    private static async Task SendEmailAsync(TblEmailConfig config, TblEmailQueue email)
    {
        var message = new MimeMessage();

        message.From.Add(MailboxAddress.Parse(config.SmtpUser!));
        message.To.AddRange(email.ToEmailAddresses!.Split(';', ',').Select(e => MailboxAddress.Parse(e.Trim())));

        if (!string.IsNullOrWhiteSpace(email.CCEmailAddresses))
            message.Cc.AddRange(email.CCEmailAddresses.Split(';', ',').Select(e => MailboxAddress.Parse(e.Trim())));

        message.Subject = email.Subject ?? string.Empty;
        message.Body = new TextPart("html") { Text = email.Body ?? string.Empty };

        using var client = new SmtpClient();

        await client.ConnectAsync(config.SmtpServer!, config.SmtpPort, config.SmtpEnableSsl);
        await client.AuthenticateAsync(config.SmtpUser, config.SmtpPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
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
