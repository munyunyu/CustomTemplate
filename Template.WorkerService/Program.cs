using Coravel;
using Microsoft.EntityFrameworkCore;
using Template.Business.Interfaces.System;
using Template.Business.Services.System;
using Template.Database.Context;
using Template.WorkerService.Extensions;
using Template.WorkerService.Jobs;

IHost host = Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
{
    IConfiguration configuration = hostContext.Configuration;

    services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);

    services.AddScoped<IDatabaseService, DatabaseService>();

    services.AddScheduler();
    
    services.AddCustomTransientServices();

}).Build();

host.Services.RegisterScheduledJobs(); //Configure schedules job

host.Run();
