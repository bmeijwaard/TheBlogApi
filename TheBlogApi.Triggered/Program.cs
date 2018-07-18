using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using TheBlogApi.Config;

namespace TheBlogApi.Triggered
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
            try
            {
                IServiceCollection serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);

                var jobConfig = new JobHostConfiguration();
                jobConfig.UseFiles();
                jobConfig.UseTimers();
                jobConfig.Queues.MaxDequeueCount = 1;
                jobConfig.JobActivator = new ShootlrJobActivator(serviceCollection.BuildServiceProvider());

                JobHost host = new JobHost(jobConfig);
                host.RunAndBlock();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
                    .AddUserSecrets<Program>()
                    .AddEnvironmentVariables()
                .Build();

            if (!initialize)
            {
                services = new ServiceCollection();
            }

            services = AppSettings.Load(services, configuration);
            services = AppModules.Load(services, configuration);
            

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

    public class ShootlrJobActivator : IJobActivator
    {
        private readonly IServiceProvider _service;
        public ShootlrJobActivator(IServiceProvider service)
        {
            _service = service;
        }

        public T CreateInstance<T>()
        {
            var service = _service.GetService<T>();
            return service;
        }
    }
}
