using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactDemo.Application.Dtos;
using ReactDemo.Application.Services;
using ReactDemo.Presentation.ViewObject;

namespace ReactDemo.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {

        private readonly IConferenceAppService _conferenceAppService;
        private readonly ILogger<SampleDataController> _logger;

        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public SampleDataController(
            IConferenceAppService conferenceAppService, 
            ILogger<SampleDataController> logger)
        {
            this._conferenceAppService = conferenceAppService;
            this._logger = logger;
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
        public IEnumerable<ConferenceItemVo> Conferences([FromQuery]Page page)
        {
            _logger.LogDebug("list conferences");
            var conferences = _conferenceAppService.GetListByPage(page);
            var conferenceItemVos = new List<ConferenceVo>(conferences.Count);
            foreach (var conference in conferences)
            {
                conferenceItemVos.Add(new ConferenceVo(conference));
            }
            return (IEnumerable<ConferenceItemVo>) conferenceItemVos;
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
