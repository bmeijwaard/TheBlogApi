using Microsoft.AspNetCore.Mvc.Filters;
using TheBlogApi.Helpers.Extensions;

namespace TheBlogApi.Helpers.Filters
{
    public class SanatizeHtmlFilter : ActionFilterAttribute
    {
        public string Property { get; set; }
        public string Model { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var formData = context.ActionArguments[Model];
            var dirtyHtml = (string)formData.GetPropertyValue(Property);
            var cleanHtml = dirtyHtml.SanatizeHtml();
            formData.SetPropertyValue<string>(Property, cleanHtml);
        }
    }
}
