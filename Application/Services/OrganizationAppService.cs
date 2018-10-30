using System.Collections.Generic;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.Party;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Application.Services
{
    public class OrganizationAppService : IOrganizationAppService
    {

        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationAppService(IOrganizationRepository organizationRepository)
        {
            this._organizationRepository = organizationRepository;
        }

        public IList<Organization> GetListByPage(Page page)
        {
            return _organizationRepository.FindList(o => 1 == 1, page);
        }
    }
}