using Template.Library.Constants;
using Template.WorkerService;
using Template.WorkerService.Extensions;
using Template.WorkerService.Job.SystemJob;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        //services.AddHostedService<Worker>();

        services.AddCronJob<EmailServiceJob>(c =>
        {
            c.TimeZoneInfo = TimeZoneInfo.Local;
            c.CronExpression = CronExpressions.EveryMinute;
        });
    })
    .Build();

host.Run();
