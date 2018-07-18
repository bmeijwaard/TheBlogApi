using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TheBlogApi.Models.Responses;

namespace TheBlogApi.Helpers.Filters
{
    public class ModelStateApiFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (!modelState.IsValid)
            {
                context.Result = new ObjectResult(new ApiResponse(modelState)) { StatusCode = 400 };
            }
        }
    }
}
