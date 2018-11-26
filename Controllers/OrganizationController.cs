using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactDemo.Application.Services;

namespace ReactDemo.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly IOrganizationAppService _organizationService;
        private readonly ILogger<OrganizationController> _logger;

        public OrganizationController(
            IOrganizationAppService organizationService, 
            ILogger<OrganizationController> logger)
        {
            _organizationService = organizationService;
            _logger = logger;
        }
    }
}