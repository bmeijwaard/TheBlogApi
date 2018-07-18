using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Config.Settings
{
    public class JWTSettings
    {
        public string SiteAddress { get; set; }
        public string Audience { get; set; }
        public string SecurityKey { get; set; }
        public string DeviceKey { get; set; }
        public int ExpiresAfterHours { get; set; }
    }
}
