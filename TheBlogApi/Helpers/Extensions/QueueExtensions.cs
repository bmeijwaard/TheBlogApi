using Microsoft.WindowsAzure.Storage.Queue;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Data.Queue;

namespace TheBlogApi.Helpers.Extensions
{
    public static class QueueExtensions
    {
        public static BaseQueueMessage GetBaseQueueMessage(this CloudQueueMessage message)
        {
            using (var memoryStream = new MemoryStream(message.AsBytes))
            {
                return Serializer.Deserialize<BaseQueueMessage>(memoryStream);
            }
        }
    }
}
