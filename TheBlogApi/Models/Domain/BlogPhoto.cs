using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Models.Domain
{
    public class BlogPhoto
    {
        public Guid BlogId { get; set; }
        public Blog Blog { get; set; }
        public Guid PhotoId { get; set; }
        public Photo Photo { get; set; }
    }
}
