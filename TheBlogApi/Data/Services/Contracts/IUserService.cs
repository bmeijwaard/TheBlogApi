using System;
using System.Threading.Tasks;
using TheBlogApi.Models.Domain;
using TheBlogApi.Data.Messages;
using TheBlogApi.Models.DTO;
using TheBlogApi.Models.Types;

namespace TheBlogApi.Data.Services.Contracts
{
    public interface IUserService
    {
        Task<ServiceResponse> CheckUserExistsAsync(UserDTO user);
        Task<ServiceResponse> CreateAsync(UserDTO user, string password);
        Task<EntityResponse<UserDTO>> FindByEmailAsync(string email);
        Task<EntityResponse<UserDTO>> FindByIdAsync(Guid id);
        Task<EntityResponse<User>> FindByTenantKeyAsync(string tenantKey);
        Task<ServiceResponse> ForgotPasswordAsync(string email);
        Task<ServiceResponse> ResetPasswordAsync(string email, string token, string password);
        Task<ServiceResponse> ActivateAccountAsync(string email, string token);
        Task<ServiceResponse> AddToRoleAsync(string email, Roles role);
        Task<string> GenerateEmailConfirmationTokenAsync(string email);
        Task<string> GetTenantKeyByUserIdAsync(Guid id);
    }
}