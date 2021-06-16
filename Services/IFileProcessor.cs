using System.Collections.Generic;
using SuppliesPriceLister.Models;

namespace SuppliesPriceLister.Services
{
    public interface IFileProcessor
    {
        string FileType { get; }
        List<PrintModel> ProcessFile(string embeddedpath);
    }
}