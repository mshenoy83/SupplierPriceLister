using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;
using SuppliesPriceLister.Services;

namespace SuppliesPriceLister
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly IFileProcessorStrategyService _fileProcessorStrategy;

        public App(ILogger<App> logger, IFileProcessorStrategyService fileProcessorStrategy)
        {
            _logger = logger;
            _fileProcessorStrategy = fileProcessorStrategy;
        }

        public async Task RunAsync(string[] fileNames)
        {
            _logger.LogInformation("Begin processing files.");

            if (fileNames == null)
            {
                _logger.LogInformation("No files sent for processing. Exiting file processor");
                Environment.Exit(-1);
            }


            foreach (var file in fileNames)
            {
                var extension = Path.GetExtension(file);
                var fileprocessor = _fileProcessorStrategy.GetFileProcessor(extension);
                if (fileprocessor == null)
                {
                    _logger.LogWarning("No file processor found for extensiontype : {extension}", extension);
                    continue;
                }

                await fileprocessor.ProcessFile(file);
            }
            _logger.LogInformation("File processing complete.");
        }
    }
}