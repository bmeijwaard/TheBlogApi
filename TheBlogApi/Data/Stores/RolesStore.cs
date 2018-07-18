using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Data.Stores
{
    public static class RolesStore
    {
        public const string ADMINISTRATOR = "Administrator";
        public const string USER = "User";
        public const string TENANT = "Tenant";

        public const string ADMINISTRATOR_USER = "Administrator, User";       
        public const string ADMINISTRATOR_USER_TENANT = "Administrator, User, Tenant";       

    }
}
