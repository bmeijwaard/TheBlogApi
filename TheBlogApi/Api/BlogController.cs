using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Api.Base;
using TheBlogApi.Data.Services.Contracts;
using TheBlogApi.Helpers.Attributes;
using TheBlogApi.Helpers.Filters;
using TheBlogApi.Models.DTO;
using TheBlogApi.Models.Requests;
using TheBlogApi.Models.Responses;

namespace TheBlogApi.Api
{
    [Route("~/api/v1/blog")]
    public class BlogController : BaseApiController
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpPost("")]
        [TenantBlock]
        [SanatizeHtmlFilter(Model = "request", Property = "Text")]
        [Produces(typeof(ApiResponse<BlogResponse>))]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post([FromForm] BlogRequest request) =>
            await ExcecuteAsync(async () => await _blogService.CreateAsync(Mapper.Map<BlogDTO>(request)));
        
    }
}
