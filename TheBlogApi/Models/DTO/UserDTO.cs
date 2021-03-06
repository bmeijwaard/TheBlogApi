﻿using System;
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
        public UserDTO()
        {
            Photos = new List<PhotoDTO>();
            Blogs = new List<BlogDTO>();
        }
        public TenantBaseDTO Tenant { get; set; }

        public IList<PhotoDTO> Photos { get; set; }
        public IList<BlogDTO> Blogs { get; set; }
    }
}
