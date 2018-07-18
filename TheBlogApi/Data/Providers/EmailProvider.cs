using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Config.Settings;
using TheBlogApi.Data.Providers.Contracts;
using TheBlogApi.Data.Queue;
using TheBlogApi.Data.Resources;
using TheBlogApi.Data.Stores;

namespace TheBlogApi.Data.Providers
{
    public class EmailProvider : IEmailProvider
    {
        private readonly SmtpSettings _settings;
        private readonly IQueueProvider _queueProvider;

        public EmailProvider(IOptions<SmtpSettings> settings, IQueueProvider queueProvider)
        {
            _settings = settings.Value;
            _queueProvider = queueProvider;
        }

        public async Task SendEmailAsync(string to, string subject, string body, string from, IEnumerable<string> additionalAddresses = null)
        {
            var message = PrepareMessage(to, subject, body, from, additionalAddresses);
            await SendEmailAsync(message);
        }

        public async Task SendEmailAsync(MailQueueMessage queueMessage)
        {
            var message = PrepareMessage(queueMessage.To, queueMessage.Subject, queueMessage.Body, queueMessage.From, queueMessage.AdditionalAddresses);
            await SendEmailAsync(message);
        }
        
        public async Task PrepareMailQueueMessageAsync(string to, string subject, string body, string from, IEnumerable<string> additionalAddresses = null)
        {
            await _queueProvider.AddMessageToQueueAsync(QueueStore.NEW_MAIL, new MailQueueMessage(to, subject, body, from, additionalAddresses));
        }

        public async Task SendEmailAsync(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                // TODO Uitzoeken welke ssl certificaten worden ondersteund.
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                // client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                
                // TODO check SSL configuration
                client.Connect(_settings.Smtp, _settings.SmtpPort, MailKit.Security.SecureSocketOptions.None);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_settings.SendGridUser, _settings.SendGridPassword);

                await client.SendAsync(message);

                client.Disconnect(true);
            }
        }

        private MimeMessage PrepareMessage(string to, string subject, string body, string from, IEnumerable<string> additionalAddresses = null)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(string.IsNullOrEmpty(from) ? _settings.FromEmailAddress : from));
            message.To.Add(new MailboxAddress(to));
            if (additionalAddresses != null)
            {
                foreach (var email in additionalAddresses)
                {
                    message.Cc.Add(new MailboxAddress(email));
                }
            }

            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                TextBody = body,
                HtmlBody = GetEmailHtmlBody(subject, body)
            };

            message.Body = bodyBuilder.ToMessageBody();

            return message;
        }

        private string GetEmailHtmlBody(string subject, string body)
        {
            var emailTemplate = EmailBody.Base;
            emailTemplate = emailTemplate.Replace("%%EMAILSUBJECT%%", subject);
            return emailTemplate.Replace("%%EMAILBODY%%", body);
        }
    }
}
