using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactDemo.Infrastructure.Error;
using ReactDemo.Infrastructure.Security.Authorization;
using ReactDemo.Presentation.ViewObject;

namespace ReactDemo.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : BaseController
    {

        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public SampleDataController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        [HttpGet("[action]")]
        [DefaultRequirement(resource: "test", operation: ResourceOperation.Query)]
        public ActionResult<ReturnData> Test()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok("测试成功");
            }
            throw new BusinessException(ErrorCode.AuthenticationFail);
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
    }
}
