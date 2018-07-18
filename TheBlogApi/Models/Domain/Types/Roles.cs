using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Models.Types
{
    public enum Roles
    {
        [Display(Name = "Administrator")]
        Administrator = 100
    }
}
