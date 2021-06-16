using System.Collections.Generic;
using SuppliesPriceLister.Models;

namespace SuppliesPriceLister.Services
{
    public interface IDataPrinterService
    {
        void PrintData(IEnumerable<PrintModel> model);
    }
}