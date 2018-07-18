using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Data.Queue
{
    public class BaseQueueObject
    {
        public BaseQueueObject() { }

        public BaseQueueObject(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
