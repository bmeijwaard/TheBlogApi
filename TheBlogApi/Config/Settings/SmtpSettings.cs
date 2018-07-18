using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Config.Settings
{
    public class SmtpSettings
    {
        public string FromEmailAddress { get; set; }

        public string Smtp { get; set; }

        public string SendGridUser { get; set; }

        public string SendGridPassword { get; set; }

        public int SmtpPort { get; set; }
    }
}
