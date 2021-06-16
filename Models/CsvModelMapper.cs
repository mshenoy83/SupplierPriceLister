using CsvHelper.Configuration;

namespace SuppliesPriceLister.Models
{
    public class CsvModelMapper : ClassMap<CSVModel>
    {
        public CsvModelMapper()
        {
            Map(m => m.Identifier).Index(0).Name("identifier");
            Map(m => m.Description).Index(1).Name("desc");
            Map(m => m.Unit).Index(2).Name("unit");
            Map(m => m.CostAud).Index(3).Name("costAUD");
        }    
    }
    
}