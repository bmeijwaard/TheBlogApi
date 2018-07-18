using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Config.Settings
{
    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
        public string AzureWebJobsDashboard { get; set; }
        public string AzureWebJobsStorage { get; set; }
    }
}
