using Ganss.XSS;
using System;

namespace TheBlogApi.Helpers.Extensions
{
    public static class StringExtensions
    {

        /// <summary>
        /// Returns max given characters from a string.
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="maximum">Maximum characters to return</param>
        /// <returns>Truncated string</returns>
        public static string ToMaxCharacters(this string value, int maximum)
        {
            if (value == null) return null;
            return value.Length > maximum ? value.Substring(0, maximum) : value;
        }

        /// <summary>
        /// Makes the first char of a string a cap, and the rest lowercase.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="restToLower">Set to false to only change the first char to upper.</param>
        /// <returns></returns>
        public static string FirstOnlyToUpper(this string value, bool restToLower = true)
        {
            if (string.IsNullOrEmpty(value)) return null;

            var firstChar = value.ToUpper()[0];
            var theRest = restToLower ? value.Remove(0, 1).ToLower() : value.Remove(0, 1);
            return firstChar + theRest;
        }

        public static string FirstToLower(this string value)
        {
            if (string.IsNullOrEmpty(value)) return null;

            var firstChar = value.ToLower()[0];
            var theRest = value.Remove(0, 1);
            return firstChar + theRest;
        }

        /// <summary>
        /// StringComperer with option to ignore case sensitive
        /// </summary>
        /// <param name="source"></param>
        /// <param name="toCheck"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source != null && toCheck != null && source.IndexOf(toCheck, comp) >= 0;
        }


        /// <summary>
        /// Clean HTML code from wysiwyg editors
        /// </summary>
        /// <param name="input">Input HTML</param>
        /// <returns>Sanatized HTML</returns>
        public static string SanatizeHtml(this string input)
        {
            var sanatizer = new HtmlSanitizer();
            var output = sanatizer.Sanitize(input);
            return output;
        }

        /// <summary>
        /// Replace unicoding to actual {} for string formatting
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ReplaceBrackets(this string input)
        {
            var output = input.Replace("%7B", "{").Replace("%7D", "}");
            return output;
        }

        /// <summary>
        /// Formats a Guid to string, removing hyphens
        /// </summary>
        /// <param name="input"></param>
        /// <returns>00000000000000000000000000000000</returns>
        public static string Flatten(this Guid input) => input.ToString("N");

        /// <summary>
        /// Validate if a string is a valid basse 64 string
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static bool IsBase64(this string base64String)
        {

            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0 || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
            {
                return false;
            }

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Splits a string into a string array.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="split">Seperator char.</param>
        /// <returns></returns>
        public static string[] ToStringArray(this string input, char split)
        {
            if (string.IsNullOrWhiteSpace(input)) return new string[0];
            return input.Split(split, StringSplitOptions.RemoveEmptyEntries);
        }
        public static string[] ToStringArray(this string input)
        {
            return input.ToStringArray('|');
        }


        /// <summary>
        /// Joins a string array.
        /// </summary>
        /// <param name="input">The input string array.</param>
        /// <param name="split">Seperator char.</param>
        /// <returns></returns>     
        public static string ToArrayString(this string[] input, char split)
        {
            if (input.Length == 0) return string.Empty;
            if (input.Length == 1) return input[0].Trim();
            return string.Join(split, input);
        }
        public static string ToArrayString(this string[] input)
        {
            return input.ToArrayString('|');
        }
    }
}
