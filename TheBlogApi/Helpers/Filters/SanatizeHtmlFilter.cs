using Microsoft.AspNetCore.Mvc.Filters;
using TheBlogApi.Helpers.Extensions;

namespace TheBlogApi.Helpers.Filters
{
    public class SanatizeHtmlFilter : ActionFilterAttribute
    {
        public string PropertyName { get; set; }
        public string ModelName { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var formData = context.ActionArguments[ModelName];
            var dirtyHtml = (string)formData.GetPropertyValue(PropertyName);
            var cleanHtml = dirtyHtml.SanatizeHtml();
            formData.SetPropertyValue<string>(PropertyName, cleanHtml);
        }
    }
}
