using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactDemo.Application.Dtos;
using ReactDemo.Application.Services;
using ReactDemo.Infrastructure.Security.Authorization;
using ReactDemo.Infrastructure.Utils;

namespace ReactDemo.Controllers
{
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ImageUtil _imageUtil;
        private readonly ILogger _logger;
        private readonly IUserAppService _userAppService;
        private readonly string _id;

        public UserController(ImageUtil imageUtil, ILoggerFactory loggerFactory, IUserAppService userAppService)
        {
            _imageUtil = imageUtil;
            _logger = loggerFactory.CreateLogger(this.GetType());
            _userAppService = userAppService;
            _id = Guid.NewGuid().ToString();
        }

        [HttpGet("verifycode")]
        [AllowAnonymous]
        public IActionResult CreateVerifyCode([FromQuery(Name = "seed")]long seed)
        {
            _logger.LogDebug($"controller id is {_id}");
            var guid = Guid.NewGuid().ToString();
            string pattern = "[A-Za-z0-9]";
            var result = Regex.Matches(guid, pattern);
            var random = new Random((int) seed);
            var verifyCodeBuilder = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                verifyCodeBuilder.Append(result[random.Next(result.Count)].ToString());
            }
            var verifyCode = verifyCodeBuilder.ToString();
            _logger.LogDebug(verifyCode);
            HttpContext.Session.SetString("verifyCode", verifyCode);
            var memoryStream = _imageUtil.CreateVerifyCodePicture(verifyCode, random);
            Response.Body.Dispose();
            return File(memoryStream.ToArray(), @"image/png");
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm]UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                string verifyCode = HttpContext.Session.GetString("verifyCode");
                if (userDto.ValidateVerifyCode(verifyCode?.ToLower()))
                {
                    var result = await _userAppService.UserSignInAsync(userDto);
                    if (result)
                        return Ok("登录成功");
                    else
                        return BadRequest("登录失败");
                }
                else
                {
                    return BadRequest("验证码有误");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _userAppService.UserSignOutAsync();
            return Ok("登出成功");
        }
    }
}