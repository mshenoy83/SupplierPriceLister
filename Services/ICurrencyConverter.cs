namespace SuppliesPriceLister.Services
{
    public interface ICurrencyConverter
    {
        decimal ConvertAudToUsd(int audprice);
        decimal ConvertAudToUsd(decimal price);
    }
}