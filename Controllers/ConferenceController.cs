using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactDemo.Application.Dtos;
using ReactDemo.Application.Services;
using ReactDemo.Presentation.ViewObject;

namespace ReactDemo.Controllers
{
    public class ConferenceController : Controller
    {

        private readonly IConferenceAppService _conferenceAppService;
        private readonly ILogger<ConferenceController> _logger;


        public ConferenceController(
            IConferenceAppService conferenceAppService, 
            ILogger<ConferenceController> logger)
        {
            this._conferenceAppService = conferenceAppService;
            this._logger = logger;
        }

        [HttpGet("[action]")]
        public IList<ConferenceItemVo> Conferences([FromQuery]Page page)
        {
            _logger.LogDebug("list conferences");
            var conferences = _conferenceAppService.GetListByPage(page);
            var conferenceItemVos = new List<ConferenceItemVo>(conferences.Count);
            foreach (var conference in conferences)
            {
                conferenceItemVos.Add(new ConferenceItemVo(conference));
            }
            return  conferenceItemVos;
        }

        [HttpPost("[action]")]
        public ActionResult CreateConference([FromForm]ConferenceDto dto)
        {
            var memberID = TempData["memberId"] as int?;
            if (ModelState.IsValid && memberID != null)
            {
                _conferenceAppService.CreateConference(dto, memberID);
                _logger.LogDebug("add conference sucessfully");
                return Ok();
            }
            _logger.LogError("add conference failed");
            return BadRequest(ModelState);
        }
    }
}