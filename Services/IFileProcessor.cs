using System.Threading.Tasks;

namespace SuppliesPriceLister.Services
{
    public interface IFileProcessor
    {
        string FileType { get; }
        Task ProcessFile(string path);
    }
}