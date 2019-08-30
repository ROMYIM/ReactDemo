using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactDemo.Application.Dtos;
using ReactDemo.Application.Services;
using ReactDemo.Infrastructure.Error;
using ReactDemo.Infrastructure.Extensions;
using ReactDemo.Infrastructure.Utils;
using ReactDemo.Presentation.ViewObject;

namespace ReactDemo.Presentation.Controllers
{
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private readonly ImageUtil _imageUtil;
        private readonly IUserAppService _userAppService;
        private readonly string _id;

        public UserController(ImageUtil imageUtil, ILoggerFactory loggerFactory, IUserAppService userAppService) : base(loggerFactory)
        {
            _imageUtil = imageUtil;
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
        public async Task<ActionResult<ReturnData>> Login([FromForm]UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                string verifyCode = HttpContext.Session.GetString("verifyCode");
                _logger.LogInformation($"验证码：{verifyCode}");

                List<string> keyList = HttpContext.Session.Keys.ToList();
                _logger.LogInformation($"id数量：{keyList.Count}");
                keyList.ForEach(key => _logger.LogInformation(key));

                if (userDto.ValidateVerifyCode(verifyCode?.ToLower()))
                {
                    var result = await _userAppService.UserSignInAsync(userDto);
                    if (result)
                        return Ok("登录成功");
                    else
                        throw new BusinessException(ErrorCode.LoginFail);
                }
                else
                    throw new BusinessException(ErrorCode.VerifyCodeError);
            }
            else
                throw new Exception("参数有误");
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<ActionResult<ReturnData>> Logout()
        {
            await _userAppService.UserSignOutAsync();
            return Ok("登出成功");
        }
    }
}