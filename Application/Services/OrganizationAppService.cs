using System;
using System.Collections.Generic;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.Party;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Application.Services
{
    public class OrganizationAppService : IOrganizationAppService
    {

        private readonly IOrganizationRepository _organizationRepository;
        private readonly IMemberRepository _memberRepository;

        public OrganizationAppService(
            IOrganizationRepository organizationRepository,
            IMemberRepository memberRepository)
        {
            this._organizationRepository = organizationRepository;
            this._memberRepository = memberRepository;
        }

        public IList<Organization> GetListByPage(Page page)
        {
            var member = _memberRepository.FindMember();

            return _organizationRepository.FindList(o => true, page);
        }

        void IOrganizationAppService.CreateOrganization(OrganizationDto dto)
        {
            var superOrganization = _organizationRepository.FindOne(o => o.ID == dto.SuperOrganizationID);
            var organization = superOrganization.CreateOrganization(dto);
            _organizationRepository.Add(organization);
        }

        void IOrganizationAppService.EditOrganization(OrganizationDto dto)
        {
            var organization = _organizationRepository.FindOne(o => o.ID == dto.OrganizationID);
            organization.Edit(dto);
            _organizationRepository.Update(organization);
        }

        IList<Organization> IApplicationService<Organization>.GetListByPage(Page page)
        {
            var member = _memberRepository.FindMember();
            int? superOrganizationID = member.OrganizationID;
            List<Organization> organizations = null;
            FindAllChildrenOrganization(out organizations, superOrganizationID);
            return organizations;
        }

        private void FindAllChildrenOrganization(out List<Organization> organizations, int? superOrganizationID)
        {
            organizations = new List<Organization>();
            List<Organization> organizationList = _organizationRepository.FindList(o => o.SuperOrganizationID == superOrganizationID);
            Queue<Organization> organizationQueue = new Queue<Organization>(organizationList);
            while (organizationQueue.Count > 0)
            {
                organizations.AddRange(organizationList);
                var organization = organizationQueue.Dequeue();
                superOrganizationID = organization.SuperOrganizationID;
                organizationList = _organizationRepository.FindList(o => o.SuperOrganizationID == superOrganizationID);
                if (organizationList != null && organizationList.Count > 0)
                {
                    organizationList.ForEach(item => organizationQueue.Enqueue(item));
                }
            }
        }
    }
}