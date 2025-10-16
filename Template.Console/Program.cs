using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


InitializeDependencyInjection(args);

//code here

// Get the application's startup path (output directory)
string startupPath = AppDomain.CurrentDomain.BaseDirectory;












#region Dependency Injection
static void InitializeDependencyInjection(string[] args)
{
    // Build a Generic Host to enable Dependency Injection, Configuration, and Logging
    var host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            IConfiguration configuration = hostContext.Configuration;

            // Register application services here
            // Example:
            // services.AddScoped<IMyService, MyService>();

            // If you need configuration-bound options classes, you can do:
            // services.Configure<MyOptions>(configuration.GetSection("MyOptions"));
        })
        .Build();

    // Resolve and run your application logic from DI
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("Template.Console");

        logger.LogInformation("Console started with DI in place.");

        // TODO: Resolve and invoke your entry service here, e.g.:
        // var app = services.GetRequiredService<IApp>();
        // await app.RunAsync();
    }
}

#endregion