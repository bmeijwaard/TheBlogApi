using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Models.DTO
{
    public abstract class BaseDTO
    {
        public Guid Id { get; set; }
    }
}
