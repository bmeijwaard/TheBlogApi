using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Blob;

namespace TheBlogApi.Data.Providers.Contracts
{
    public interface IStorageProvider
    {
        Task DeleteAsync(CloudBlockBlob blob);
        Task DeleteAsync(string containerName, string fileName);
        CloudBlobContainer GetContainer(string containerName);
        Task<CloudBlockBlob> GetFileAsync(string containerName, string fileName);
        Task<IEnumerable<CloudBlockBlob>> GetFilesByPrefixAsync(string containerName, string prefix);
        Task<CloudBlockBlob> GetSingleFileByPrefixAsync(string containerName, string prefix);
        Task<Uri> SaveFileAndGetUriAsync(string containerName, string fileName, IFormFile file);
        Task SaveFileAsync(string containerName, string fileName, IFormFile file);
        Task SaveFileAsync(string containerName, string fileName, string contentType, byte[] data);
        Task SaveFileAsync(string containerName, string fileName, string contentType, Stream stream);
    }
}