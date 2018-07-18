using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Models.Types;

namespace TheBlogApi.Models.DTO
{
    public class UserBaseDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Country Country { get; set; }
        public DateTime? AccountConfirmedDateUtc { get; set; }
        public bool EmailConfirmed { get; set; }
    }

    public class UserDTO : UserBaseDTO
    {
        public TenantBaseDTO Tenant { get; set; }

        public IList<PhotoDTO> Photos { get; } = new List<PhotoDTO>();
        public IList<BlogDTO> Blogs { get; } = new List<BlogDTO>();
    }
}
