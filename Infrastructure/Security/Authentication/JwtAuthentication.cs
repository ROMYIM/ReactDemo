using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ReactDemo.Infrastructure.Security.Authentication
{

    public class JwtAuthenticationHandler : JwtBearerHandler, IAuthenticationSignInHandler
    {

        private readonly JwtSecurityTokenHandler _tokenHandler;

        public JwtAuthenticationHandler(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder, IDataProtectionProvider dataProtection, ISystemClock clock) : base(options, logger, encoder, dataProtection, clock)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
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
                    IssuedUtc = DateTimeOffset.UtcNow,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };
            }

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                IssuedAt = properties.IssuedUtc.Value.UtcDateTime,
                Expires = properties.ExpiresUtc.Value.UtcDateTime,
                Issuer = Options.ClaimsIssuer,
                Audience = Options.Audience,
                Subject = new ClaimsIdentity(user.Claims),
                SigningCredentials = new SigningCredentials(Options.TokenValidationParameters.IssuerSigningKey, SecurityAlgorithms.HmacSha256)
            };

            var token = _tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var tokeText = _tokenHandler.WriteToken(token);
            // Response.Headers["Authorization"] = "Bearer " + tokeText;

            Context.SignInAsync(Startup.DefaultConfig.SchemeName, user);

            return Task.CompletedTask;
        }

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            Logger.LogDebug("jwt logout");
            Response.Headers.Remove("Authorization");
            return Task.CompletedTask;
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            // Response.
            Logger.LogDebug("jwt authentication changellenge");
            Logger.LogDebug($"authorizationHeader:{Request.Headers["Authorization"]}");
            
            var result = await Context.AuthenticateAsync(Startup.DefaultConfig.SchemeName);
            if (result.Succeeded)
                await Task.CompletedTask;
            
        }
    }
}