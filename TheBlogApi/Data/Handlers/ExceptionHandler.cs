using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Config.Settings;
using TheBlogApi.Data.Providers;
using TheBlogApi.Data.Providers.Contracts;
using TheBlogApi.Data.Queue;
using TheBlogApi.Data.Services.Contracts;
using TheBlogApi.Data.Stores;

namespace TheBlogApi.Data.Handlers
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly IEmailService _emailService;
        private readonly IQueueProvider _queueProvider;
        private readonly ExceptionSettings _settings;

        public ExceptionHandler(IEmailService emailService, IOptions<ExceptionSettings> settings, IQueueProvider queueProvider)
        {
            _emailService = emailService;
            _queueProvider = queueProvider;
            _settings = settings.Value;
        }

        /// <summary>
        /// Exceltion handling.
        /// </summary>
        /// <typeparam name="T">typeof Exception</typeparam>
        /// <param name="exception">The Exception</param>
        /// <param name="source">default(null): The calling function or another source if otherwise.</param>
        /// <returns>Task</returns>
        public async Task ProcessExceptionAsync<T>(T exception, string source = null) where T : Exception
        {
            await _processExceptionAsync(exception.Message, exception, source).ConfigureAwait(false);
        }

        /// <summary>
        /// Exceltion handling.
        /// </summary>
        /// <typeparam name="T">typeof Exception</typeparam>
        /// <param name="message">Custom message to replace the eception message.</param>
        /// <param name="exception">The Exception</param>
        /// <param name="source">default(null): The calling function or another source if otherwise.</param>
        /// <returns>Task</returns>
        public async Task ProcessExceptionAsync<T>(string message, T exception, string source = null) where T : Exception
        {
            await _processExceptionAsync(message, exception, source).ConfigureAwait(false);
        }

        /// <summary>
        /// Exceltion handling.
        /// </summary>
        /// <typeparam name="T">typeof Exception</typeparam>
        /// <param name="message">Custom message to replace the eception message.</param>
        /// <param name="exception">The Exception</param>
        /// <param name="source">default(null): The calling function or another source if otherwise.</param>
        /// <param name="data">A data object the will be serialized as a string. This usually contains the source parameters or relevant object that was used and might give hints to the cause of the exception.</param>
        /// <returns>Task</returns>
        public async Task ProcessExceptionAsync<T>(string message, T exception, string source, object data) where T : Exception
        {
            await _processExceptionAsync(message, exception, source, data).ConfigureAwait(false);
        }

        private async Task _processExceptionAsync<T>(string message, T exception, string source = null, object data = null) where T : Exception
        {
            var queueMessage = new ExceptionQueueMessage<T>(exception, message, source, data);
            await _queueProvider.AddMessageToQueueAsync(QueueStore.PROCESS_EXCEPTION, queueMessage);
        }

        public async Task NotifyExceptionAsync<T>(T exception, string message = null, string source = null, object data = null) where T : Exception
        {
            var mainAddress = _settings.EmailAddresses[0];
            var additionalAddesses =
                _settings.EmailAddresses[0].Length > 1
                    ? _settings.EmailAddresses.Skip(1)
                    : null;

            var title = EmailTitle(message);
            var header = source == null ? HeaderText(message) : data == null ? HeaderText(message, source) : HeaderText(message, source, data);
            var body = string.Format(EmailBody(exception), header);

            await _emailService.SendExceptionEmailNotificationAsync(mainAddress, message, body, title, additionalAddesses).ConfigureAwait(false);
        }

        private string EmailTitle(string message) =>
            $"The Blog Api ({_settings.Env.ToUpper()}) Error: {message}";

        private static string EmailBody<T>(T exception) =>
            "<br>{0}" + $"{JsonConvert.SerializeObject(exception).Replace(@""",""", "<br>").Replace(@",""", "<br>").Replace('"', ' ').Replace('{', ' ').Replace('}', ' ')}";

        private static string HeaderText(string message) =>
            $"<h1>Exception report</h1>Message: {message}<br><h2>Exception</h2>";

        private static string HeaderText(string message, string source) =>
            $"<h1>Exception report</h1>Message: {message}<br>Source: {source}<br><h2>Exception</h2>";

        private static string HeaderText(string message, string source, object data) =>
            $"<h1>Exception report</h1>Message: {message}<br>Source: {source}<br>Data: {JsonConvert.SerializeObject(data)}<br><h2>Exception</h2>";
    }
}

