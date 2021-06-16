namespace SuppliesPriceLister.Services
{
    public interface IFileProcessorStrategyService
    {
        IFileProcessor GetFileProcessor(string fileType);
    }
}