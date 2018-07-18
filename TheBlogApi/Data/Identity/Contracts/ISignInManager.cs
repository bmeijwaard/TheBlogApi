using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TheBlogApi.Models.Domain;

namespace TheBlogApi.Data.Identity.Contracts
{
    public interface ISignInManager
    {
        Task<ClaimsPrincipal> CreateUserPrincipalAsync(User user);
        IHttpContextAccessor GetHttpContext();
        bool IsSignedIn(ClaimsPrincipal user);
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe, bool lockoutOnFailure);
        Task SignInAsync(User user, bool isPersistent);

        Task SignOutAsync();
        Task<bool> CanSignInAsync(User user);
    }
}