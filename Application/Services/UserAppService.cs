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
        private readonly IMemberRepository _memberRepository;
        private readonly HttpContext _httpContext;
        private readonly ILogger _logger;

        public UserAppService(
            IUserRepository userRepository, 
            IMemberRepository memberRepository, 
            IHttpContextAccessor httpContextAccessor, 
            ILoggerFactory loggerFactory)
        {
            _userRepository = userRepository;
            _memberRepository = memberRepository;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = loggerFactory.CreateLogger<UserAppService>();
        }

        async Task<bool> IUserAppService.UserSignInAsync(UserDto userDto)
        {
            var user = _userRepository.FindOne(u => u.Username == userDto.Username);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("username", user.Username, ClaimValueTypes.String),    
                };
                if (user.MemberID != null)
                {
                    var member = user.Member;
                    claims.Add(new Claim("role", member.Role.Name, ClaimValueTypes.String));
                }
                await _httpContext.SignInAsync(Startup.SchemeName, new ClaimsPrincipal(new ClaimsIdentity(claims, "PartyAuth", "name", "role")));
                var cookieValue = _httpContext.Request.Cookies[Startup.CookieName];
                _logger.LogDebug(cookieValue);
                return true;
            }
            return false;
        }
    }
}