using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;

namespace TheBlogApi.Data.Providers.Contracts
{
    public interface IQueueProvider
    {
        Task AddMessageToQueueAsync(string queueName, Guid id);
        Task AddMessageToQueueAsync(string queueName, object message);
        Task<CloudQueue> GetQueueAsync(string queueName);
    }
}