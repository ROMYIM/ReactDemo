using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.System;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserAppService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        async Task<bool> IUserAppService.UserSignInAsync(UserDto userDto)
        {
           var user = new User
           {
                Username = userDto.Username,
                Password = userDto.Password
           };
           var result = await _userManager.CreateAsync(user, user.Password);
           if (result.Succeeded)
           {
               await _signInManager.SignInAsync(user, isPersistent: true);
               return true;
           }
           return false;
        }
    }
}