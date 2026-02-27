using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

InitializeDependencyInjection(args);

//code here

// Get the application's startup path (output directory)
string startupPath = AppDomain.CurrentDomain.BaseDirectory;








#region Dependency Injection
static void InitializeDependencyInjection(string[] args)
{
    // Build a Generic Host to enable Dependency Injection, Configuration, and Logging
    var host = Host.CreateDefaultBuilder(args)
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
                        ["service"] = configuration["Serilog:Service"] ?? "Template.Console",
                        ["environment"] = hostContext.HostingEnvironment.EnvironmentName,
                    };
                })
                .CreateLogger();

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