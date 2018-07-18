using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TheBlogApi.Models.Domain;
using TheBlogApi.Data.Identity.Contracts;
using TheBlogApi.Data.Providers.Contracts;
using TheBlogApi.Data.Services.Contracts;
using TheBlogApi.Helpers.Extensions;

namespace TheBlogApi.Data.Identity
{
    public class UserManager : UserManager<User>, IUserManager
    {
        public UserManager(
            IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services, ILogger<UserManager<User>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public async Task<DateTime?> IsBlockedAsync(Guid userId)
        {
            var user = await FindByIdAsync(userId.ToString());
            if (user == null)
                throw new NullReferenceException("User not found");

            var now = DateTime.UtcNow;
            if (user.LockoutEnabled)
                if (user.BlockedUntilUtc != null && user.BlockedUntilUtc >= now)
                    if (user.LockoutEnd != null && user.LockoutEnd >= now)
                        return user.BlockedUntilUtc;

            return null;
        }
    }
}
