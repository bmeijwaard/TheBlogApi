using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Helpers
{
    public static class MessageHelper
    {
        public static string BlockedUntilMessage(DateTime blockedUntilUtc)
        {
            var diff = blockedUntilUtc - DateTime.UtcNow;
            return $"Your account is locked out for {diff.Days} days, {diff.Hours} hours and {diff.Minutes} minutes.";
        }

        public static string BlockedUntilMessage(DateTime? blockedUntilUtc)
        {
            if (blockedUntilUtc == null) return string.Empty;
            return BlockedUntilMessage((DateTime)blockedUntilUtc);
        }
    }
}
