using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage.Queue;
using ProtoBuf;
using TheBlogApi.Data.Handlers;
using TheBlogApi.Data.Queue;
using TheBlogApi.Data.Stores;
using TheBlogApi.Helpers.Extensions;

namespace TheBlogApi.Data.Stores
{
    public static class QueueStore
    {
        public const string PROCESS_EXCEPTION = "process-exception";
        public const string NEW_MAIL = "new-mail";
        public const string KEEP_ALIVE = "keep-alive";
    }
}
