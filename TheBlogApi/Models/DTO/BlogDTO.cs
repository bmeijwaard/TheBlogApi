using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Models.DTO
{
    public class BlogBaseDTO
    {
        public Guid Id { get; set; }
        public DateTime PublicationDateUtc { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string[] Tags { get; set; }

        public string Category { get; set; }
        public string SubCategory { get; set; }

        public string Text { get; set; }
    }

    public class BlogDTO : BlogBaseDTO
    {
        public IList<PhotoBaseDTO> Photos { get; } = new List<PhotoBaseDTO>();
    }
}
