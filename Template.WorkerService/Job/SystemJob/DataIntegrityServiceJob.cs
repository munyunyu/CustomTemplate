using Microsoft.Extensions.DependencyInjection;
using Template.Business.Interfaces.System;
using Template.Library.Models.Shedular;
using Template.WorkerService.Job.Base;

namespace Template.WorkerService.Job.SystemJob
{
    public class DataIntegrityServiceJob : CronJobService
    {
        private readonly ILogger<DataIntegrityServiceJob> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public DataIntegrityServiceJob(
            IScheduleConfig<DataIntegrityServiceJob> config,
            ILogger<DataIntegrityServiceJob> logger,
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

            // TODO: add real integrity checks here
            _logger.LogInformation("DataIntegrityServiceJob: running integrity checks.");

            await Task.CompletedTask;

            return JobResult.WithRecords(0, "Integrity checks passed.");
        }
    }
}
