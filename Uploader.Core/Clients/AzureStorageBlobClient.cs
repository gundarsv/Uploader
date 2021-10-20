using Azure.Storage.Blobs;
using System;
using Uploader.Core.Configurations;

namespace Uploader.Core.Clients
{
    public class AzureStorageBlobClient
    {
        public BlobContainerClient Client;

        public AzureStorageBlobClient(IStorageAccountConfiguration storageAccountConfiguration)
        {
            if (storageAccountConfiguration is null)
            {
                throw new ArgumentNullException(nameof(storageAccountConfiguration));
            }

            Client = new BlobContainerClient(storageAccountConfiguration.StorageAccountConnection, storageAccountConfiguration.ContainerName);
        }
    }
}
