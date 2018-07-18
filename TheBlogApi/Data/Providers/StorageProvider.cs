using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Config.Settings;
using TheBlogApi.Data.Providers.Contracts;
using TheBlogApi.Data.Stores;

namespace TheBlogApi.Data.Providers
{
    public class StorageProvider : IStorageProvider
    {
        private readonly StorageSettings _storageSettings;
        private CloudStorageAccount _storageAccount;
        private CloudBlobClient _cloudBlobClient;

        public StorageProvider(IOptions<StorageSettings> storageSettings)
        {
            _storageSettings = storageSettings.Value;
            Initialize(true);
        }

        public CloudBlobContainer GetContainer(string containerName)
        {
            if (_cloudBlobClient != null && !string.IsNullOrEmpty(containerName))
            {
                return _cloudBlobClient.GetContainerReference(containerName);
            }
            return null;
        }

        public async Task SaveFileAsync(string containerName, string fileName, string contentType, byte[] data)
        {
            var container = GetContainer(containerName);
            var blob = container.GetBlockBlobReference(fileName);
            blob.Properties.ContentType = contentType;
            await blob.UploadFromByteArrayAsync(data, 0, data.Length);
        }

        public async Task SaveFileAsync(string containerName, string fileName, string contentType, Stream stream)
        {
            var container = GetContainer(containerName);
            var blob = container.GetBlockBlobReference(fileName);
            blob.Properties.ContentType = contentType;
            await blob.UploadFromStreamAsync(stream);
        }

        public async Task SaveFileAsync(string containerName, string fileName, IFormFile file)
        {
            if (file.Length <= 0) return;

            var container = GetContainer(containerName);
            var blob = container.GetBlockBlobReference(fileName);
            blob.Properties.ContentType = file.ContentType;

            using (var stream = file.OpenReadStream())
            {
                await blob.UploadFromStreamAsync(stream);
            }
        }

        public async Task<Uri> SaveFileAndGetUriAsync(string containerName, string fileName, IFormFile file)
        {
            if (file.Length <= 0) return null;

            var container = GetContainer(containerName);
            var blob = container.GetBlockBlobReference(fileName);
            blob.Properties.ContentType = file.ContentType;

            using (var stream = file.OpenReadStream())
            {
                await blob.UploadFromStreamAsync(stream);
            }

            return blob.Uri;
        }

        public async Task<CloudBlockBlob> GetSingleFileByPrefixAsync(string containerName, string prefix)
        {
            CloudBlockBlob blob = null;
            var files = await GetFilesByPrefixAsync(containerName, prefix);

            if (files != null && files.Count() == 1)
            {
                blob = files.First();
            }

            return blob;
        }

        public async Task<IEnumerable<CloudBlockBlob>> GetFilesByPrefixAsync(string containerName, string prefix)
        {

            var container = GetContainer(containerName);
            var files = await container.ListBlobsSegmentedAsync(prefix, null);

            return files.Results.Select(x => (CloudBlockBlob)x);
        }

        public Task<CloudBlockBlob> GetFileAsync(string containerName, string fileName)
        {
            var container = GetContainer(containerName);
            var blob = container.GetBlockBlobReference(fileName);

            return Task.FromResult(blob);
        }

        public async Task DeleteAsync(string containerName, string fileName)
        {
            var container = GetContainer(containerName);
            var blob = container.GetBlockBlobReference(fileName);

            if (blob != null)
            {
                await DeleteAsync(blob);
            }
        }

        public async Task DeleteAsync(CloudBlockBlob blob)
        {
            await blob.DeleteIfExistsAsync();
        }

        #region Private

        private void Initialize(bool ensureContainersExists)
        {
            _storageAccount = CloudStorageAccount.Parse(_storageSettings.ConnectionString);
            _cloudBlobClient = _storageAccount.CreateCloudBlobClient();
            if (ensureContainersExists)
            {
                EnsureContainerExists(ContainerStore.PHOTO_CONTAINER, BlobContainerPublicAccessType.Container);
            }
        }

        private async void EnsureContainerExists(string containerName, BlobContainerPublicAccessType blobAccess)
        {
            CloudBlobContainer container = GetContainer(containerName);

            if (container == null)
                throw new ArgumentException("Invalid container name: " + containerName);

            await container.CreateIfNotExistsAsync(blobAccess, null, null);
        }

        #endregion
    }
}
