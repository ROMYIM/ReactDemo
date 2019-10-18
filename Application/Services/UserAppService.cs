using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Repositories;
using ReactDemo.Domain.Services;
using ReactDemo.Infrastructure.Transaction.Attributes;

namespace ReactDemo.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;

        private readonly UserManager _userManager;

        private readonly HttpContext _httpContext;

        private readonly ILogger _logger;

        public UserAppService(
            IUserRepository userRepository, 
            UserManager userManager,
            IHttpContextAccessor httpContextAccessor, 
            ILoggerFactory loggerFactory)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = loggerFactory.CreateLogger(this.GetType());
        }

        [UnitOfWork]
        async Task<bool> IUserAppService.UserSignInAsync(UserDto userDto)
        {
            var user = await _userRepository.DbSet.AsNoTracking()
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .SingleAsync(u => u.Username == userDto.Username);

            if (user?.VerifyPassword(userDto.Password) ?? false)
            {
                await _userManager.SignInAsync(user);
                return true;
            }
            return false;
        }

        async Task IUserAppService.UserSignOutAsync()
        {
            var userCache = _httpContext.User;
            int id = 0;
            
            if (int.TryParse(userCache?.FindFirstValue("user_id") ?? null, out id))
            {
                _logger.LogDebug("get the username");
                var user = await _userRepository.FindOneAsync(u => u.ID == id);
                // var properties = user?.CreateAuthenticationProperties();
                await _httpContext.SignOutAsync(Startup.JwtConfig.SchemeName);
            }
        }
    }
}