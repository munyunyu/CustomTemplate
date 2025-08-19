using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cronos;

namespace Template.WorkerService.Job.Base
{
    public abstract class CronJobService: IHostedService, IDisposable
    {
        private readonly string cronExpression;
        private readonly TimeZoneInfo timeZoneInfo;
        private readonly ILogger logger;

        private System.Timers.Timer? _timer;
        private readonly CronExpression _expression;
        private Task? _executingTask;
        private CancellationTokenSource _stoppingCts = new();
        private readonly SemaphoreSlim _schedulerCycle = new(0);
        

        public CronJobService(string cronExpression, TimeZoneInfo timeZoneInfo, ILogger logger)
        {
            this.cronExpression = cronExpression;
            this.timeZoneInfo = timeZoneInfo;
            this.logger = logger;

            _expression = CronExpression.Parse(cronExpression);
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("{jobName}: started with expression [{expression}].", GetType().Name, cronExpression);
            _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            _executingTask = ScheduleJob(_stoppingCts.Token);
            return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
        }

        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var next = _expression.GetNextOccurrence(DateTimeOffset.Now, timeZoneInfo);
                    if (!next.HasValue) continue;

                    logger.LogInformation("{jobName}: scheduled next run at {nextRun}", GetType().Name, next.ToString());
                    var delay = next.Value - DateTimeOffset.Now;
                    if (delay.TotalMilliseconds <= 0) // prevent non-positive values from being passed into Timer
                    {
                        logger.LogInformation("{LoggerName}: scheduled next run is in the past. Moving to next.", GetType().Name);
                        continue;
                    }

                    _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                    _timer.Elapsed += async (_, _) =>
                    {
                        try
                        {
                            _timer.Dispose(); // reset and dispose timer
                            _timer = null;

                            if (!cancellationToken.IsCancellationRequested)
                            {
                                await DoWork(cancellationToken);
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            logger.LogInformation("{LoggerName}: job received cancellation signal, stopping...", GetType().Name);
                        }
                        catch (Exception e)
                        {
                            logger.LogError(e, "{LoggerName}: an error happened during execution of the job", GetType().Name);
                        }
                        finally
                        {
                            _schedulerCycle.Release(); // Let the outer loop know that the next occurrence can be calculated.
                        }
                    };
                    _timer.Start();
                    await _schedulerCycle.WaitAsync(cancellationToken); // Wait nicely for any timer result.
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("{LoggerName}: job received cancellation signal, stopping...", GetType().Name);
            }
        }

        public virtual async Task DoWork(CancellationToken cancellationToken)
        {
            await Task.Delay(5000, cancellationToken);  // do the work

            logger.LogInformation("{LoggerName}: job was executed in 5000ms", GetType().Name);

        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("{jobName}: stopping...", GetType().Name);
            _timer?.Stop();
            _timer?.Dispose();
            _stoppingCts.Cancel();
            logger.LogInformation("{jobName}: stopped.", GetType().Name);

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
    }
}
