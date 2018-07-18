using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Config.Settings
{
    public class CacheSettings
    {
        public string ConnectionString { get; set; }
        public bool UseCache { get; set; }
        public string DefaultExpiry { get; set; }
    }
}
