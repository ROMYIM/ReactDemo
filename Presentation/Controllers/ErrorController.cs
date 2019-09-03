using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactDemo.Infrastructure.Error;
using ReactDemo.Presentation.ViewObject;

namespace ReactDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger _logger;

        private readonly Func<string, ReturnData> _errorHandle; 

        public ErrorController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _errorHandle = message => 
            {
                var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
                var pathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

                var error = errorFeature?.Error;
                message = error?.Message ?? message;
                _logger.LogError(error, "request path ---> {0}   message ---> {1}", pathFeature?.Path, message);

                var businessError = error as BusinessException;
                return new ReturnData
                {
                    Code = (int) (businessError?.Code.Value ?? ErrorCode.SystemError),
                    Message = message
                };
            };
        }

        [Route("")]
        [Route("/index")]
        [AllowAnonymous]
        public ActionResult<ReturnData> Index()
        {
            return BadRequest(_errorHandle("系统异常"));
        }

        [Route("/404")]
        [AllowAnonymous]
        public new ActionResult<ReturnData> NotFound()
        {
            return NotFound(_errorHandle("页面没找到"));
        }

        [Route("/{statusCode:regex(^(401|403)$)}")]
        public ReturnData UnauthorizedForbidden(int statusCode)
        {
            HttpContext.Response.StatusCode = statusCode;
            return _errorHandle("权限不足");
        }

    }
}