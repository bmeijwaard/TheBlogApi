using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using TheBlogApi.Config;
using TheBlogApi.Data.Handlers;
using TheBlogApi.Data.Providers.Contracts;
using TheBlogApi.Data.Stores;
using TheBlogApi.Helpers.Extensions;

namespace TheBlogApi.Continuous
{
    class Program
    {
        private static ServiceProvider _serviceProvider;

        public static ServiceProvider ServiceProvider
        {
            get => EnsureServiceProvider();
        }

        static void Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var configuration = new JobHostConfiguration();
            configuration.Queues.MaxPollingInterval = TimeSpan.FromSeconds(10);
            configuration.Queues.BatchSize = 1;
            configuration.JobActivator = new CustomJobActivator(serviceCollection.BuildServiceProvider());
            configuration.UseTimers();

            var host = new JobHost(configuration);
            host.RunAndBlock();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            EnsureServiceProvider(true, serviceCollection);
        }

        private static ServiceProvider EnsureServiceProvider(bool initialize = false, IServiceCollection services = default(IServiceCollection))
        {
            if (_serviceProvider != null) return _serviceProvider;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>()
                .Build();

            if (!initialize)
            {
                services = new ServiceCollection();
            }

            services = AppSettings.Load(services, configuration);
            services = AppModules.Load(services, configuration);

            services.AddDataProtection(opt => opt.ApplicationDiscriminator = "d4b5ab63-7874-40e0-8c07-0409779ccb0f");

            // Your classes that contain the webjob methods need to be DI-ed up too
            services.AddScoped<WebJobsMethods>();

            if (initialize)
            {
                Mapper.Initialize(cfg => { cfg.AddProfile<ApiProfile>(); });
                Environment.SetEnvironmentVariable("AzureWebJobsDashboard", configuration.GetConnectionString("AzureWebJobsDashboard"));
                Environment.SetEnvironmentVariable("AzureWebJobsStorage", configuration.GetConnectionString("AzureWebJobsStorage"));
            }

            _serviceProvider = services.BuildServiceProvider();
            return _serviceProvider;
        }
    }

    public class CustomJobActivator : IJobActivator
    {
        private readonly IServiceProvider _service;
        public CustomJobActivator(IServiceProvider service)
        {
            _service = service;
        }

        public T CreateInstance<T>()
        {
            var service = _service.GetService<T>();
            return service;
        }
    }


    public class WebJobsMethods
    {
        private readonly IQueueProvider _queueProvider;
        private readonly IJobRunner _jobRunner;

        public WebJobsMethods(IQueueProvider queueProvider, IJobRunner jobRunner)
        {

            _queueProvider = queueProvider;
            _jobRunner = jobRunner;
        }

        public async Task ScheduledMaintenanceAsync([TimerTrigger("0 */15 * * * *", RunOnStartup = true)] TimerInfo timerInfo, TextWriter log)
        {
            Console.WriteLine($"Timer job fired!: {DateTime.UtcNow.ToLocalString()}");

            // start the triggered job if it is not running
            try
            {
                // will be thrown if it is running already.
                await _jobRunner.RunTriggeredJobAsync();
            }
            catch
            {
                // we ping the triggered webjob to keep it alive if it is already running
                await _queueProvider.AddMessageToQueueAsync(QueueStore.KEEP_ALIVE, DateTime.UtcNow);
            }
        }
    }
}
