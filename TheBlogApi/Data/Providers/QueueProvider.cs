using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Config.Settings;
using TheBlogApi.Data.Providers.Contracts;
using TheBlogApi.Data.Queue;

namespace TheBlogApi.Data.Providers
{
    public class QueueProvider : IQueueProvider
    {
        private readonly StorageSettings _storageSettings;
        private CloudStorageAccount _storageAccount;
        private CloudQueueClient _cloudQueueClient;

        public QueueProvider(IOptions<StorageSettings> storageSettings)
        {
            _storageSettings = storageSettings.Value;
            Initialize();
        }

        private bool Initialize()
        {
            var result = true;
            try
            {
                _storageAccount = CloudStorageAccount.Parse(_storageSettings.ConnectionString);
                _cloudQueueClient = _storageAccount.CreateCloudQueueClient();
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public async Task<CloudQueue> GetQueueAsync(string queueName)
        {
            if (!Initialize()) return null;

            var queue = _cloudQueueClient.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();

            return queue;
        }

        public async Task AddMessageToQueueAsync(string queueName, object message)
        {
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, new BaseQueueMessage(message));
                await AddMessageToQueueAsync(queueName, CloudQueueMessage.CreateCloudQueueMessageFromByteArray(ms.ToArray()));
            }
        }

        public async Task AddMessageToQueueAsync(string queueName, Guid id)
        {
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, new BaseQueueMessage(id));
                await AddMessageToQueueAsync(queueName, CloudQueueMessage.CreateCloudQueueMessageFromByteArray(ms.ToArray()));
            }
        }

        private async Task AddMessageToQueueAsync(string queueName, CloudQueueMessage message)
        {
            var queue = await GetQueueAsync(queueName);
            await AddMessageToQueueAsync(queue, message);
        }

        private async Task AddMessageToQueueAsync(CloudQueue queue, CloudQueueMessage message)
        {
            await queue.AddMessageAsync(message);
        }
    }
}
