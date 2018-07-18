using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Data.Identity;
using TheBlogApi.Models.Responses;

namespace TheBlogApi
{
    public class RolesFilter : ActionFilterAttribute
    {
        public string Roles { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var currentRole = context.HttpContext.User.Identity.Role();
            var t = Roles.Split(", ", StringSplitOptions.RemoveEmptyEntries);
            var authenticated = t.Contains(currentRole);
            if (!authenticated)
            {
                context.Result = new OkObjectResult(new ApiResponse("Insufficient rights.")) { StatusCode = 401 };
            }
        }
    }
}
