using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReactDemo.Application.Services;
using ReactDemo.Domain.Models.System;
using ReactDemo.Domain.Repositories;
using ReactDemo.Infrastructure.Repositories;
using ReactDemo.Infrastructure.Security.Authentication;
using ReactDemo.Infrastructure.Utils;

namespace ReactDemo
{
    public class Startup
    {

        public const string SchemeName = "PartyAuth";

        public const string CookieName = "PartyBuildCookie";

        public const string KeyName = "OpenKey";

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _logger = loggerFactory.CreateLogger<Startup>();
        }

        public IConfiguration Configuration { get; }

        private readonly ILogger<Startup> _logger;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContextPool<DatabaseContext>(optionBuilder => optionBuilder.UseMySQL(Configuration.GetConnectionString("test")));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddTransient<IUserAppService, UserAppService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSession(options => 
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = CookieName;
            });
            services.AddDistributedMemoryCache();
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

            services.AddDataProtection();
            services.AddSingleton<IDataSerializer<AuthenticationTicket>, TicketSerializer>();
            // services.AddSingleton<ISecureDataFormat<AuthenticationTicket>, DefaultAuthenticationDataFormat>();

            // services.AddDefaultIdentity<User>().AddEntityFrameworkStores<DatabaseContext>();
            // services.Configure<IdentityOptions>(options =>
            // {
            //     // Password settings.
            //     options.Password.RequireDigit = true;
            //     options.Password.RequireLowercase = true;
            //     options.Password.RequireNonAlphanumeric = true;
            //     options.Password.RequireUppercase = true;
            //     options.Password.RequiredLength = 6;
            //     options.Password.RequiredUniqueChars = 1;

            //     // Lockout settings.
            //     options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //     options.Lockout.MaxFailedAccessAttempts = 5;
            //     options.Lockout.AllowedForNewUsers = true;

            //     // User settings.
            //     options.User.AllowedUserNameCharacters =
            //     "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            //     options.User.RequireUniqueEmail = false;
            // });
            services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = SchemeName;
                options.DefaultSignInScheme = SchemeName;
            }).AddScheme<DefaultAuthenticationOptions, DefaultAuthenticationHandler>(SchemeName, options => 
            {
                options.LoginPath = new PathString("/user/login");
                options.LogoutPath = new PathString("/user/logout");
                options.Whitelist = new List<PathString>
                {
                    new PathString("/user/verifycode"),
                    new PathString("/static/*"),
                    new PathString("/sockjs-node/*"),
                    new PathString("/")
                };
            });
            // .AddCookie(SchemeName, options =>
            // {
            //     // Cookie settings
            //     options.Cookie.HttpOnly = true;
            //     options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

            //     options.LoginPath = "/user/login";
            //     options.LogoutPath = "/user/logout";
            //     // 访问拒绝路径
            //     // options.AccessDeniedPath = "/user/verifycode";
            //     options.SlidingExpiration = true;
            //     options.Cookie.Name = CookieName;

            // });
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

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
