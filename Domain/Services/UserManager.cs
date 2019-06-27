using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ReactDemo.Domain.Models.System;

namespace ReactDemo.Domain.Services
{
    public interface IUserManager : IDomainService
    {
        Task SignInAsync(User user, Role userRole);
    }

    public class UserManager : IUserManager
    {
        private readonly ILogger _logger;

        private readonly HttpContext _httpContext;

        public UserManager(ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
            _httpContext = httpContextAccessor.HttpContext;
        }

        async Task IUserManager.SignInAsync(User user, Role role)
        {
            var userIdentity = user.CreateIdentity();
            var roleIdentity = role.CreateIdentity();
            var principal = new ClaimsPrincipal(new List<ClaimsIdentity> { userIdentity, roleIdentity });
            await _httpContext.SignInAsync(Startup.JwtConfig.SchemeName, principal);
        }
    }
}