using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Coravel;
using Template.WorkerService.Jobs;

namespace Template.WorkerService.Extensions;

public static class JobSchedularRegistryExtensions
{
    public static void RegisterScheduledJobs(this IServiceProvider service)
    {
        service.UseScheduler(scheduler =>
        {
            scheduler.Schedule<EmailServiceJob>().EveryFiveMinutes().PreventOverlapping(nameof(EmailServiceJob));

            scheduler.Schedule<DataIntegrityJob>().DailyAtHour(0).PreventOverlapping(nameof(DataIntegrityJob));

            scheduler.Schedule<NotificationJob>().EveryFiveMinutes().PreventOverlapping(nameof(NotificationJob));
            
            scheduler.Schedule<SystemUsageStatsJob>().DailyAtHour(23).PreventOverlapping(nameof(SystemUsageStatsJob));
        })
        .OnError(exception =>
        {
            Console.Error.WriteLine($"Scheduler error: {exception.Message}");
        });
    }
}
