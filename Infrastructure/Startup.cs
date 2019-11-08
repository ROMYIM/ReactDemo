using System;
using System.Text;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ReactDemo.Application.Services;
using ReactDemo.Domain.Models.User;
using ReactDemo.Domain.Repositories;
using ReactDemo.Domain.Services;
using ReactDemo.Infrastructure.Config.Authentication;
using ReactDemo.Infrastructure.Config.Cache;
using ReactDemo.Infrastructure.Event.Buses;
using ReactDemo.Infrastructure.Event.Events.Domain;
using ReactDemo.Infrastructure.Event.Handlers;
using ReactDemo.Infrastructure.Event.Handlers.Domain;
using ReactDemo.Infrastructure.Event.Helpers;
using ReactDemo.Infrastructure.Repositories;
using ReactDemo.Infrastructure.Security.Authentication;
using ReactDemo.Infrastructure.Security.Authorization;
using ReactDemo.Infrastructure.Transaction.Attributes;
using ReactDemo.Infrastructure.Utils;
using StackExchange.Redis;

namespace ReactDemo
{
    public class Startup
    {
        public static JwtOptions JwtConfig { private set; get; }

        public static DefaultOptions DefaultConfig { private set; get;}

        public static RedisOptions RedisConfig { get; private set; }

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            var authenticationConfig = Configuration.GetSection("Authentication").Get<Infrastructure.Config.Authentication.AuthenticationOptions>();
            RedisConfig = Configuration.GetSection("Redis").Get<RedisOptions>();
            JwtConfig = authenticationConfig.Jwt;
            DefaultConfig = authenticationConfig.Default;
            _logger = loggerFactory.CreateLogger<Startup>();
        }

        public IConfiguration Configuration { get; }

        private readonly ILogger<Startup> _logger;

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IEventBus, LocalEventBus>();
            services.AddSingleton<IEventHelper, EventHelper>();

            services.AddDbContextPool<DatabaseContext>((serviceProvider, optionBuilder) => 
            {
                optionBuilder.UseMySql(Configuration.GetConnectionString("MySQL"));
                // optionBuilder.UseInternalServiceProvider(serviceProvider);
            }, 100);

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddTransient<IUserAppService, UserAppService>();
            services.AddTransient<UserManager>();

            services.AddTransient<UnitOfWorkAttribute>();

            services.AddTransient<IEventHandler<EntityUpdateEvent<User>>, EntityUpdateEventHandler<User>>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                        .WithMethods("post", "get")
                        .WithHeaders("Origin", "Content-Type", "Accept", "Authorization")
                        .WithExposedHeaders("Authorization");
                    builder.AllowCredentials();
                    builder.AllowAnyHeader();
                });
            });

            services.AddSession(options => 
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = "SessionID";
            });

            // redis缓存
            services.AddStackExchangeRedisCache(options => 
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = RedisConfig.Name;
            });

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ImageUtil>(new ImageUtil());

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = httpContext => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var redisConnect = ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis"));
            services.AddDataProtection().PersistKeysToStackExchangeRedis(redisConnect, RedisConfig.Name + "DataProtection-Keys");                                                            
            services.AddSingleton<IDataSerializer<AuthenticationTicket>, TicketSerializer>();

            // 注册自定义的授权策略提供程序和授权处理程序
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, DefaultAuthorizationHandler>();

            services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtConfig.SchemeName;
                options.DefaultSignInScheme = JwtConfig.SchemeName;
                options.DefaultChallengeScheme = JwtConfig.SchemeName;

            }).AddScheme<JwtBearerOptions, JwtAuthenticationHandler>(JwtConfig.SchemeName, options => 
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "user_id",
                    RoleClaimType = "role_id",

                    ValidIssuer = JwtConfig.Issuer,
                    ValidAudience = JwtConfig.Audience,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.SecretKey))
                };

                options.Audience = JwtConfig.Audience;
                options.ClaimsIssuer = JwtConfig.Issuer;

                options.SaveToken = true;
                
            }).AddScheme<DefaultAuthenticationOptions, DefaultAuthenticationHandler>(DefaultConfig.SchemeName, options => 
            {
                options.LoginPath = new PathString(DefaultConfig.LoginPath);
                options.LogoutPath = new PathString(DefaultConfig.LogoutPath);
                options.CookieName = DefaultConfig.CookieName;
                options.TokenName = DefaultConfig.TokenName;
                options.Whitelist = DefaultConfig.WhiteList;
            });
            
            // 选项配置。读取配置文件中的选项并注册到容器中。选项配置就可以通过依赖注入到其他组件中
            services.Configure<RedisOptions>(Configuration.GetSection("Redis"));
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowSpecificOrigins"));
            });

            services.ConfigureDynamicProxy();
            return services.BuildDynamicProxyServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // var options = new ExceptionHandlerOptions
                // {
                //     ExceptionHandlingPath = "/Error",
                //     ExceptionHandler = context =>
                //     {
                //         var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                //         if (_logger.IsEnabled(LogLevel.Critical))
                //         {
                //             _logger.LogCritical("Request Path {0}", context.Request.Path);
                //             _logger.LogCritical("Response status {0}", context.Response.StatusCode);
                //             _logger.LogCritical("Error Message: {0}", error?.Message);
                //         }
                //         return Task.CompletedTask;
                //     }
                // };
                // app.UseExceptionHandler(options);
                app.UseExceptionHandler("/Error");
                // app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseCors("AllowSpecificOrigins");
            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(/* routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            } */ );

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp"; 

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

        }
    }
}
