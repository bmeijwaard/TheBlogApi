using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TheBlogApi.Config.Settings;

namespace TheBlogApi.Data.Handlers
{
    public class JobRunner : IJobRunner
    {
        private readonly WebjobSettings _settings;
        private readonly IExceptionHandler _exceptionHandler;

        public JobRunner(IExceptionHandler exceptionHandler, IOptions<WebjobSettings> settings)
        {

            _settings = settings.Value;
            _exceptionHandler = exceptionHandler;
        }
        public async Task RunTriggeredJobAsync()
        {
            var request = (HttpWebRequest)WebRequest.Create(_settings.Webhook);
            request.Method = "POST";
            var byteArray = Encoding.ASCII.GetBytes($"{_settings.Username}:{_settings.Password}");
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(byteArray));
            request.ContentLength = 0;
            var response = (HttpWebResponse)await request.GetResponseAsync();
        }
    }
}
