using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TheBlogApi.Data.Context;
using TheBlogApi.Data.Context.Contracts;
using TheBlogApi.Data.Context.Providers;
using TheBlogApi.Data.Context.Providers.Contracts;
using TheBlogApi.Models.Domain;
using TheBlogApi.Data.Handlers;
using TheBlogApi.Data.Identity;
using TheBlogApi.Data.Identity.Contracts;
using TheBlogApi.Data.Providers;
using TheBlogApi.Data.Providers.Contracts;
using TheBlogApi.Data.Services;
using TheBlogApi.Data.Services.Contracts;

namespace TheBlogApi.Config
{
    public static class AppModules
    {
        private const string EmailConfirmationTokenProviderName = "ConfirmEmail";

        public static IServiceCollection Load(IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<SqlContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Tokens.ChangePhoneNumberTokenProvider = EmailConfirmationTokenProviderName;
            });
            services.Configure<ConfirmEmailDataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromDays(7);
            });

            services.AddIdentity<User, Role>(options =>
            {
                options.Lockout = new LockoutOptions
                {
                    AllowedForNewUsers = true,
                    DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30),
                    MaxFailedAccessAttempts = 5
                };
            })
            .AddEntityFrameworkStores<SqlContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<ConfirmEmailDataProtectorTokenProvider<User>>(EmailConfirmationTokenProviderName)
            .AddUserStore<UserStore<User, Role, SqlContext, Guid>>()
            .AddRoleStore<RoleStore<Role, SqlContext, Guid>>()
            .AddUserManager<UserManager>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = true;
            });

            services.ConfigureApplicationCookie(options =>
            options.Events = new CookieAuthenticationEvents
            {
                OnRedirectToLogin = ctx =>
                {
                    if (ctx.Request.Path.StartsWithSegments("/api") &&
                        ctx.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    else if (ctx.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    }
                    else
                    {
                        ctx.Response.Redirect(ctx.RedirectUri);
                    }
                    return Task.FromResult(0);
                }
            });
            // this need to be instantiated only once during application lifetime

            services.AddSingleton<IQueueProvider, QueueProvider>();
            services.AddSingleton<ICachingProvider, CachingProvider>();
            services.AddSingleton<IStorageProvider, StorageProvider>();

            // specific injection of DbContext/Dapper to ensure thread safety.
            services.AddTransient<IDbContext>(provider => provider.GetService<SqlContext>());
            //services.AddTransient<IDapperProvider>(provider => new DapperProvider(config.GetConnectionString("DefaultConnection")));
            services.AddTransient<IDbContextProvider, DbContextProvider>();

            // alterative options to use delegates, but that needs more refinement.
            services.AddTransient(provider => new Func<SqlContext>(provider.GetService<SqlContext>));

            // providers etc
            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<ISignInManager, SignInManager>();
            services.AddTransient<ICryptoProvider, CryptoProvider>();
            services.AddTransient<IEmailProvider, EmailProvider>();

            // services
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<IJobRunner, JobRunner>();
            services.AddTransient<IExceptionHandler, ExceptionHandler>();

            // this need to be insured transient.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();            
            services.AddTransient<IHttpAccessProvider, HttpAccessProvider>();

            return services;
        }
    }

    public class ConfirmEmailDataProtectorTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public ConfirmEmailDataProtectorTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<ConfirmEmailDataProtectionTokenProviderOptions> options) : base(dataProtectionProvider, options)
        {
        }
    }

    public class ConfirmEmailDataProtectionTokenProviderOptions : DataProtectionTokenProviderOptions { }
}

