using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using TheBlogApi.Api.Base;
using TheBlogApi.Models.Domain;
using TheBlogApi.Models.DTO;
using TheBlogApi.Data.Identity;
using TheBlogApi.Data.Identity.Contracts;
using TheBlogApi.Data.Messages;
using TheBlogApi.Data.Services;
using TheBlogApi.Data.Services.Contracts;
using TheBlogApi.Helpers;
using TheBlogApi.Models.Requests;
using TheBlogApi.Models.Responses;

namespace TheBlogApi.Api
{
    [Route("~/api/v1/user")]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        [TenantBlock]
        [Produces(typeof(ApiResponse<UserDTO>))]
        public async Task<IActionResult> Get(Guid id) =>
            await ExcecuteAsync(async () => await _userService.FindByIdAsync(id));

        [HttpGet("current")]
        [TenantBlock]
        [Produces(typeof(ApiResponse<UserDTO>))]
        public async Task<IActionResult> GetCurrent() =>
            await ExcecuteAsync(async () => await _userService.FindByIdAsync(User.Identity.Id()));

        [AllowAnonymous]
        [HttpPost("register")]
        [Produces(typeof(ApiResponse))]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request) =>
            await ExcecuteAsync(async () => await _userService.CreateAsync(Mapper.Map<UserDTO>(request), request.Password));

        [AllowAnonymous]
        [HttpGet("forgotpassword/{email}")]
        [Produces(typeof(ApiResponse))]
        public async Task<IActionResult> Forgotpassword(string email) =>
            await ExcecuteAsync(async () => await _userService.ForgotPasswordAsync(email));


        [AllowAnonymous]
        [HttpPost("resetpassword")]
        [Produces(typeof(ApiResponse))]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request) =>
            await ExcecuteAsync(async () => await _userService.ResetPasswordAsync(request.Email, request.Token, request.Password));

        [AllowAnonymous]
        [HttpPost("activate")]
        [Produces(typeof(ApiResponse))]
        public async Task<IActionResult> ActivateAccount([FromBody] ActivateAccountRequest request) =>
            await ExcecuteAsync(async () => await _userService.ActivateAccountAsync(request.Email, request.Token));

        [HttpGet("tenantkey")]
        [TenantBlock]
        [Produces(typeof(ApiResponse<string>))]
        public async Task<IActionResult> TenantKey() =>
            await ExcecuteAsync(async () => await _userService.GetTenantKeyByUserIdAsync(User.Identity.Id()));
    }
}
