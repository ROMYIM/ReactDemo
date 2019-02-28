using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactDemo.Application.Dtos;
using ReactDemo.Infrastructure.Utils;

namespace ReactDemo.Controllers
{
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ImageUtil _imageUtil;
        private readonly ILogger<UserController> _logger;

        public UserController(ImageUtil imageUtil, ILoggerFactory loggerFactory)
        {
            _imageUtil = imageUtil;
            _logger = loggerFactory.CreateLogger<UserController>();
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

        public Task<IActionResult> Login([FromBody, Bind("username, password, verifycode")]UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                string verifyCode = HttpContext.Session.GetString("verifyCode").ToLower();

            }
        }
    }
}