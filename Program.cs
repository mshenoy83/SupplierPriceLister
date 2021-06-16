using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using SuppliesPriceLister.Models;
using SuppliesPriceLister.Services;

namespace SuppliesPriceLister
{
    class Program
    {
        static void Main(string[] args)
        {
            // Your solution begins here
            // Create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create service provider
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();


            // entry to run app
            var maintask = serviceProvider.GetService<App>();
            maintask.Run(Assembly.GetExecutingAssembly().GetManifestResourceNames());
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
            serviceCollection.AddTransient<IFileProcessor, CSVFileProcessor>();
            serviceCollection.AddTransient<IFileProcessor, JsonFileProcessor>();
            serviceCollection.AddTransient<IDataPrinterService, DataPrinterService>();
            serviceCollection.AddTransient<IFileProcessorStrategyService, FileProcessorStrategyService>();
            serviceCollection.AddTransient<ICurrencyConverter, CurrencyConverter>();

            // Add logging
            serviceCollection.AddLogging(logging => logging.AddSerilog(dispose: true));


            // Add app
            serviceCollection.AddTransient<App>();
        }
    }
}