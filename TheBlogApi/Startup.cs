using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using TheBlogApi.Config;
using TheBlogApi.Data.Stores;
using TheBlogApi.Helpers.Adapters;
using TheBlogApi.Helpers.Filters;
using TheBlogApi.Helpers.Middleware;

namespace TheBlogApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private static ServiceProvider _serviceProvider;
        public static ServiceProvider ServiceProvider
        {
            get => _serviceProvider;
        }

        private RoleSeeder RoleSeeder;
        private UserSeeder UserSeeder;


        public void ConfigureServices(IServiceCollection services)
        {
            services = AppSettings.Load(services, Configuration);
            services = AppModules.Load(services, Configuration);

            services.AddScoped<IAuthenticationAdapter, AuthenticationAdapter>();
            services.AddTransient<ControllerContext>();

            AutoMapperConfig.Register();

            services.AddMemoryCache();

            var jwtSetting = Configuration.GetSection("JWTSettings");
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Configuration = new OpenIdConnectConfiguration();
                    options.Authority = jwtSetting.GetValue<string>("SiteAddress");
                    options.Audience = jwtSetting.GetValue<string>("Audience");
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {                            
                            NameClaimType = ClaimTypes.Name,
                            RoleClaimType = ClaimTypes.Role,
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = jwtSetting.GetValue<string>("SiteAddress"),
                            ValidAudience = jwtSetting.GetValue<string>("Audience"),
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.GetValue<string>("SecurityKey")))
                        };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Debug.WriteLine($"OnAuthenticationFailed: {context.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Debug.WriteLine($"OnTokenValidated: {context.SecurityToken}");
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DefaultPolicy", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });
            });

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.FormatterMappings.SetMediaTypeMappingForFormat("json", "application/json");
            })
            .AddJsonOptions(options =>
             {
                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                 options.SerializerSettings.DateFormatString = "yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'";
             });

            services.AddSwaggerGen(swagger =>
            {
                swagger.DescribeAllEnumsAsStrings();
                swagger.DescribeAllParametersInCamelCase();
                swagger.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "TheBlogApi.xml"));
                swagger.SwaggerDoc("v1", new Info
                {
                    Title = "The Blog API",
                    Description = "The Blog API",
                    Version = "V1",
                    Contact = new Contact() { Email = "info@bobdebouwer.net", Name = "Bob Meijwaard", Url = "https://bobdebouwer.net" },
                    TermsOfService = "Terms of Service",
                    License = new License() { Name = "License", Url = "https://bobdebouwer.net" }
                });
                swagger.AddSecurityDefinition("JWT", new ApiKeyScheme() { In = "header", Description = "The Blog API", Name = "Authorization", Type = "apiKey" });
            });

            services.ConfigureSwaggerGen(swagger =>
            {
                swagger.OperationFilter<FormFileOperationFilter>();
            });

            services.AddTransient<RoleSeeder>();
            services.AddTransient<UserSeeder>();

            _serviceProvider = services.BuildServiceProvider();

            RoleSeeder = _serviceProvider.GetService<RoleSeeder>();
            UserSeeder = _serviceProvider.GetService<UserSeeder>();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                RoleSeeder.Seed().Wait();
                UserSeeder.Seed().Wait();

                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseRequestHandlerMiddleware();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "The Blog Api");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "login",
                   template: "",
                   defaults: new { controller = "home", action = "index" }
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
