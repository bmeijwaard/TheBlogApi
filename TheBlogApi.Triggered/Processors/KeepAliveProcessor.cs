using System;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Queue;
using TheBlogApi.Data.Stores;
using TheBlogApi.Helpers.Extensions;

namespace TheBlogApi.Triggered.Processors
{
    public class KeepAliveProcessor
    {
        public static void TriggerKeepAlive([QueueTrigger(QueueStore.KEEP_ALIVE)] CloudQueueMessage message)
        {
            try
            {
                var msg = message.GetBaseQueueMessage();
                var content = msg.DeserializeData<DateTime>();
                var text = $"Triggered WebJob keep-alive: {content.ToLocalString()}";

                Console.WriteLine(text);
            }
            catch
            {
                //ignore, the goal is only to keep the triggered job alive;
            }
        }
    }
}
