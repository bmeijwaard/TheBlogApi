using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Models.Requests
{
    public class BlogRequest
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required]
        public string[] Tags { get; set; }

        [Required]
        public string Category { get; set; }
        public string SubCategory { get; set; }

        public string Text { get; set; }

        public IFormFile ImageFile { get; set; }
        public Guid[] Photos { get; set; }
    }
}
