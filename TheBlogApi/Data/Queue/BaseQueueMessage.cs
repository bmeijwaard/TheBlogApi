using Newtonsoft.Json;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Data.Queue
{
    [ProtoContract]
    public class BaseQueueMessage
    {
        // constructors
        public BaseQueueMessage() { }
        public BaseQueueMessage(object data)
        {
            TriggerDelay = 2;
            RetryNext = true;
            Data = JsonConvert.SerializeObject(data, JsonSettings);
        }
        public BaseQueueMessage(Guid id)
        {
            TriggerDelay = 2;
            RetryNext = true;
            Data = JsonConvert.SerializeObject(new BaseQueueObject(id), JsonSettings);
        }
        public BaseQueueMessage(BaseQueueMessage previousMsg)
        {
            TriggerDelay = previousMsg.TriggerDelay * 2;
            RetryNext = TriggerDelay < 64;
            Data = previousMsg.Data;
        }

        // properties
        [ProtoMember(1)]
        public int TriggerDelay { get; set; }

        [ProtoMember(2)]
        public bool RetryNext { get; set; }

        [ProtoMember(3)]
        public string Data { get; set; }


        // public methods
        public T DeserializeData<T>() => JsonConvert.DeserializeObject<T>(Data);

        public BaseQueueMessage NextMessage => new BaseQueueMessage(this);

        public byte[] AsBytes()
        {
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, this);
                return ms.ToArray();
            }
        }

        // private methods
        private JsonSerializerSettings JsonSettings => new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Objects
        };
    }
}
