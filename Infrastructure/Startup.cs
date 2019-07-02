using System;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
using ReactDemo.Infrastructure.Repositories;
using ReactDemo.Infrastructure.Security.Authentication;
using ReactDemo.Infrastructure.Security.Authorization;
using ReactDemo.Infrastructure.Utils;

namespace ReactDemo
{
    public class Startup
    {
        public static JwtConfig JwtConfig { private set; get; }

        public static DefaultConfig DefaultConfig { private set; get;}

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            var authenticationConfig = Configuration.GetSection("Authentication").Get<AuthenticationConfig>();
            JwtConfig = authenticationConfig.Jwt;
            DefaultConfig = authenticationConfig.Default;
            _logger = loggerFactory.CreateLogger<Startup>();
        }

        public IConfiguration Configuration { get; }

        private readonly ILogger<Startup> _logger;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContextPool<DatabaseContext>(optionBuilder => optionBuilder.UseMySQL(Configuration.GetConnectionString("test")));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddTransient<IUserAppService, UserAppService>();
            services.AddTransient<IUserManager, UserManager>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSession(options => 
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = DefaultConfig.CookieName;
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
