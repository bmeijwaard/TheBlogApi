using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TheBlogApi.Config.Settings;
using TheBlogApi.Models.Domain;
using TheBlogApi.Data.Identity.Contracts;
using TheBlogApi.Data.Providers.Contracts;
using TheBlogApi.Models.Requests;
using TheBlogApi.Models.Responses;
using TheBlogApi.Models.Types;
using TheBlogApi.Data.Services.Contracts;
using TheBlogApi.Data.Stores;

namespace TheBlogApi.Helpers.Adapters
{
    public class AuthenticationAdapter : IAuthenticationAdapter
    {
        private readonly JWTSettings _jwtTokenSettings;
        private readonly IUserManager _userManager;
        private readonly ISignInManager _signInManager;
        private readonly IUserService _userService;
        private readonly Data.Providers.Contracts.ICryptoProvider _cryptoProvider;

        public AuthenticationAdapter(IUserManager userManager, ISignInManager signInManager, IUserService userService, IOptions<JWTSettings> settings, Data.Providers.Contracts.ICryptoProvider cryptoProvider)
        {
            _jwtTokenSettings = settings.Value;
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _cryptoProvider = cryptoProvider;
        }

        /// <summary>
        /// Private and generic tokenrequest. The function overloads has been split iot keep one generic token exchange method.
        /// </summary>
        /// <param name="request">TokenRequest</param>
        /// <param name="grantType">Specified method of login.</param>
        /// <returns>A valid json web token.</returns>
        public async Task<TokenResult> TokenExchange(TokenRequest request, GrantType grantType)
        {

            User user = null;
            switch (grantType)
            {
                case GrantType.Tenant:
                    if (string.IsNullOrWhiteSpace(request.TenantKey))
                    {
                        return new TokenResult(400, "This tenantkey is unkown.");
                    }

                    user = (await _userService.FindByTenantKeyAsync(request.TenantKey))?.Entity;
                    if (user == null)
                    {
                        return new TokenResult(400, "This tenantkey is unkown.");
                    }
                    break;

                case GrantType.Password:

                    if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                    {
                        return new TokenResult(400, "The username and/or password are required.");
                    }

                    // be sure to check that the username is the email address // or rewrite the service functionality to also find by username
                    user = await _userManager.FindByEmailAsync(_cryptoProvider.EncryptPrivate(request.Email));
                    if (user == null)
                    {
                        user = await _userManager.FindByNameAsync(request.Email);
                        if (user == null)
                        {
                            return new TokenResult(400, "The username/password couple is invalid.");
                        }
                    }

                    if (!await _userManager.CheckPasswordAsync(user, request.Password))
                    {
                        return new TokenResult(400, "The username/password couple is invalid.");
                    }

                    break;
            }

            // when no user is found, then terminate the request.
            if (user == null) return new TokenResult(400, "The specified grant type is not supported.");

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return new TokenResult(401, "Please confirm your email or reset your password.");
            }

            if (user.BlockedUntilUtc.HasValue && user.BlockedUntilUtc.Value > DateTime.UtcNow)
            {
                return new TokenResult(401, MessageHelper.BlockedUntilMessage(user.BlockedUntilUtc));
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return new TokenResult(401, "The account is locked out.");
            }

            if (!await _signInManager.CanSignInAsync(user))
            {
                return new TokenResult(401, "The specified user is not allowed to sign in.");
            }

            // reset locks
            await _userManager.ResetAccessFailedCountAsync(user);
            await _userManager.SetLockoutEnabledAsync(user, false);

            // Create a new authentication token.
            var token = await GetJwtSecurityToken(user, grantType);
            return new TokenResult(200)
            {
                TokenData = new TokenData
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    ExpiredDateUtc = token.ValidTo
                }
            };
        }

        /// <summary>
        /// Generates the json web token.
        /// </summary>
        /// <param name="user">UserDTO</param>
        /// <returns>A JwtSecurityToken object.</returns>
        private async Task<JwtSecurityToken> GetJwtSecurityToken(User user, GrantType grantType)
        {
            var expiresAfterHours = _jwtTokenSettings.ExpiresAfterHours;
            IEnumerable<Claim> userClaims = new List<Claim>();
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            switch (grantType)
            {
                case GrantType.Password:
                    userClaims = principal.Claims.Union(GetUserClaims(user)).Distinct();
                    break;
                case GrantType.Tenant:
                    userClaims = principal.Claims.Union(GetTenantClaims(user)).Distinct();
                    //expiresAfterHours = 24;
                    break;
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenSettings.SecurityKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwtTokenSettings.SiteAddress,
                audience: _jwtTokenSettings.Audience,
                claims: userClaims,
                expires: DateTime.UtcNow.AddHours(expiresAfterHours),
                signingCredentials: credentials
            );

            return token;
        }

        private static IEnumerable<Claim> GetTenantClaims(User user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Role, RolesStore.TENANT)
            };
        }

        private static IEnumerable<Claim> GetUserClaims(User user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Role, RolesStore.USER),          
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
            };
        }
    }
}
