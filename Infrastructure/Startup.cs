using System;
using System.Security.Claims;
using System.Text;
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
using ReactDemo.Domain.Repositories;
using ReactDemo.Domain.Services;
using ReactDemo.Infrastructure.Config.Authentication;
using ReactDemo.Infrastructure.Config.Cache;
using ReactDemo.Infrastructure.Repositories;
using ReactDemo.Infrastructure.Security.Authentication;
using ReactDemo.Infrastructure.Security.Authorization;
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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<DatabaseContext>(optionBuilder => optionBuilder.UseMySQL(Configuration.GetConnectionString("MySQL")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddTransient<IUserAppService, UserAppService>();
            services.AddTransient<IUserManager, UserManager>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                        .WithMethods("post", "get")
                        .WithHeaders("Origin", "Content-Type", "Accept", "Authorization");
                    builder.AllowCredentials();
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

            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

            services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtConfig.SchemeName;
                options.DefaultSignInScheme = JwtConfig.SchemeName;
                options.DefaultChallengeScheme = JwtConfig.SchemeName;

            }).AddScheme<JwtBearerOptions, JwtAuthenticationHandler>(JwtConfig.SchemeName, options => 
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = ClaimTypes.Role,

                    ValidIssuer = JwtConfig.Issuer,
                    ValidAudience = JwtConfig.Audience,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.SecretKey))
                };

                options.Audience = JwtConfig.Audience;
                options.ClaimsIssuer = JwtConfig.Issuer;
                
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
                app.UseExceptionHandler("/Error");
                // app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseCors("AllowSpecificOrigins");
            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp"; 

                // if (env.IsDevelopment())
                // {
                //     spa.UseReactDevelopmentServer(npmScript: "start");
                // }
            });

        }
    }
}
