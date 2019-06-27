using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace ReactDemo.Infrastructure.Security.Authentication
{
    public class DefaultAuthenticationOptions : AuthenticationSchemeOptions
    {
        public IDataProtector DataProcetor { get; set; }

        public PathString LoginPath { get; set; } = "/User/Login";

        public PathString LogoutPath { get; set; } = "/User/Logout";

        public List<string> Whitelist { get; set; }

        public TimeSpan ExpiredTimeSpan { get; set; } = TimeSpan.FromHours(1);

        public bool SlidingExpiration { get; set; } = true;

        public string CookieName { get; set; }

        public string TokenName { get; set; }

    }

    public class DefaultAuthenticationHandler : AuthenticationHandler<DefaultAuthenticationOptions>, IAuthenticationSignInHandler, IAuthenticationSignOutHandler
    {
        private readonly IAuthenticationTicketDataFormat _format;
        
        private const string DefaultTokenName = "auth token";

        private const string DefaultCookieName = "auth cookie";

        public DefaultAuthenticationHandler(
            IOptionsMonitor<DefaultAuthenticationOptions> options, 
            ILoggerFactory logger, UrlEncoder encoder, 
            ISystemClock clock, 
            IDataProtectionProvider provider,
            IDataSerializer<AuthenticationTicket> serializer) : base(options, logger, encoder, clock)
        {
            var dataProtector = provider.CreateProtector(Startup.DefaultConfig.SecretKey);
            _format = new DefaultAuthenticationDataFormat(dataProtector, serializer);
        }

        protected override Task InitializeHandlerAsync()
        {
            if (Options == null)
            {
                throw new NullReferenceException(nameof(Options));
            }
            Options.DataProcetor = Options.DataProcetor ?? _format.DataProcetor;
            return Task.CompletedTask;
        }

        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (properties == null)
            {
                properties = new AuthenticationProperties()
                {
                    IsPersistent = true,
                    IssuedUtc = DateTimeOffset.UtcNow
                };
            }

            var token = new AuthenticationToken
            {
                Name = Options.TokenName ?? DefaultTokenName,
                Value = Guid.NewGuid().ToString()
            };

            var tokens = new List<AuthenticationToken> { token };
            properties.StoreTokens(tokens);
            properties.ExpiresUtc = properties.IssuedUtc.Value.Add(Options.ExpiredTimeSpan);

            Context.User = user;

            var ticket = new AuthenticationTicket(user, properties, Scheme.Name);
            var authCookie = _format.Protect(ticket, token.Value);

            AppendResponseSigInCookie(token, authCookie);
            return Task.CompletedTask;
        }

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            properties = properties ?? new AuthenticationProperties();
            properties.RedirectUri = Options.LoginPath;

            var cookies = Response.Cookies;
            var tokenName = Options.TokenName ?? DefaultTokenName;
            var cookieName = Options.CookieName ?? DefaultCookieName;
            cookies.Delete(tokenName);
            cookies.Delete(cookieName);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 身份验证处理主体
        /// </summary>
        /// <returns>身份验证结果 <see cref="AuthenticateResult"/></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var currentPath = Request.Path;
            Logger.LogDebug($"current path: {currentPath}");

            var isWhitelist = Options.Whitelist?.Exists(path => Regex.IsMatch(currentPath, path)) ?? false;
            if (isWhitelist || currentPath == Options.LoginPath)
            {
                return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(), new AuthenticationProperties(), Scheme.Name));
                // return AuthenticateResult.NoResult();
            }

            var protectedCookie = Request.Cookies[Options.CookieName ?? DefaultCookieName];
            var token = Request.Cookies[Options.TokenName ?? DefaultTokenName];
            try
            {
                var ticket = token == null ? _format.Unprotect(protectedCookie) : _format.Unprotect(protectedCookie, token);
                var properties = ticket.Properties;
                var principal = ticket.Principal;
                var userClaim = principal.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier && c.ValueType == ClaimValueTypes.String);    
                var roleClaim = principal.Claims.Single(c => c.Type == ClaimTypes.Role && c.ValueType == ClaimValueTypes.String);
                return await CreateAuthenticatedResultAsync(userClaim, roleClaim, properties);
            }
            catch (System.Exception e)
            {
                Logger.LogError(e.Message);
                return AuthenticateResult.Fail(e);
            }
        }

        private Task<AuthenticateResult> CreateAuthenticatedResultAsync(Claim userClaim, Claim roleClaim, AuthenticationProperties properties)
        {
            var claims = new List<Claim>{ userClaim, roleClaim };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            if (Options.SlidingExpiration)
            {
                properties.ExpiresUtc = DateTime.UtcNow.Add(Options.ExpiredTimeSpan);
            }
            var result = AuthenticateResult.Success(new AuthenticationTicket(principal, properties, Scheme.Name));
            return Task.FromResult(result);
        }

        private void AppendResponseSigInCookie(AuthenticationToken token, string cookieValue)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (cookieValue == null)
            {
                throw new ArgumentNullException(nameof(cookieValue));
            }

            Logger.LogDebug($"{Response.Cookies == Context.Response.Cookies}");

            var cookieName = Options.CookieName ?? DefaultCookieName;
            var cookies = Response.Cookies;
            cookies.Delete(token.Name);
            cookies.Delete(cookieName);
            cookies.Append(token.Name, token.Value);
            cookies.Append(cookieName, cookieValue);
        }
    }

    public interface IAuthenticationTicketDataFormat : ISecureDataFormat<AuthenticationTicket>
    {
        IDataProtector DataProcetor { get; }
    }

    public class DefaultAuthenticationDataFormat : IAuthenticationTicketDataFormat
    {
        private readonly IDataProtector _protector;

        private readonly IDataSerializer<AuthenticationTicket> _serializer;

        public DefaultAuthenticationDataFormat(IDataProtector protector, IDataSerializer<AuthenticationTicket> serializer)
        {
            _protector = protector;
            _serializer = serializer;
        }

        public IDataProtector DataProcetor => _protector ?? throw new NullReferenceException(nameof(_protector));

        /// <summary>
        /// 对认证票据进行base64编码，没有做任何加密处理
        /// </summary>
        /// <param name="data">认证票据 <see cref="AuthenticationTicket"/></param>
        /// <returns>编码后的字符串</returns>
        public string Protect(AuthenticationTicket data)
        {
            var serializerData = _serializer.Serialize(data ?? throw new ArgumentNullException(nameof(data)));
            return Convert.ToBase64String(serializerData);
        }

        /// <summary>
        /// 对认证票据进行加密后再进行base64编码
        /// </summary>
        /// <param name="data">认证票据 <see cref="AuthenticationTicket"/></param>
        /// <param name="purpose">供加密程序(<see cref="IDataProtector"/>)用的目标字符串</param>
        /// <returns>加密和编码后的字符串</returns>
        public string Protect(AuthenticationTicket data, string purpose)
        {
            var serializerData = _serializer.Serialize(data ?? throw new ArgumentNullException(nameof(data)));
            var protectedText = Protector(purpose).Protect(serializerData);
            return Convert.ToBase64String(protectedText);
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            var serializerData = Convert.FromBase64String(protectedText ?? throw new ArgumentNullException(nameof(protectedText)));
            return _serializer.Deserialize(serializerData);
        }

        public AuthenticationTicket Unprotect(string protectedText, string purpose)
        {
            var protectedBytes = Convert.FromBase64String(protectedText ?? throw new ArgumentNullException(nameof(protectedText)));
            var serializerData = Protector(purpose).Unprotect(protectedBytes);
            return _serializer.Deserialize(serializerData);
        }

        private IDataProtector Protector(string purpose)
        {
            var protector = _protector;
            if (!string.IsNullOrEmpty(purpose))
            {
                protector = _protector.CreateProtector(purpose);
            }
            return protector;
        }
    }
}