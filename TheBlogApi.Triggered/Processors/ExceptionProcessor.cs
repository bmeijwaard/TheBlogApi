using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using ProtoBuf;
using TheBlogApi.Data.Handlers;
using TheBlogApi.Data.Queue;
using TheBlogApi.Data.Stores;
using TheBlogApi.Helpers.Extensions;

namespace TheBlogApi.Triggered.Processors
{
    public class ExceptionProcessor
    {
        private static IExceptionHandler _exceptionHandler;
        static ExceptionProcessor()
        {
            _exceptionHandler = Program.ServiceProvider.GetService<IExceptionHandler>();
        }

        public static async Task ProcessExceptionAsync([QueueTrigger(QueueStore.PROCESS_EXCEPTION)] CloudQueueMessage message)
        {
            Console.WriteLine($"Started {QueueStore.PROCESS_EXCEPTION}: {DateTime.UtcNow.ToLocalString()}");
            try
            {
                var msg = message.GetBaseQueueMessage();
                var exception = msg.DeserializeData<ExceptionQueueMessage<Exception>>();
                await _exceptionHandler.NotifyExceptionAsync(exception.Exception, exception.Message, exception.Source, exception.Data);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception {QueueStore.PROCESS_EXCEPTION}: {DateTime.UtcNow.ToLocalString()}, {e.Message}");
                throw;
            }
            finally
            {
                Console.WriteLine($"Finished {QueueStore.PROCESS_EXCEPTION}: {DateTime.UtcNow.ToLocalString()}");
            }
        }
    }
}
