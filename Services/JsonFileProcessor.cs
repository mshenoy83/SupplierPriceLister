using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SuppliesPriceLister.Models;

namespace SuppliesPriceLister.Services
{
    public class JsonFileProcessor : IFileProcessor
    {
        public string FileType => ApplicationConstants.JsonExtension;
        public async Task ProcessFile(string embeddedpath)
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedpath);
            using StreamReader reader = new StreamReader(stream);
            JsonSerializer serializer = new JsonSerializer();
            var jsonModel = (JsonModel)serializer.Deserialize(reader, typeof(JsonModel));
            foreach (var model in jsonModel.Partners)
            {
                
            }
        }
    }
}