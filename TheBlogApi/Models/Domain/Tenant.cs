using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Models.Domain;

namespace TheBlogApi.Models.Domain
{
    [Table("Tenants")]
    public class Tenant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string TenantKey { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime ExpirationDateUtc { get; set; }

        public Guid UserId { get; set; }
    }
}
