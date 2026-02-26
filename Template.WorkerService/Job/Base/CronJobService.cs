using Cronos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Template.Library.Enums;
using Template.Library.Tables.Job;

namespace Template.WorkerService.Job.Base
{
    /// <summary>
    /// Returned by DoWork() to report how many records were affected and any notes.
    /// </summary>
    public class JobResult
    {
        public int AffectedRecords { get; set; } = 0;
        public string? Notes { get; set; }

        public static JobResult Empty => new();
        public static JobResult WithRecords(int count, string? notes = null) => new() { AffectedRecords = count, Notes = notes };
    }

    public abstract class CronJobService : IHostedService, IDisposable
    {
        private readonly string _cronExpression;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private System.Timers.Timer? _timer;
        private readonly CronExpression _expression;
        private Task? _executingTask;
        private CancellationTokenSource _stoppingCts = new();
        private readonly SemaphoreSlim _schedulerCycle = new(0);

        private Guid _scheduleId = Guid.Empty;

        protected CronJobService(string cronExpression, TimeZoneInfo timeZoneInfo, ILogger logger, IServiceScopeFactory scopeFactory)
        {
            _cronExpression = cronExpression;
            _timeZoneInfo = timeZoneInfo;
            _logger = logger;
            _scopeFactory = scopeFactory;

            _expression = CronExpression.Parse(cronExpression, CronFormat.IncludeSeconds);
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{jobName}: started with expression [{expression}].", GetType().Name, _cronExpression);
            _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            _scheduleId = await UpsertScheduleAsync(cancellationToken);

            _executingTask = ScheduleJob(_stoppingCts.Token);
        }

        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
                    if (!next.HasValue) continue;

                    _logger.LogInformation("{jobName}: next run at {nextRun}", GetType().Name, next.Value.ToString("s"));

                    await UpdateNextRunTimeAsync(next.Value.UtcDateTime, cancellationToken);

                    var delay = next.Value - DateTimeOffset.Now;
                    if (delay.TotalMilliseconds <= 0)
                    {
                        _logger.LogWarning("{jobName}: next run is in the past, skipping.", GetType().Name);
                        continue;
                    }

                    _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                    _timer.Elapsed += async (_, _) =>
                    {
                        try
                        {
                            _timer?.Dispose();
                            _timer = null;

                            if (!cancellationToken.IsCancellationRequested)
                            {
                                await ExecuteWithLoggingAsync(cancellationToken);
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            _logger.LogInformation("{jobName}: cancelled.", GetType().Name);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "{jobName}: unhandled error in timer.", GetType().Name);
                        }
                        finally
                        {
                            _schedulerCycle.Release();
                        }
                    };
                    _timer.Start();
                    await _schedulerCycle.WaitAsync(cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("{jobName}: stopping scheduler loop.", GetType().Name);
            }
        }

        private async Task ExecuteWithLoggingAsync(CancellationToken cancellationToken)
        {
            var historyId = await BeginHistoryAsync(cancellationToken);
            var startedAt = DateTime.UtcNow;
            var result = JobResult.Empty;
            var status = Status.Success;
            string? errorMessage = null;

            try
            {
                _logger.LogInformation("{jobName}: executing...", GetType().Name);
                result = await DoWork(cancellationToken);
                _logger.LogInformation("{jobName}: completed. Affected records: {count}", GetType().Name, result.AffectedRecords);
            }
            catch (OperationCanceledException)
            {
                status = Status.Cancelled;
                _logger.LogInformation("{jobName}: cancelled during execution.", GetType().Name);
            }
            catch (Exception ex)
            {
                status = Status.Failed;
                errorMessage = ex.Message;
                _logger.LogError(ex, "{jobName}: failed.", GetType().Name);
            }
            finally
            {
                await EndHistoryAsync(historyId, startedAt, status, result, errorMessage, cancellationToken);
            }
        }

        /// <summary>
        /// Override this in each job. Return JobResult with the count of affected records.
        /// </summary>
        public virtual Task<JobResult> DoWork(CancellationToken cancellationToken)
        {
            return Task.FromResult(JobResult.Empty);
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{jobName}: stopping...", GetType().Name);
            _timer?.Stop();
            _timer?.Dispose();
            _stoppingCts.Cancel();
            _logger.LogInformation("{jobName}: stopped.", GetType().Name);
            await Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            _timer?.Dispose();
            _executingTask?.Dispose();
            _schedulerCycle.Dispose();
            _stoppingCts.Dispose();
            GC.SuppressFinalize(this);
        }

