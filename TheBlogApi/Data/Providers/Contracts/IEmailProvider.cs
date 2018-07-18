using MimeKit;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBlogApi.Data.Queue;

namespace TheBlogApi.Data.Providers.Contracts
{
    public interface IEmailProvider
    {
        Task PrepareMailQueueMessageAsync(string to, string subject, string body, string from, IEnumerable<string> additionalAddresses = null);
        Task SendEmailAsync(MailQueueMessage queueMessage);
        Task SendEmailAsync(MimeMessage message);
        Task SendEmailAsync(string to, string subject, string body, string from, IEnumerable<string> additionalAddresses = null);
    }
}