using System;
using System.Linq;
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
using ReactDemo.Infrastructure.Config.Cache;
using ReactDemo.Infrastructure.Extensions;
using System.Collections.Generic;

namespace ReactDemo.Infrastructure.Security.Authentication
{

    public class JwtAuthenticationHandler : JwtBearerHandler, IAuthenticationSignInHandler
    {

        private readonly JwtSecurityTokenHandler _tokenHandler;

        private readonly RedisOptions _redisOptions;

        public JwtAuthenticationHandler(IOptionsMonitor<JwtBearerOptions> options, IOptionsMonitor<RedisOptions> redisOptions, ILoggerFactory logger, UrlEncoder encoder, IDataProtectionProvider dataProtection, ISystemClock clock) : base(options, logger, encoder, dataProtection, clock)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            _redisOptions = redisOptions.CurrentValue;
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
            Response.Headers["Authorization"] = "Bearer " + tokeText;

            Context.SignInAsync(Startup.DefaultConfig.SchemeName, user);

            return Task.CompletedTask;
        }

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            Logger.LogDebug("jwt logout");
            Response.Headers.Remove("Authorization");
            return Task.CompletedTask;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Logger.LogInformation(Request.Path);
            Logger.LogInformation(Request.Headers["Authorization"]);
            var result = await base.HandleAuthenticateAsync();
            if (result.Succeeded)
            {
                var idClaim = result.Principal.Claims.Single(c => c.Type == "user_id");
                if (idClaim == null)
                {
                    Logger.LogError("找不到用户ID");
                    return AuthenticateResult.NoResult();
                }

                /// <summary>
                /// 从session中（项目配置的是redis缓存）获取用户的缓存数据
                /// </summary>
                /// <value>用户在缓存中的数据</value>
                var cacheKey = $"{_redisOptions.Name}_{idClaim.Type}_{idClaim.Value}";
                var userInfo = Context.Session.Get<Dictionary<string, string>>(cacheKey);

                /// <summary>
                /// 构建用户身份信息。分别是用户id和用户名。
                /// 只做演示
                /// </summary>
                /// <returns>用户身份信息</returns>
                var userIdentity = new ClaimsIdentity();
                userIdentity.AddClaims(new List<Claim>
                {
                    new Claim("user_id", userInfo["user_id"]),
                    new Claim("username", userInfo["username"])
                });

                /// <summary>
                /// 构建角色身份信息。只做演示
                /// </summary>
                /// <returns>用户的角色身份信息</returns>
                var roleIdentity = new ClaimsIdentity();
                roleIdentity.AddClaims(new List<Claim>
                {
                    new Claim("role_id", userInfo["role_id"]),
                    new Claim("role_name", userInfo["role_name"])
                });

                /// <summary>
                /// 把缓存信息加入到<code>AuthenticationResult.Principal</code>
                /// 注意：身份认证中间件会在最后把<code>AuthenticationResult.Principal</code>加入到<code>HttpContext.User</code>中。
                /// 因此确保需要的缓存信息等其他用户数据添加到<code>AuthenticationResult.Principal</code>
                /// </summary>
                /// <returns></returns>
                var user = result.Principal ?? new ClaimsPrincipal();
                user.AddIdentities(new List<ClaimsIdentity>
                {
                    userIdentity,
                    roleIdentity
                });

                /// <summary>
                /// 如果<code>AuthenticationResult.Principal == null</code>，则直接把缓存信息存入到<code>HttpContext.User</code>中。
                /// 注意：如果<code>AuthenticationResult.Principal != null</code>，则一定要存入<code>AuthenticationResult</code>中。
                /// </summary>
                Context.User = user;

                Context.Response.Headers["Authorization"] = "Bearer " + result.Properties.GetTokenValue("access_token");
 
            }
            else
                result = await Context.AuthenticateAsync(Startup.DefaultConfig.SchemeName);

            return result;
        }
    }
}