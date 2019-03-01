using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.System;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly HttpContext _httpContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserAppService(IUserRepository userRepository, IMemberRepository memberRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _memberRepository = memberRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        async Task<bool> IUserAppService.UserSignInAsync(UserDto userDto)
        {
            var user = await _userManager.FindByNameAsync(userName: userDto.Username);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username, ClaimValueTypes.String),    
                };
                if (user.MemberID != null)
                {
                    var member = _memberRepository.FindOne(m => m.ID == user.MemberID);
                    claims.Add(new Claim(ClaimTypes.Role, member.Role.Name, ClaimValueTypes.String));
                }
                await _signInManager.SignInAsync(user, isPersistent: true);
                await _httpContext.SignInAsync(Startup.SchemeName, new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookie", ClaimTypes.Name, ClaimTypes.Role)));
                return true;
            }
            return false;
        }
    }
}