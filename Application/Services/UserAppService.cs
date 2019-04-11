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
                await _httpContext.SignInAsync(Startup.SchemeName, user.CreateClaimsPrincipal());
                var cookieValue = _httpContext.Request.Cookies[Startup.CookieName];
                _logger.LogDebug(cookieValue);
                return true;
            }
            
            return false;
        }
    }
}