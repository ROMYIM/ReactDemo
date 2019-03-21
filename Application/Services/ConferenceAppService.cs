using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.Meeting;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Application.Services
{
    public class ConferenceAppService : IApplicationService<Conference>, IConferenceAppService
    {

        private readonly IConferenceRepository _conferenceRepository;
        private readonly IMemberRepository _memberRepository;

        public ConferenceAppService(
            IConferenceRepository conferenceRepository, 
            IMemberRepository memberRepository)
        {
            this._conferenceRepository = conferenceRepository;
            this._memberRepository = memberRepository;
        }

        void IConferenceAppService.CreateConference(ConferenceDto dto)
        {
            var member = _memberRepository.FindMember();
            var hall = _conferenceRepository.FindHallById(dto.HallID);
            var conference = member.CreateConference(dto, hall);
            _conferenceRepository.Add(conference);
            if (_conferenceRepository.SaveChanges() == 0)
            {
                throw new Exception();
            }
            //  加入推送事件
        }

        public async Task<List<Conference>> GetListByPageAsync(Page page)
        {
            return await _conferenceRepository.FindListAsync((c => 1 == 1), page);
        }
    }
}