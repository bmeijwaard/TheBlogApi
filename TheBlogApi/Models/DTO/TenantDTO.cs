using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Models.DTO
{
    public class TenantBaseDTO
    {
        public Guid Id { get; set; }
        public string TenantKey { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime ExpirationDateUtc { get; set; }
    }

    public class TenantDTO : TenantBaseDTO
    {
        public UserDTO User { get; set; }
    }
}
