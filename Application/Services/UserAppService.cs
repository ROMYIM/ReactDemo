using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AspectCore.Injector;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.User;
using ReactDemo.Domain.Repositories;
using ReactDemo.Domain.Services;
using ReactDemo.Infrastructure.Event.Buses;
using ReactDemo.Infrastructure.Event.Entities;
using ReactDemo.Infrastructure.Transaction.Attributes;

namespace ReactDemo.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;

        private readonly UserManager _userManager;

        private readonly HttpContext _httpContext;

        private readonly ILogger _logger;

	    [FromContainer]
	    public IEventBus EventBus { get; set; }

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

		        user.ImageUrl = "image_url_1";
                user.AddEvent(new EntityUpdateEvent<User>(user));
                _userRepository.Update(user);
		        
                return true;
            }
            return false;
        }

        async Task IUserAppService.UserSignOutAsync()
        {
            var userCache = _httpContext.User;

	        if (int.TryParse(userCache?.FindFirstValue("user_id") ?? null, out int id))
	        {
		        _logger.LogDebug("get the username");
		        var user = await _userRepository.FindOneAsync(u => u.ID == id);
		        // var properties = user?.CreateAuthenticationProperties();
		        await _httpContext.SignOutAsync(Startup.JwtConfig.SchemeName);
	        }
	    }

    }

    
}