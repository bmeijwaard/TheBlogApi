using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Helpers.Extensions
{
    public static class KeyExtensions
    {
        public static string GetKey<T>(this Guid objectId)
        {
            return $"{typeof(T).Name}_{objectId.ToString("N").ToLower()}";
        }

        public static string GetEnumerableKey<T>()
        {
            return $"{typeof(T).Name}_enumerable";
        }
    }
}
