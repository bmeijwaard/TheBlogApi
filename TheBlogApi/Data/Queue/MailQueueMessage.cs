using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Data.Queue
{
    public class MailQueueMessage
    {
        public MailQueueMessage() { }
        public MailQueueMessage(string to, string subject, string body, string from, IEnumerable<string> additionalAddresses = null)
        {
            To = to;
            Subject = subject;
            Body = body;
            From = from;
            AdditionalAddresses = additionalAddresses;
        }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public IEnumerable<string> AdditionalAddresses { get; set; }
    }
}
