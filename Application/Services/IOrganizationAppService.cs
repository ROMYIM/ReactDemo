using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.Party;

namespace ReactDemo.Application.Services
{
    public interface IOrganizationAppService : IApplicationService<Organization>
    {
        void CreateOrganization(OrganizationDto dto);

        void EditOrganization(OrganizationDto dto);
    }
}