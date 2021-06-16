using System;
using System.Globalization;
using Microsoft.Extensions.Options;
using SuppliesPriceLister.Models;

namespace SuppliesPriceLister.Services
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly ApplicationSettings _appSettings;

        public CurrencyConverter(IOptions<ApplicationSettings> appSettings)
        {
            _appSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public decimal ConvertAudToUsd(int price)
        {
            return Math.Round((price / 100) / _appSettings.audUsdExchangeRate, 2);
        }

        public decimal ConvertAudToUsd(decimal price)
        {
            return Math.Round(price / _appSettings.audUsdExchangeRate, 2);
        }
    }
}