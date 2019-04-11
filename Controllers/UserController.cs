using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactDemo.Application.Dtos;
using ReactDemo.Application.Services;
using ReactDemo.Infrastructure.Utils;

namespace ReactDemo.Controllers
{
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ImageUtil _imageUtil;
        private readonly ILogger<UserController> _logger;
        private readonly IUserAppService _userAppService;

        public UserController(ImageUtil imageUtil, ILoggerFactory loggerFactory, IUserAppService userAppService)
        {
            _imageUtil = imageUtil;
            _logger = loggerFactory.CreateLogger<UserController>();
            _userAppService = userAppService;
        }

        [HttpGet("verifycode")]
        public IActionResult CreateVerifyCode([FromQuery(Name = "seed")]long seed)
        {
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
        public async Task<IActionResult> Login([FromForm]UserDto userDto)
        {
            var cookieValue = HttpContext.Request.Cookies[Startup.CookieName];
            _logger.LogDebug($"cookie value:----------  {cookieValue}");
            if (ModelState.IsValid)
            {
                string verifyCode = HttpContext.Session.GetString("verifyCode");
                if (verifyCode != null && userDto.ValidateVerifyCode(verifyCode.ToLower()))
                {
                    var result = await _userAppService.UserSignInAsync(userDto);
                    if (result)
                        return Ok("登录成功");
                    else
                        return BadRequest("登录失败");
                }
                else
                {
                    var User = HttpContext.User;
                    if (!User.Identity.IsAuthenticated)
                    {
                        _logger.LogDebug("验证不通过");
                    }

                    return BadRequest("验证码有误");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}