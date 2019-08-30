using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactDemo.Presentation.ViewObject;

namespace ReactDemo.Presentation.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly ILogger _logger;

        public BaseController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public ActionResult<ReturnData> Ok(object data, int code = 0, string message = "SUCCESS")
        {
            var returnData = new ReturnData
            {
                Code = code,
                Data = data,
                Message = message
            };
            return base.Ok(returnData);
        }

        public ActionResult<ReturnData> Ok(string message = "SUCCESS") => Ok(null, 0, message);
        
    }
}