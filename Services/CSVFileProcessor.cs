using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using CsvHelper;
using Microsoft.Extensions.Logging;
using SuppliesPriceLister.Models;
using ILogger = Serilog.ILogger;

namespace SuppliesPriceLister.Services
{
    public class CSVFileProcessor : IFileProcessor
    {
        private readonly ILogger<CSVFileProcessor> _logger;
        private readonly ICurrencyConverter _currencyConverter;

        public CSVFileProcessor(ILogger<CSVFileProcessor> logger, ICurrencyConverter currencyConverter)
        {
            _logger = logger;
            _currencyConverter = currencyConverter;
        }

        public string FileType => ApplicationConstants.CsvExtension;
        public IEnumerable<PrintModel> ProcessFile(string embeddedpath)
        {
            try
            {
                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedpath);
                using var reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecords<CSVModel>();

                if (records != null)
                {
                    return records.Select(x => new PrintModel
                    {
                        Identifier = x.Identifier,
                        Description = x.Description,
                        Price = _currencyConverter.ConvertAudToUsd(x.CostAud)
                    });
                }
                _logger.LogWarning("Empty CSV file Embedded");
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e,"CSVFileProcessor has thrown an error");
                return null;
            }
            
        }
    }
}