using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Data.Providers.Contracts;

namespace TheBlogApi.Data.Providers
{
    public class HttpAccessProvider : IHttpAccessProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpAccessProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string CurrentHost => $"{_httpContextAccessor.HttpContext?.Request?.Host}";
        public string CurrentProtocol => $"{_httpContextAccessor.HttpContext?.Request?.Scheme}";
        public string CurrentUrl => $"{CurrentProtocol}://{CurrentHost}";
    }
}
