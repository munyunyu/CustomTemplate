using System;
using System.Collections.Generic;
using System.Text;
using Template.WorkerService.Jobs;

namespace Template.WorkerService.Extensions;

public static class ServiceCollectionExtensions
{
    // ── Register jobs (transient so each run gets a fresh scope) ──────────
    public static void AddCustomTransientServices(this IServiceCollection services)
    {
        services.AddTransient<EmailServiceJob>();
        services.AddTransient<DataIntegrityJob>();
        services.AddTransient<NotificationJob>();
        services.AddTransient<SystemUsageStatsJob>();
    }
}
