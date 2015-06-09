using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using RainIt.Domain.DTO;
using RainIt.Interfaces;
using RainIt.Interfaces.Repository;

namespace RainIt.Repository
{
    public class AzureCloudContext : IAzureCloudContext
    {
        public StatusMessage TryAddToCloudInContainer(MemoryStream fileStream, string fileName, string containerName, out string generatedUri)
        {
            generatedUri = String.Empty;
            try
            {
                var container = GetCloudContainerReference(containerName);
                container.CreateIfNotExists();
                container.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
                fileStream.Position = 0;
                blockBlob.UploadFromStream(fileStream);
                generatedUri = blockBlob.Uri.ToString();
                return StatusMessage.WriteMessage("The file was successfully uploaded to the cloud");
            }
            catch (Exception e)
            {
                return StatusMessage.WriteError("An error occurred while adding to the cloud");
            }
        }

        public StatusMessage TryUpdateToCloudInContainer(MemoryStream fileStream, string fileName, string containerName, out string generatedUri)
        {
            return TryAddToCloudInContainer(fileStream, fileName, containerName, out generatedUri);
        }

        public StatusMessage TryDeleteFromCloudInContainer(string fileName, string containerName)
        {
            try
            {
                var container = GetCloudContainerReference(containerName);
                var blockBlob = container.GetBlockBlobReference(fileName);
                blockBlob.Delete();
                return StatusMessage.WriteMessage("The file was successfully erased from the cloud");
            }
            catch (Exception e)
            {
                return StatusMessage.WriteError("An error occurred while erasing from the cloud");
            }
        }

        private static CloudBlobContainer GetCloudContainerReference(string containerName)
        {
            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            return container;
        }
    }
}
