using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Models.Domain
{
    [Table("Photos")]
    public class Photo : BaseEntity  
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime PublicationDateUtc { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }

        public string Category { get; set; }
        public string SubCategory { get; set; }

        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual IList<BlogPhoto> BlogPhotos { get; set; }

    }
}
