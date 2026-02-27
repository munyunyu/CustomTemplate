using Coravel;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.OpenTelemetry;
using Template.Business.Interfaces.System;
using Template.Business.Services.System;
using Template.Database.Context;
using Template.WorkerService.Extensions;
using Template.WorkerService.Jobs;

IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureServices((hostContext, services) =>
{
    IConfiguration configuration = hostContext.Configuration;

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.OpenTelemetry(x =>
        {
            x.Endpoint = configuration["Serilog:OpenTelemetry:Endpoint"];
            x.Protocol = (OtlpProtocol)Enum.Parse(typeof(OtlpProtocol), configuration["Serilog:OpenTelemetry:Protocol"] ?? "HttpProtobuf");
            x.Headers = new Dictionary<string, string>
            {
                ["X-Seq-ApiKey"] = configuration["Serilog:OpenTelemetry:ApiKey"] ?? ""
            };
            x.ResourceAttributes = new Dictionary<string, object>
            {
                ["service"] = configuration["Serilog:Service"] ?? "Template.WorkerService",
                ["environment"] = hostContext.HostingEnvironment.EnvironmentName,
            };
        })
        .CreateLogger();

    services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);

    services.AddScoped<IDatabaseService, DatabaseService>();

    services.AddScheduler();
    
    services.AddCustomTransientServices();

}).Build();

host.Services.RegisterScheduledJobs(); //Configure schedules job

host.Run();
