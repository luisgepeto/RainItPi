using System.IO;
using RainIt.Domain.DTO;

namespace RainIt.Interfaces.Repository
{
    public interface IAzureCloudContext
    {
        StatusMessage TryAddToCloudInContainer(MemoryStream fileStream, string fileName, string containerName, out string generatedUri);
        StatusMessage TryUpdateToCloudInContainer(MemoryStream fileStream, string fileName, string containerName, out string generatedUri);
        StatusMessage TryDeleteFromCloudInContainer(string fileName, string username);
    }
}
