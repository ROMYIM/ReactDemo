using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ReactDemo.Domain.Models.User;

namespace ReactDemo.Domain.Services
{

    public class UserManager : IDomainService
    {
        private readonly ILogger _logger;

        private readonly HttpContext _httpContext;

        public UserManager(ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task SignInAsync(User user)
        {
            var userIdentity = user.CreateIdentity();
            var principal = new ClaimsPrincipal(new List<ClaimsIdentity> { userIdentity });
            await _httpContext.SignInAsync(Startup.JwtConfig.SchemeName, principal);
        }
    }
}