using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Config.Settings;

namespace TheBlogApi.Config
{
    public static class AppSettings
    {
        public static IServiceCollection Load(IServiceCollection services, IConfiguration config)
        {
            services.AddOptions();
            services.Configure<SmtpSettings>(opt => config.GetSection("SmtpSettings").Bind(opt));
            services.Configure<JWTSettings>(opt => config.GetSection("JWTSettings").Bind(opt));
            services.Configure<StorageSettings>(opt => config.GetSection("StorageSettings").Bind(opt));
            services.Configure<CacheSettings>(opt => config.GetSection("CacheSettings").Bind(opt));
            services.Configure<ExceptionSettings>(opt => config.GetSection("ExceptionSettings").Bind(opt));
            services.Configure<ConnectionStrings>(opt => config.GetSection("ConnectionStrings").Bind(opt));
            services.Configure<EncryptionSettings>(opt => config.GetSection("EncryptionSettings").Bind(opt));

            return services;
        }
    }
}
