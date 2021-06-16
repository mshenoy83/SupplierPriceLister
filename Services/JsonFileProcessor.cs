using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SuppliesPriceLister.Models;

namespace SuppliesPriceLister.Services
{
    public class JsonFileProcessor : IFileProcessor
    {
        private readonly ICurrencyConverter _currencyConverter;
        private readonly ILogger<JsonFileProcessor> _logger;

        public JsonFileProcessor(ILogger<JsonFileProcessor> logger, ICurrencyConverter currencyConverter)
        {
            _logger = logger;
            _currencyConverter = currencyConverter;
        }


        public string FileType => ApplicationConstants.JsonExtension;

        public List<PrintModel> ProcessFile(string embeddedpath)
        {
            try
            {
                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedpath);
                using var reader = new StreamReader(stream);
                var serializer = new JsonSerializer();
                var jsonModel = (JsonModel) serializer.Deserialize(reader, typeof(JsonModel));

                if (jsonModel != null && jsonModel.Partners.Any())
                {
                    return jsonModel.Partners.SelectMany(x => x.Supplies).Select(x => new PrintModel
                    {
                        Identifier = x.Id.ToString(),
                        Description = x.Description,
                        Price = _currencyConverter.ConvertAudToUsd(x.PriceInCents)
                    }).ToList();
                }

                _logger.LogWarning("Empty Json file embedded");
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e,"JsonFileProcessor has thrown an error");
                return null;
            }
        }
    }
}