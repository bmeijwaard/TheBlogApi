using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Data.Context.Providers.Contracts;
using TheBlogApi.Models.Domain;
using TheBlogApi.Data.Identity.Contracts;
using TheBlogApi.Data.Messages;
using TheBlogApi.Data.Services.Contracts;
using TheBlogApi.Models.DTO;
using TheBlogApi.Data.Providers.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TheBlogApi.Models.Types;
using TheBlogApi.Helpers.Extensions;

namespace TheBlogApi.Data.Services
{
    public class UserService : IUserService
    {
        private readonly IUserManager _userManager;
        private readonly ISignInManager _signInManager;
        private readonly IDbContextProvider _contextProvider;
        private readonly ICryptoProvider _cryptoProvider;
        private readonly IEmailService _emailService;

        public UserService(IUserManager userManager, ISignInManager signInManager, IDbContextProvider contextProvider, ICryptoProvider cryptoProvider, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _contextProvider = contextProvider;
            _cryptoProvider = cryptoProvider;
            _emailService = emailService;
        }

        public async Task<EntityResponse<UserDTO>> FindByIdAsync(Guid id)
        {
            var user = await _contextProvider.Context.Users.FindAsync(id).ConfigureAwait(false);
            if (user == null)
            {
                return new EntityResponse<UserDTO>("User not found.");
            }
            return new EntityResponse<UserDTO>(Mapper.Map<UserDTO>(user));
        }

        public async Task<EntityResponse<User>> FindByTenantKeyAsync(string tenantKey)
        {
            var tenant = await _contextProvider.Context.Tenants.FirstOrDefaultAsync(t => t.TenantKey == tenantKey);
            if (tenant == null)
                return new EntityResponse<User>("Tenantkey is unkown.");

            var user = await _contextProvider.Context.Users
                .FirstOrDefaultAsync(u => u.Id == tenant.UserId);

            if (user == null)
                return new EntityResponse<User>("Tenantkey is unkown.");

            user.Tenant = tenant;

            return new EntityResponse<User>(user);
        }

        public async Task<EntityResponse<UserDTO>> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(_cryptoProvider.EncryptPrivate(email)).ConfigureAwait(false);
            if (user == null)
            {
                return new EntityResponse<UserDTO>("User not found.");
            }
            return new EntityResponse<UserDTO>(Mapper.Map<UserDTO>(user));
        }

        public async Task<ServiceResponse> CreateAsync(UserDTO userDTO, string password)
        {
            var checkIfExists = await CheckUserExistsAsync(userDTO).ConfigureAwait(false);
            if (!checkIfExists.Succeeded) return checkIfExists;

            var user = Mapper.Map<User>(userDTO);

            // the decrypted email will be used later
            var email = user.Email;
            user.Email = _cryptoProvider.EncryptPrivate(email);

            var result = await _userManager.CreateAsync(user, password).ConfigureAwait(false);
            if (!result.Succeeded) return new ServiceResponse(string.Join(", ", result.Errors.Select(e => e.Description)));

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);

            await _emailService
                .ConfirmationEmailNotificationAsync(email, token)
                .ConfigureAwait(false);

            return new ServiceResponse();
        }

        public async Task<ServiceResponse> CheckUserExistsAsync(UserDTO user)
        {
            if (await _contextProvider.Context.Users.AnyAsync(u => u.UserName == user.UserName).ConfigureAwait(false))
                return new ServiceResponse($"This username is already taken: {user.UserName}");

            if (await _contextProvider.Context.Users.AnyAsync(u => u.Email == _cryptoProvider.EncryptPrivate(user.Email)).ConfigureAwait(false))
                return new ServiceResponse($"This email address is already taken: {user.Email}");

            return new ServiceResponse();
        }

        public async Task<ServiceResponse> ForgotPasswordAsync(string email)
        {
            var encryptedEmail = _cryptoProvider.EncryptPrivate(email);
            var user = await _userManager.FindByEmailAsync(_cryptoProvider.EncryptPrivate(encryptedEmail)).ConfigureAwait(false);
            if (user == null)
                return new ServiceResponse();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
            await _emailService.ForgotPasswordNotificationAsync(encryptedEmail, token).ConfigureAwait(false);
            return new ServiceResponse();
        }

        public async Task<ServiceResponse> ResetPasswordAsync(string email, string token, string password)
        {
            var user = await _userManager.FindByEmailAsync(_cryptoProvider.EncryptPrivate(email)).ConfigureAwait(false);
            if (user == null)
                return new ServiceResponse("User not found.");


            var result = await _userManager.ResetPasswordAsync(user, WebUtility.UrlDecode(token), password).ConfigureAwait(false);
            if (!result.Succeeded)
                return new ServiceResponse($"Reset password failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");


            token = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
            result = await _userManager.ConfirmEmailAsync(user, token).ConfigureAwait(false);

            if (!result.Succeeded)
                return new ServiceResponse($"Confirm account failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            return new ServiceResponse();
        }

        public async Task<ServiceResponse> ActivateAccountAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(_cryptoProvider.EncryptPrivate(email)).ConfigureAwait(false);
            if (user == null)
                return new ServiceResponse("User not found.");

            var result = await _userManager.ConfirmEmailAsync(user, WebUtility.UrlDecode(token)).ConfigureAwait(false);
            if (!result.Succeeded)
                return new ServiceResponse($"Account activation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            if (!await _contextProvider.Context.Tenants.AnyAsync(t => t.UserId == user.Id).ConfigureAwait(false))
            {
                await _contextProvider.ExecuteTransactionAsync(async context =>
                {
                    var tenant = new Tenant
                    {
                        ExpirationDateUtc = DateTime.UtcNow.AddYears(100),
                        IsBlocked = false,
                        UserId = user.Id,
                        TenantKey = WebUtility.UrlDecode(user.Id.Hash())
                    };
                    await context.Tenants.AddAsync(tenant);
                    await context.SaveChangesAsync();
                    return new ServiceResponse();
                }).ConfigureAwait(false);
            }
            return new ServiceResponse();
        }

        public async Task<ServiceResponse> AddToRoleAsync(string email, Roles role)
        {
            var user = await _userManager.FindByEmailAsync(_cryptoProvider.EncryptPrivate(email)).ConfigureAwait(false);
            if (user == null)
                return new ServiceResponse("User not found.");

            var result = await _userManager.AddToRoleAsync(user, role.ToString()).ConfigureAwait(false);
            if (!result.Succeeded)
                return new ServiceResponse(string.Join(", ", result.Errors.Select(e => e.Description)));

            return new ServiceResponse();
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(_cryptoProvider.EncryptPrivate(email));
            if (user == null)
                throw new Exception("User not found.");

            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GetTenantKeyByUserIdAsync(Guid id)
        {
            return await _contextProvider.Context.Tenants
                .Where(t => t.UserId == id)
                .Select(t => t.TenantKey)
                .FirstOrDefaultAsync();
        }
    }
}
