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
            throw new System.NotImplementedException();
        }

        Organization IApplicationService<Organization, OrganizationDto>.CreateEntity(OrganizationDto dto)
        {
            var organization = new Organization(dto);
            return organization;
        }
    }
}