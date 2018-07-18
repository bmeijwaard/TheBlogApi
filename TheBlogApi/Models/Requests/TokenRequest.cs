using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Models.Requests
{
    public class PasswordRequest
    {
        /// <summary>
        /// Email or Username
        /// </summary>
        [Required]        
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class TenantRequest
    {
        [Required]
        public string TenantKey { get; set; }
    }

    public class TokenRequest
    {
        public string TenantKey { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
