using System.Collections.Generic;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.Party;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Application.Services
{
    public class MemberService : IMemberAppService
    {

        private readonly IMemberRepository _memberRepository;
        private readonly IOrganizationRepository _organizationRepository;

        public MemberService(IMemberRepository memberRepository, IOrganizationRepository organizationRepository)
        {
            this._memberRepository = memberRepository;
            this._organizationRepository = organizationRepository;
        }

        IList<Member> IApplicationService<Member>.GetListByPage(Page page)
        {
            return _memberRepository.FindList(m => true, page);
        }
    }
}