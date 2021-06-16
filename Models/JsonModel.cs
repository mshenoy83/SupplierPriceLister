using System.Collections.Generic;

namespace SuppliesPriceLister.Models
{
    public class JsonModel
    {
        public List<PartnerModel> Partners { get; set; }
    }
    
    public class SupplyModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Uom { get; set; }
        public int PriceInCents { get; set; }
        public string ProviderId { get; set; }
        public string MaterialType { get; set; }
    }

    public class PartnerModel
    {
        public string Name { get; set; }
        public string PartnerType { get; set; }
        public string PartnerAddress { get; set; }
        public List<SupplyModel> Supplies { get; set; }
    }


}