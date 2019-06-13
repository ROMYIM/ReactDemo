using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Repositories;
using ReactDemo.Domain.Services;

namespace ReactDemo.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;

        private readonly IRoleRepository _roleRepository;

        private readonly IUserManager _userManager;

        private readonly HttpContext _httpContext;

        private readonly ILogger _logger;

        public UserAppService(
            IUserRepository userRepository, 
            IRoleRepository roleRepository,
            IUserManager userManager,
            IHttpContextAccessor httpContextAccessor, 
            ILoggerFactory loggerFactory)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = loggerFactory.CreateLogger(this.GetType());
        }

        async Task<bool> IUserAppService.UserSignInAsync(UserDto userDto)
        {
            var user = await _userRepository.FindOneAsync(u => u.Username == userDto.Username);
            if (user != null)
            {
                if (user.verifyPassword(userDto.Password))
                {
                    var role = await _roleRepository.FindOneAsync(r => r.ID == user.RoleID);
                    await _userManager.SignInAsync(user, role);
                    var cookieValue = _httpContext.Request.Cookies[Startup.CookieName];
                    _logger.LogDebug(cookieValue);
                    return true;
                }
            }
            return false;
        }

        async Task IUserAppService.UserSignOutAsync()
        {
            var principalUser = _httpContext.User;
            int id = 0;
            
            if (int.TryParse(principalUser?.FindFirstValue(ClaimTypes.NameIdentifier) ?? null, out id))
            {
                _logger.LogDebug("get the username");
                var user = await _userRepository.FindOneAsync(u => u.ID == id);
                // var properties = user?.CreateAuthenticationProperties();
                await _httpContext.SignOutAsync();
            }
        }
    }
}