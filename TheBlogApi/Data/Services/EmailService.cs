using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TheBlogApi.Data.Providers;
using TheBlogApi.Data.Providers.Contracts;
using TheBlogApi.Data.Resources;
using TheBlogApi.Data.Services.Contracts;

namespace TheBlogApi.Data.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailProvider _emailProvider;
        private readonly IHttpAccessProvider _httpAccessProvider;

        public EmailService(IEmailProvider emailProvider, IHttpAccessProvider httpAccessProvider)
        {
            _emailProvider = emailProvider;
            _httpAccessProvider = httpAccessProvider;
        }

        public async Task ForgotPasswordNotificationAsync(string email, string token)
        {
            var link = $"{_httpAccessProvider.CurrentUrl}/account/resetpassword?email={WebUtility.UrlEncode(email)}&code={WebUtility.UrlEncode(token)}";
            var body = EmailBody.Forgot_Password;
            var emailBody = string.Format(body, link);
            await _emailProvider.PrepareMailQueueMessageAsync(email, "The Blog Api - Reset password", emailBody, "no-reply@bobdebouwer.net").ConfigureAwait(false);
        }

        public async Task ConfirmationEmailNotificationAsync(string email, string token)
        {
            var link = $"{_httpAccessProvider.CurrentUrl}/account/accountactivated?email={WebUtility.UrlEncode(email)}&code={WebUtility.UrlEncode(token)}";
            var body = EmailBody.Confirm_Email;
            var emailBody = string.Format(body, link);
            await _emailProvider.PrepareMailQueueMessageAsync(email, "The Blog Api - Activate account", emailBody, "no-reply@bobdebouwer.net").ConfigureAwait(false);
        }


        // this function does not send the mail to the queue, but does so directly to prevent hitting the queue twice
        public async Task SendExceptionEmailNotificationAsync(string mainAddress, string message, string body, string title, IEnumerable<string> additionalAddesses = null)
        {
            await _emailProvider.SendEmailAsync(mainAddress, title, body, "no-reply@bobdebouwer.net", additionalAddesses).ConfigureAwait(false);
        }

        public async Task SendEmailNotificationAsync(string mainAddress, string body, string title, IEnumerable<string> additionalAddesses = null)
        {
            await _emailProvider.SendEmailAsync(mainAddress, title, body, "no-reply@bobdebouwer.net", additionalAddesses).ConfigureAwait(false);
        }
    }
}
