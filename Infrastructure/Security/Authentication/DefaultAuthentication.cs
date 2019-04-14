using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace ReactDemo.Infrastructure.Security.Authentication
{
    public class DefaultAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string ProtectorPurpose { get; set; }

        public PathString LoginPath { get; set; } = "User/Login";

        public PathString LogoutPath { get; set; } = "User/Logout";

        public List<PathString> Whitelist { get; set; }

        public TimeSpan ExpiredTimeSpan { get; set; } = TimeSpan.FromHours(1);

        public bool SlidingExpiration { get; set; } = true;

    }

    public class DefaultAuthenticationHandler : AuthenticationHandler<DefaultAuthenticationOptions>, IAuthenticationSignInHandler, IAuthenticationSignOutHandler
    {
        private readonly ISecureDataFormat<AuthenticationTicket> _format;
        
        private const string TokenName = "default token";

        public DefaultAuthenticationHandler(IOptionsMonitor<DefaultAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ISecureDataFormat<AuthenticationTicket> format) : base(options, logger, encoder, clock)
        {
            _format = format;
        }

        public async Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            var token = new AuthenticationToken
            {
                Name = TokenName,
                Value = Guid.NewGuid().ToString()
            };

            properties = properties ?? new AuthenticationProperties();
            var tokens = new List<AuthenticationToken> { token };
            properties.StoreTokens(tokens);

            properties.IssuedUtc = DateTimeOffset.UtcNow;
            properties.ExpiresUtc = new DateTimeOffset(properties.IssuedUtc.Value.Ticks, Options.ExpiredTimeSpan);

            var ticket = new AuthenticationTicket(user, properties, Scheme.Name);
        }

        public async Task SignOutAsync(AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 身份验证处理主体
        /// </summary>
        /// <returns>身份验证结果 <see cref="AuthenticateResult"/></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var currentPath = Request.Path;
            Logger.LogDebug($"current path: {currentPath}");

            var isWhitelist = Options.Whitelist?.Exists(path => path == currentPath) ?? false;
            if (isWhitelist)
            {
                return AuthenticateResult.Success(null);
            }

            var protectedCookie = Request.Cookies[Startup.CookieName];

            try
            {
                var ticket = _format.Unprotect(protectedCookie);
                var properties = ticket.Properties;
                var principal = ticket.Principal;
                var userClaim = principal.Claims.Single(c => c.Type == "username" && c.ValueType == ClaimValueTypes.String);    
                var roleClaim = principal.Claims.Single(c => c.Type == "role" && c.ValueType == ClaimValueTypes.String);
                return await CreateAuthenticatedResultAsync(userClaim, roleClaim, properties);
            }
            catch (System.Exception e)
            {
                Logger.LogError(e.Message);
                return AuthenticateResult.Fail(e);
            }
        }

        private async Task<AuthenticateResult> CreateAuthenticatedResultAsync(Claim userClaim, Claim roleClaim, AuthenticationProperties properties)
        {
            var claims = new List<Claim>{ userClaim, roleClaim };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            if (Options.SlidingExpiration)
            {
                properties.ExpiresUtc = new DateTimeOffset(DateTime.UtcNow, Options.ExpiredTimeSpan);
            }
            var result = AuthenticateResult.Success(new AuthenticationTicket(principal, properties, Scheme.Name));
            await Task.CompletedTask;
            return result;
        }
    }

    public class DefaultAuthenticationDataFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly IDataProtector _protector;

        private readonly IDataSerializer<AuthenticationTicket> _serializer;

        public DefaultAuthenticationDataFormat(IDataProtectionProvider provider, IDataSerializer<AuthenticationTicket> serializer)
        {
            _protector = provider.CreateProtector(Startup.SchemeName);
            _serializer = serializer;
        }

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