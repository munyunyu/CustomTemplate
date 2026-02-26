using Microsoft.EntityFrameworkCore;
using Template.Business.Interfaces.System;
using Template.Business.Services.System;
using Template.Database.Context;
using Template.Library.Constants;
using Template.Library.Models.POCO;
using Template.WorkerService;
using Template.WorkerService.Extensions;
using Template.WorkerService.Job.SystemJob;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;

        // ── Database ──────────────────────────────────────────────────────────
        services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")),
            ServiceLifetime.Scoped);

        // ── Business services ─────────────────────────────────────────────────
        services.AddScoped<IDatabaseService, DatabaseService>();

        // ── RabbitMQ ──────────────────────────────────────────────────────────
        services.Configure<RabbitMQSettings>(configuration.GetSection(nameof(RabbitMQSettings)));
        services.AddSingleton<IRabbitMQService, RabbitMQService>();

        // ── Background worker (RabbitMQ consumer) ─────────────────────────────
        services.AddHostedService<Worker>();

        // ── Cron jobs ─────────────────────────────────────────────────────────
        services.AddCronJob<EmailServiceJob>(c =>
        {
            c.TimeZoneInfo = TimeZoneInfo.Local;
            c.CronExpression = CronExpressions.Every5Minutes;
        });

        services.AddCronJob<DataIntegrityServiceJob>(c =>
        {
            c.TimeZoneInfo = TimeZoneInfo.Local;
            c.CronExpression = CronExpressions.DailyAt0030;
        });
    })
    .Build();

host.Run();
