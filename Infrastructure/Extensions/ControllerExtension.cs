using Microsoft.AspNetCore.Mvc;
using ReactDemo.Presentation.ViewObject;

namespace ReactDemo.Infrastructure.Extensions
{
    public static class ControllerExtension
    {
        public static ActionResult<ReturnData> Ok(this ControllerBase controller, object data = null, string message = "SUCCESS")
        {
            var returnData = new ReturnData
            {
                Code = 0,
                Data = data,
                Message = message
            };
            return controller.Ok(returnData);
        }
    }
}