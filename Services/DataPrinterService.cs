using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SuppliesPriceLister.Models;

namespace SuppliesPriceLister.Services
{
    public class DataPrinterService : IDataPrinterService
    {
        public void PrintData(IEnumerable<PrintModel> model)
        {
            foreach (var prntModel in model.OrderByDescending(x => x.Price))
            {
                Console.WriteLine("{0},{1},{2}", prntModel.Identifier, prntModel.Description,
                    string.Format(new CultureInfo("en-US", false), "{0:C}", prntModel.Price));
            }
        }
    }
}