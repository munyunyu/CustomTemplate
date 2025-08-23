using Template.Business.Interfaces.System;
using Template.Business.Services.System;
using Template.Library.Constants;
using Template.Library.Models.POCO;
using Template.WorkerService;
using Template.WorkerService.Extensions;
using Template.WorkerService.Job.SystemJob;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;

        services.AddHostedService<Worker>();

        services.Configure<RabbitMQSettings>(configuration.GetSection(nameof(RabbitMQSettings)));
        services.AddSingleton<IRabbitMQService, RabbitMQService>();

        ////services.AddCronJob<EmailServiceJob>(c =>
        ////{
        ////    c.TimeZoneInfo = TimeZoneInfo.Local;
        ////    c.CronExpression = CronExpressions.EveryMinute;
        ////});
    })
    .Build();

host.Run();
