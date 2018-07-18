using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Helpers.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToLocalString(this DateTime input, string format = "dd-MM-yyyy HH:mm:ss")
        {
            return DateTime.SpecifyKind(input, DateTimeKind.Utc).ToLocalTime().ToString(format);
        }

        public static string ToLocalString(this DateTime? input, string format = "dd-MM-yyyy HH:mm:ss")
        {
            if (input == null) return string.Empty;
            return ToLocalString((DateTime)input, format);
        }

        public static DateTime FromUnixTimestamp(this double timestamp, bool flattenMinutes = false)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var result = origin.AddMilliseconds(timestamp);
            if (flattenMinutes) result = origin.AddMilliseconds(timestamp - result.Millisecond);
            return result;
        }

        public static DateTime? FromUnixTimestamp(this double? timestamp, bool flattenMinutes = false)
        {
            if (timestamp == null) return null;
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var result = origin.AddMilliseconds((double)timestamp);
            if (flattenMinutes) result = origin.AddMilliseconds((double)timestamp - result.Millisecond);
            return result;
        }

        public static double ToUnixTimestamp(this DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalMilliseconds);
        }
    }
}
