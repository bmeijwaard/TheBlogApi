using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using TheBlogApi.Data.Stores;

namespace TheBlogApi.Data.Identity
{
    public static class IdenityExtensions
    {
        public static string UserName(this IIdentity identity) => ((ClaimsIdentity)identity).FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        public static string FirstName(this IIdentity identity) => ((ClaimsIdentity)identity).FindFirst(ClaimTypes.GivenName)?.Value ?? string.Empty;
        public static string LastName(this IIdentity identity) => ((ClaimsIdentity)identity).FindFirst(ClaimTypes.Surname)?.Value ?? string.Empty;
        public static Guid Id(this IIdentity identity) => new Guid(((ClaimsIdentity)identity).FindFirst(ClaimTypes.Sid)?.Value ?? Guid.Empty.ToString());
        public static bool IsAdmin(this IIdentity identity) => ((ClaimsIdentity)identity).FindFirst(ClaimsStore.ADMINISTRATOR)?.Value == "True";
        public static bool IsAccountConfirmed(this IIdentity identity) => ((ClaimsIdentity)identity).FindFirst(ClaimsStore.ACCOUNT_CONFIRMED)?.Value == "True";

        public static string Role(this IIdentity identity) => ((ClaimsIdentity)identity).FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        public static string TenantKey(this IIdentity identity) => ((ClaimsIdentity)identity).FindFirst(ClaimsStore.TENANT_KEY)?.Value ?? string.Empty;
        public static Guid TenantUserId(this IIdentity identity) => new Guid(((ClaimsIdentity)identity).FindFirst(ClaimsStore.USER_ID)?.Value ?? Guid.Empty.ToString());
    }
}
