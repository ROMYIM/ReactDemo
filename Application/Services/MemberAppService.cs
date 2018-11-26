using System.Collections.Generic;
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

        IList<Member> IApplicationService<Member>.GetListByPage(Page page)
        {
            var member = _memberRepository.FindMember();
            return _memberRepository.FindList(m => m.OrganizationID == member.OrganizationID, page);
        }
    }
}