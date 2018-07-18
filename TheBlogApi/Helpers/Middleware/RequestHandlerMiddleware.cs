using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using TheBlogApi.Models.Responses;

namespace TheBlogApi.Helpers.Middleware
{
    public class RequestHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static bool ContainsPath(string path, string subString) => path.Contains(subString);

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            //if (exception != null) code = HttpStatusCode.BadRequest;

            var result = JsonConvert.SerializeObject(new ApiResponse<Exception>(exception));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }

    public static class RequestHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseRequestHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestHandlerMiddleware>();
        }
    }
}
