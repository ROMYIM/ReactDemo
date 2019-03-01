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
        public async Task<ActionResult<string>> Login([FromBody, Bind("username, password, verifycode")]UserDto userDto)
        {
            string resultString = null;
            if (ModelState.IsValid)
            {
                string verifyCode = HttpContext.Session.GetString("verifyCode").ToLower();
                if (userDto.ValidateVerifyCode(verifyCode))
                {
                    var result = await _userAppService.UserSignInAsync(userDto);
                    if (result)
                        resultString = "登录成功";
                    else
                        resultString = "登录失败";
                }
                resultString = "验证码有误";
                return resultString;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}