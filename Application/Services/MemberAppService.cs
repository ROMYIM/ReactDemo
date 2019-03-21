using System.Collections.Generic;
using System.Threading.Tasks;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.Party;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Application.Services
{
    public class MemberAppService : IMemberAppService
    {

        private readonly IMemberRepository _memberRepository;
        private readonly IOrganizationRepository _organizationRepository;

        public MemberAppService(IMemberRepository memberRepository, IOrganizationRepository organizationRepository)
        {
            this._memberRepository = memberRepository;
            this._organizationRepository = organizationRepository;
        }

        async Task<List<Member>> IApplicationService<Member>.GetListByPageAsync(Page page)
        {
            var member = _memberRepository.FindMember();
            return await _memberRepository.FindListAsync(m => m.OrganizationID == member.OrganizationID, page);
        }
    }
}