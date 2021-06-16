using System.Collections.Generic;
using System.Linq;

namespace SuppliesPriceLister.Services
{
    public class FileProcessorStrategyService : IFileProcessorStrategyService
    {
        private readonly IEnumerable<IFileProcessor> _fileProcessors;

        public FileProcessorStrategyService(IEnumerable<IFileProcessor> fileProcessors)
        {
            _fileProcessors = fileProcessors;
        }

        public IFileProcessor GetFileProcessor(string fileType)
        {
            return _fileProcessors.FirstOrDefault(x => x.FileType == fileType);
        }
    }
}