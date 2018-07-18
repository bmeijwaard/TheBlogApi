using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using TheBlogApi.Data.Handlers;
using TheBlogApi.Data.Providers.Contracts;
using TheBlogApi.Data.Queue;
using TheBlogApi.Data.Stores;
using TheBlogApi.Helpers.Extensions;

namespace TheBlogApi.Triggered.Processors
{
    public class EmailProcessor
    {
        private static IEmailProvider _emailProvider;
        private static IExceptionHandler _exceptionHandler;
        static EmailProcessor()
        {
            _emailProvider = Program.ServiceProvider.GetService<IEmailProvider>();
            _exceptionHandler = Program.ServiceProvider.GetService<IExceptionHandler>();
        }
        public static async Task ProcessMailAsync([QueueTrigger(QueueStore.NEW_MAIL)] CloudQueueMessage message, [Queue(QueueStore.NEW_MAIL)] CloudQueue nextQueue)
        {
            Console.WriteLine($"Started {QueueStore.NEW_MAIL}: {DateTime.UtcNow.ToLocalString()}");
            var msg = message.GetBaseQueueMessage();
            try
            {
                await _emailProvider.SendEmailAsync(msg.DeserializeData<MailQueueMessage>());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception {QueueStore.NEW_MAIL}: {DateTime.UtcNow.ToLocalString()}, Retry {msg.RetryNext}: {e.Message}");
                if (!msg.RetryNext)
                {
                    // if a message is catched here then we inform the admin.
                    await _exceptionHandler.ProcessExceptionAsync(QueueStore.NEW_MAIL, e);
                    throw;
                }

                await nextQueue.AddMessageAsync(
                        CloudQueueMessage.CreateCloudQueueMessageFromByteArray(
                                msg.NextMessage.AsBytes()
                        ),
                        null, 
                        TimeSpan.FromSeconds(msg.NextMessage.TriggerDelay), 
                        new QueueRequestOptions(), 
                        new OperationContext()
                    );
            }
            finally
            {
                Console.WriteLine($"Finished {QueueStore.NEW_MAIL}: {DateTime.UtcNow.ToLocalString()}");
            }
        }
    }
}
