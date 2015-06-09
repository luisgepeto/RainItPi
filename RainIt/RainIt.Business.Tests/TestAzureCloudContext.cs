using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RainIt.Domain.DTO;
using RainIt.Interfaces.Repository;

namespace RainIt.Business.Tests
{
    public class TestAzureCloudContext :IAzureCloudContext
    {
        public  StatusMessage TryAddToCloudInContainer(MemoryStream fileStream, string fileName, string containerName,
            out string generatedUri)
        {
            generatedUri =  "path for " + fileName + " in container " + containerName;
            return StatusMessage.WriteMessage("everything is ok");
        }

        public StatusMessage TryUpdateToCloudInContainer(MemoryStream fileStream, string fileName, string containerName,
            out string generatedUri)
        {
            generatedUri =  "path for " + fileName + " in container " + containerName;
            return StatusMessage.WriteMessage("everything is ok");
        }

        public StatusMessage TryDeleteFromCloudInContainer(string fileName, string containerName)
        {
            return StatusMessage.WriteMessage("everything is ok");
        }
    }
}
