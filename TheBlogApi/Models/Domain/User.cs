using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Models.Domain;
using TheBlogApi.Models.Types;

namespace TheBlogApi.Models.Domain
{
    [Table("Users")]
    public class User : IdentityUser<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }

        public override string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public override string NormalizedEmail { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime RegisteredDateUTC { get; set; }

        public DateTime? AccountConfirmedDateUtc { get; set; }

        public override bool EmailConfirmed { get; set; }

        public Country Country { get; set; }

        public DateTime? BlockedUntilUtc { get; set; }

        public string AvatarUrl { get; set; }

        public string BackgroundUrl { get; set; }
        
        public override bool LockoutEnabled { get; set; }

        public override DateTimeOffset? LockoutEnd { get; set; }

        public Guid? TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
