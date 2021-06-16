using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SuppliesPriceLister.Models;
using SuppliesPriceLister.Services;

namespace SuppliesPriceLister
{
    public class App
    {
        private readonly ConcurrentBag<List<PrintModel>> _printModels;
        private readonly ILogger<App> _logger;
        private readonly IFileProcessorStrategyService _fileProcessorStrategy;
        private readonly IDataPrinterService _printerService;

        public App(ILogger<App> logger, IFileProcessorStrategyService fileProcessorStrategy, IDataPrinterService printerService)
        {
            _logger = logger;
            _fileProcessorStrategy = fileProcessorStrategy;
            _printModels = new ConcurrentBag<List<PrintModel>>();
            _printerService = printerService;
        }

        public void Run(string[] fileNames)
        {
            _logger.LogInformation("Begin processing files.");

            if (fileNames == null)
            {
                _logger.LogInformation("No files sent for processing. Exiting file processor");
                Environment.Exit(-1);
            }


            Parallel.ForEach(fileNames,file =>
            {
                var extension = Path.GetExtension(file);
                var fileprocessor = _fileProcessorStrategy.GetFileProcessor(extension);
                if (fileprocessor == null)
                {
                    _logger.LogWarning("No file processor found for extensiontype : {extension}", extension);
                    return;
                }
                
                _printModels.Add(fileprocessor.ProcessFile(file));
            });

            var finalList = new List<PrintModel>();
            foreach (var printArray in _printModels)
            {
                finalList.AddRange(printArray.ToList());
            }
            
            _printerService.PrintData(finalList);
            
            _logger.LogInformation("File processing complete.");
        }
    }
}