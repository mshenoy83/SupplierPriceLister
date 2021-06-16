using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using SuppliesPriceLister.Models;

namespace SuppliesPriceLister
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Your solution begins here
            // Create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create service provider
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();


            // entry to run app
            var maintask = serviceProvider.GetService<App>();
            await maintask.RunAsync(Assembly.GetExecutingAssembly().GetManifestResourceNames());
        }

        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            var environment = Environment.GetEnvironmentVariable("LLGFEED_ENVIRONMENT") ?? "Local";
            // Build configuration
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var template =
                "{Timestamp:yyyy:MM:dd HH:mm:ss.fff} [{Level:4}] [{Step}] {Message}{NewLine}{Exception}";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("System", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File("log-{Date}.log", outputTemplate: template)
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("ApplicationName", ApplicationConstants.AppName).CreateLogger();
            serviceCollection.Configure<ApplicationSettings>(configuration.GetSection(nameof(ApplicationSettings)));

            // Add access to generic IConfiguration
            serviceCollection.AddSingleton(configuration);


            // Add logging
            serviceCollection.AddLogging(logging => logging.AddSerilog(dispose: true));


            // Add app
            serviceCollection.AddTransient<App>();
        }
    }
}