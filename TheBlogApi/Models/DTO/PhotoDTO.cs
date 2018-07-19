using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Models.DTO
{
    public class PhotoBaseDTO : BaseDTO
    {
        public string Title { get; set; }

        public Uri ImageUrl { get; set; }

        public IFormFile NewPhoto { get; set; }
    }

    public class PhotoDTO : PhotoBaseDTO
    {
        public DateTime PublicationDateUtc { get; set; }

        public string Description { get; set; }
        public string[] Tags { get; set; }

        public string Category { get; set; }
        public string SubCategory { get; set; }
    }
}
