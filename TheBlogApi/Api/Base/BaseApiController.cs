using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TheBlogApi.Data.Handlers;
using TheBlogApi.Helpers.Filters;
using TheBlogApi.Models.Responses;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TheBlogApi.Data.Messages;
using TheBlogApi.Data.Stores;

namespace TheBlogApi.Api.Base
{
    [Authorize(Policy = "DefaultPolicy")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Route("~/api/v1/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ModelStateApiFilter]
    public class BaseApiController : ControllerBase
    {
        // Conventions
        // GET /tickets - Retrieves a list of tickets
        // GET /tickets/12 - Retrieves a specific ticket
        // POST /tickets - Creates a new ticket
        // PUT /tickets/12 - Updates ticket #12
        // PATCH /tickets/12 - Partially updates ticket #12
        // DELETE /tickets/12 - Deletes ticket #12

        // GET /tickets/12/messages - Retrieves list of messages for ticket #12
        // GET /tickets/12/messages/5 - Retrieves message #5 for ticket #12
        // POST /tickets/12/messages - Creates a new message in ticket #12
        // PUT /tickets/12/messages/5 - Updates message #5 for ticket #12
        // PATCH /tickets/12/messages/5 - Partially updates message #5 for ticket #12
        // DELETE /tickets/12/messages/5 - Deletes message #5 for ticket #12

        private readonly IExceptionHandler _exceptionHandler;

        public BaseApiController()
        {
            _exceptionHandler = Startup.ServiceProvider.GetService<IExceptionHandler>();
        }

        protected async Task<IActionResult> ExcecuteAsync(Func<Task<ServiceResponse>> func, [CallerMemberName] string memberName = "")
        {
            try
            {
                var result = await func();
                if (!result.Succeeded)
                {
                    return StatusCode(400, new ApiResponse(result.ErrorMessage));
                }
                return Ok(new ApiResponse());
            }
            catch (Exception e)
            {
                var message = $"Something went wrong: {e.Message}";
                await _exceptionHandler.ProcessExceptionAsync(message, e, memberName);
                return StatusCode(500, new ApiResponse(e.Message));
            }
        }

        protected async Task<IActionResult> ExcecuteAsync<T>(Func<Task<EntityResponse<T>>> func, [CallerMemberName] string memberName = "")
        {
            try
            {
                var result = await func();
                if (!result.Succeeded)
                {
                    return StatusCode(400, new ApiResponse(result.ErrorMessage));
                }
                return Ok(new ApiResponse<T>(result.Entity));
            }
            catch (Exception e)
            {
                var message = $"Something went wrong: {e.Message}";
                await _exceptionHandler.ProcessExceptionAsync(message, e, memberName);
                return StatusCode(500, new ApiResponse(e.Message));
            }
        }

        protected async Task<IActionResult> ExcecuteAsync<T>(Func<Task<EntitiesResponse<T>>> func, [CallerMemberName] string memberName = "")
        {
            try
            {
                var result = await func();
                if (!result.Succeeded)
                {
                    return StatusCode(400, new ApiResponse(result.ErrorMessage));
                }
                return Ok(new ApiResponse<T>(result.Entities));
            }
            catch (Exception e)
            {
                var message = $"Something went wrong: {e.Message}";
                await _exceptionHandler.ProcessExceptionAsync(message, e, memberName);
                return StatusCode(500, new ApiResponse(e.Message));
            }
        }

        protected async Task<IActionResult> ExcecuteAsync<T>(Func<Task<T>> func, [CallerMemberName] string memberName = "")
        {
            try
            {
                var result = await func();
                return Ok(new ApiResponse<T>(result));
            }
            catch (Exception e)
            {
                var message = $"Something went wrong: {e.Message}";
                await _exceptionHandler.ProcessExceptionAsync(message, e, memberName);
                return StatusCode(500, new ApiResponse(e.Message));
            }
        }

        protected async Task<IActionResult> ExcecuteAsync(Func<Task<TokenResult>> func, [CallerMemberName] string memberName = "")
        {
            try
            {
                var result = await func();
                if (result.StatusCode != 200)
                {
                    return StatusCode(result.StatusCode, new ApiResponse(result.StatusDescription));
                }
                return Ok(new ApiResponse<TokenData>(result.TokenData));
            }
            catch (Exception e)
            {
                var message = $"Something went wrong: {e.Message}";
                await _exceptionHandler.ProcessExceptionAsync(message, e, memberName);
                return StatusCode(500, new ApiResponse(e.Message));
            }
        }
    }
}
