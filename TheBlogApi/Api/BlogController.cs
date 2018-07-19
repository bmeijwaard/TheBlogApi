using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Api.Base;
using TheBlogApi.Data.Services.Contracts;

namespace TheBlogApi.Api
{
    [Route("~/api/v1/blog")]
    public class BlogController : BaseApiController
    {
        private readonly IBlogService _BlogService;

        public BlogController(IBlogService BlogService)
        {
            _BlogService = BlogService;
        }
    }
}
