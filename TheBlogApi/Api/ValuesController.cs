using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheBlogApi.Api.Base;
using TheBlogApi.Data.Handlers;
using TheBlogApi.Data.Messages;
using TheBlogApi.Data.Services.Contracts;
using TheBlogApi.Data.Stores;
using TheBlogApi.Helpers.Extensions;
using TheBlogApi.Helpers.Filters;
using TheBlogApi.Models.DTO;
using TheBlogApi.Models.Responses;

namespace TheBlogApi.Controllers
{
    [Route("~/api/v1/values")]
    public class ValuesController : BaseApiController
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly IEmailService _emailService;
        private readonly IBlogService _blogService;

        public ValuesController(IExceptionHandler exceptionHandler, IEmailService emailService, IBlogService blogService)
        {
            _exceptionHandler = exceptionHandler;
            _emailService = emailService;
            _blogService = blogService;
        }

        [HttpGet("throw")]
        [AllowAnonymous]
        public async Task<IActionResult> Throw()
        {
            try
            {
                throw new Exception("This is a test.");
            }
            catch (Exception e)
            {
                var message = $"Something went wrong: {e.Message}";
                await _exceptionHandler.ProcessExceptionAsync(message, e, "Api controller", e);
                throw;
            }
        }

        [HttpGet("email")]
        [AllowAnonymous]
        public async Task<IActionResult> Email()
        {
            try
            {
                await _emailService.ForgotPasswordNotificationAsync("bmeijwaard@gmail.com", Guid.NewGuid().Flatten());
                return Ok(new ApiResponse());
            }
            catch (Exception e)
            {
                var message = $"Something went wrong: {e.Message}";
                await _exceptionHandler.ProcessExceptionAsync(message, e, "Api controller", e);
                return StatusCode(500, new ApiResponse(e.Message));
            }
        }

        // GET api/values
        [HttpGet("")]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var id = Guid.NewGuid();
            var photo = new PhotoBaseDTO
            {
                Id = Guid.NewGuid(),
                Title = "test"
            };
            var blog = new BlogDTO
            {
                Id = id,
                Title = "test",
                Tags = new string[] { "Some", "Test" }
            };
            blog.Photos.Add(photo);
            var result = await _blogService.CreateAsync(blog);            
            result = await _blogService.ReadAsync(id);
            return Ok(new ApiResponse<BlogDTO>(result.Entity));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _blogService.ReadAsync(id);
            return Ok(new ApiResponse<BlogDTO>(result.Entity));
        }

        // POST api/values
        [HttpPost("")]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
