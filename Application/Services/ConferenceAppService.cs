using System;
using System.Collections.Generic;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.Meeting;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Application.Services
{
    public class ConferenceAppService : IApplicationService<Conference>, IConferenceAppService
    {

        private readonly IConferenceRepository _conferenceRepository;
        private readonly IHallRepository _hallRepository;
        private readonly IMemberRepository _memberRepository;

        public ConferenceAppService(
            IConferenceRepository conferenceRepository, 
            IHallRepository hallRepository,
            IMemberRepository memberRepository)
        {
            this._conferenceRepository = conferenceRepository;
            this._hallRepository = hallRepository;
            this._memberRepository = memberRepository;
        }

        void IConferenceAppService.CreateConference(ConferenceDto dto, int? memberID)
        {
            var member = _memberRepository.FindOne(m => m.ID == memberID);
            var hall = _hallRepository.FindOne(h => h.ID == dto.HallID);
            var conference = member.CreateConference(dto, hall);
            _conferenceRepository.Add(conference);
            if (_conferenceRepository.SaveChanges() == 0)
            {
                throw new Exception();
            }
            //  加入推送事件
        }

        public IList<Conference> GetListByPage(Page page)
        {
            return _conferenceRepository.FindList((c => 1 == 1), page);
        }
    }
}