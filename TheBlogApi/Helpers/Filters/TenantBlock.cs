using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Data.Identity;
using TheBlogApi.Data.Stores;
using TheBlogApi.Models.Responses;

namespace TheBlogApi
{
    public class TenantBlock : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var currentRole = context.HttpContext.User.Identity.Role();
            if (currentRole == RolesStore.TENANT)
            {
                context.Result = new OkObjectResult(new ApiResponse("Insufficient rights.")) { StatusCode = 401 };
            }
        }
    }
}
