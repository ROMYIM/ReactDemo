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

        public PathString LoginPath { get; set; }

        public PathString LogoutPath { get; set; }

        public List<PathString> Whitelist { get; set; }

    }

    public class DefaultAuthenticationHandler : AuthenticationHandler<DefaultAuthenticationOptions>, IAuthenticationSignInHandler, IAuthenticationSignOutHandler
    {
        private readonly ISecureDataFormat<AuthenticationTicket> _format;

        public DefaultAuthenticationHandler(IOptionsMonitor<DefaultAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ISecureDataFormat<AuthenticationTicket> format) : base(options, logger, encoder, clock)
        {
            _format = format;
        }

        public async Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            var token = new AuthenticationToken
            {
                Name = Startup.KeyName,
                Value = Guid.NewGuid().ToString()
            };
        }

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

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
            var key = Request.Cookies[Startup.KeyName];

            try
            {
                var ticket = _format.Unprotect(protectedCookie, key);
                var principal = ticket.Principal;
                var userClaim = principal.Claims.Single(c => c.Type == "username" && c.ValueType == ClaimValueTypes.String);    
                var roleClaim = principal.Claims.Single(c => c.Type == "role" && c.ValueType == ClaimValueTypes.String);
                await Task.CompletedTask;
                return CreateAuthenticatedResult(userClaim, roleClaim, ticket);
            }
            catch (System.Exception e)
            {
                Logger.LogError(e.Message);
                return AuthenticateResult.Fail(e);
            }
        }

        private AuthenticateResult CreateAuthenticatedResult(Claim userClaim, Claim roleClaim, AuthenticationTicket ticket)
        {
            var claims = new List<Claim>{ userClaim, roleClaim };
            var identity = new ClaimsIdentity(claims, ticket.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            return AuthenticateResult.Success(new AuthenticationTicket(principal, ticket.Properties, ticket.AuthenticationScheme));
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

        public string Protect(AuthenticationTicket data)
        {
            return this.Protect(data, purpose: null);
        }

        public string Protect(AuthenticationTicket data, string purpose)
        {
            var serializerData = _serializer.Serialize(data ?? throw new ArgumentNullException(nameof(data)));
            var protectedText = Protector(purpose).Protect(serializerData);
            return Convert.ToBase64String(protectedText);
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            return this.Unprotect(protectedText, purpose: null);
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