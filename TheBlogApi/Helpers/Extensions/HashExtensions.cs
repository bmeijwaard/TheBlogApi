using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TheBlogApi.Helpers.Extensions
{
    public static class HashExtensions
    {
        public static string Hash(this Guid input) => input.Flatten().Hash();

        public static string Hash<T>(this T input, int rotations = 10) => Hash(S(JsonConvert.SerializeObject(input), rotations));

        public static string Hash(this string input) => ComputeHash(Encoding.ASCII.GetBytes(input));

        private static string ComputeHash(byte[] objectAsBytes)
        {
            var result = SHA256.Create().ComputeHash(objectAsBytes);
            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Used for additional encryption. The output is not random.
        /// </summary>
        /// <param name="i">input</param>
        /// <param name="r">rotations</param>
        /// <returns>#</returns>
        private static string S(this string i, int r)
        {
            var l = i.ToCharArray(); var c = l.Length; var s = new List<char>();
            for (var x = c % 2 == 0 ? 1 : 0; x < c; x = x + 2) s.Add(l[x]);
            for (var y = c - 2; y > 1; y = y - 2) s.Add(l[y]);
            var f = string.Join("", s).Trim(); r--;
            if (r > 0) S(f, r);
            return f;
        }
    }
}