        // ─── DB helpers ─────────────────────────────────────────────────────────

        private async Task<Guid> UpsertScheduleAsync(CancellationToken ct)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<Database.Context.ApplicationContext>();

                var jobName = GetType().Name;
                var existing = await db.TblJobSchedule!
                    .FirstOrDefaultAsync(x => x.JobName == jobName, ct);

                if (existing != null)
                {
                    existing.CronExpression = _cronExpression;
                    existing.IsEnabled = true;
                    existing.LastUpdatedDate = DateTime.UtcNow;
                    db.TblJobSchedule!.Update(existing);
                    await db.SaveChangesAsync(ct);
                    return existing.Id;
                }

                var schedule = new TblJobSchedule
                {
                    Id = Guid.NewGuid(),
                    JobName = jobName,
                    CronExpression = _cronExpression,
                    IsEnabled = true,
                    Description = $"Auto-registered on startup: {DateTime.UtcNow:s}",
                    LastRunStatus = Status.Pending,
                    CreatedDate = DateTime.UtcNow,
                    LastUpdatedDate = DateTime.UtcNow,
                    CreatedById = Guid.Empty,
                    LastUpdatedById = Guid.Empty,
                };

                await db.TblJobSchedule!.AddAsync(schedule, ct);
                await db.SaveChangesAsync(ct);
                return schedule.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{jobName}: failed to upsert schedule record.", GetType().Name);
                return Guid.Empty;
            }
        }

        private async Task UpdateNextRunTimeAsync(DateTime nextRun, CancellationToken ct)
        {
            if (_scheduleId == Guid.Empty) return;
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<Database.Context.ApplicationContext>();

                var schedule = await db.TblJobSchedule!.FindAsync(new object[] { _scheduleId }, ct);
                if (schedule == null) return;

                schedule.NextRunTime = nextRun;
                schedule.LastUpdatedDate = DateTime.UtcNow;
                db.TblJobSchedule!.Update(schedule);
                await db.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "{jobName}: failed to update next run time.", GetType().Name);
            }
        }

        private async Task<Guid> BeginHistoryAsync(CancellationToken ct)
        {
            if (_scheduleId == Guid.Empty) return Guid.Empty;
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<Database.Context.ApplicationContext>();

                var history = new TblJobHistory
                {
                    Id = Guid.NewGuid(),
                    JobScheduleId = _scheduleId,
                    JobName = GetType().Name,
                    StartedAt = DateTime.UtcNow,
                    Status = Status.InProgress,
                    AffectedRecords = 0,
                    CreatedDate = DateTime.UtcNow,
                    LastUpdatedDate = DateTime.UtcNow,
                    CreatedById = Guid.Empty,
                    LastUpdatedById = Guid.Empty,
                };

                await db.TblJobHistory!.AddAsync(history, ct);
                await db.SaveChangesAsync(ct);
                return history.Id;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "{jobName}: failed to begin history record.", GetType().Name);
                return Guid.Empty;
            }
        }

        private async Task EndHistoryAsync(Guid historyId, DateTime startedAt, Status status, JobResult result, string? errorMessage, CancellationToken ct)
        {
            if (historyId == Guid.Empty && _scheduleId == Guid.Empty) return;
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<Database.Context.ApplicationContext>();

                if (historyId != Guid.Empty)
                {
                    var history = await db.TblJobHistory!.FindAsync(new object[] { historyId }, CancellationToken.None);
                    if (history != null)
                    {
                        history.FinishedAt = DateTime.UtcNow;
                        history.Status = status;
                        history.AffectedRecords = result.AffectedRecords;
                        history.Notes = result.Notes;
                        history.ErrorMessage = errorMessage;
                        history.LastUpdatedDate = DateTime.UtcNow;
                        db.TblJobHistory!.Update(history);
                    }
                }

                if (_scheduleId != Guid.Empty)
                {
                    var schedule = await db.TblJobSchedule!.FindAsync(new object[] { _scheduleId }, CancellationToken.None);
                    if (schedule != null)
                    {
                        schedule.LastRunTime = startedAt;
                        schedule.LastRunStatus = status;
                        schedule.LastUpdatedDate = DateTime.UtcNow;
                        db.TblJobSchedule!.Update(schedule);
                    }
                }

                await db.SaveChangesAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "{jobName}: failed to finalise history record.", GetType().Name);
            }
        }
    }
}
