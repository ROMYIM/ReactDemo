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

        public ErrorController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType());
        }

        [Route("")]
        [Route("/index")]
        [AllowAnonymous]
        public ActionResult<ReturnData> Index()
        {
            var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var pathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var error = errorFeature?.Error;
            var message = error?.Message;
            _logger.LogError(error, "request path ---> {0}   message ---> {1}", pathFeature?.Path, message);

            var businessError = error as BusinessException;
            var returnData = new ReturnData
            {
                Code = (int) (businessError?.Code.Value ?? ErrorCode.SystemError),
                Message = message
            };
            return BadRequest(returnData);
        }


    }
}