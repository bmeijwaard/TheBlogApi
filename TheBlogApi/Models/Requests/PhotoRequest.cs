using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Models.Requests
{
    public class PhotoRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; }    

        public string Description { get; set; }
        public string[] Tags { get; set; }

        public string Category { get; set; }
        public string SubCategory { get; set; }
    }
}
