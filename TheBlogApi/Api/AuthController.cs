using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheBlogApi.Api.Base;
using TheBlogApi.Helpers.Adapters;
using TheBlogApi.Models.Requests;
using TheBlogApi.Models.Responses;
using TheBlogApi.Models.Types;

namespace TheBlogApi.Api
{
    [Route("~/api/v1/auth")]
    public class AuthController : BaseApiController
    {
        private readonly IAuthenticationAdapter _authenticationAdapter;

        public AuthController(IAuthenticationAdapter authenticationAdapter)
        {
            _authenticationAdapter = authenticationAdapter;
        }

        /// <summary>
        /// Authentication: Login with a username (email) and password. 
        /// </summary>
        /// <param name="request">PasswordRequest</param>
        /// <returns>A valid json web token if authenticated.</returns>
        [AllowAnonymous]
        [HttpPost("password")]
        [Produces(typeof(ApiResponse<TokenData>))]
        public async Task<IActionResult> PasswordLogin([FromBody] PasswordRequest request)
            => await ExcecuteAsync(async () => await _authenticationAdapter.TokenExchange(Mapper.Map<TokenRequest>(request), GrantType.Password));

        /// <summary>
        /// Authentication: Login with a tenantkey. This key will grant access to the user content designated to user linked content.
        /// </summary>
        /// <param name="request">TenantRequest</param>
        /// <returns>A valid json web token if authenticated.</returns>
        [AllowAnonymous]
        [HttpPost("tenant")]
        [Produces(typeof(ApiResponse<TokenData>))]
        public async Task<IActionResult> TenantLogin([FromBody] TenantRequest request)
            => await ExcecuteAsync(async () => await _authenticationAdapter.TokenExchange(Mapper.Map<TokenRequest>(request), GrantType.Tenant));
    }
}
