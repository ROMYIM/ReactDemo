using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.System;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;

        private readonly HttpContext _httpContext;

        private readonly ILogger _logger;

        public UserAppService(
            IUserRepository userRepository, 
            IHttpContextAccessor httpContextAccessor, 
            ILoggerFactory loggerFactory)
        {
            _userRepository = userRepository;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = loggerFactory.CreateLogger<UserAppService>();
        }

        async Task<bool> IUserAppService.UserSignInAsync(UserDto userDto)
        {
            var user = await _userRepository.FindOneAsync(u => u.Username == userDto.Username);
            if (user != null)
            {
                await _httpContext.SignInAsync(Startup.SchemeName, user.CreateClaimsPrincipal(), user.CreateAuthenticationProperties());
                var cookieValue = _httpContext.Request.Cookies[Startup.CookieName];
                _logger.LogDebug(cookieValue);
                return true;
            }
            return false;
        }

        async Task IUserAppService.UserSignOutAsync()
        {
            var principalUser = _httpContext.User;
            var username = principalUser?.FindFirstValue("username") ?? null;
            if (username != null)
            {
                _logger.LogDebug("get the username");
                var user = await _userRepository.FindOneAsync(u => u.Username == username);
                var properties = user?.CreateAuthenticationProperties();
                await _httpContext.SignOutAsync(properties);
            }
        }
    }
}