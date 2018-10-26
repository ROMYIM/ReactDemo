using System;
using System.Collections.Generic;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.Conference;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Application.Services
{
    public class ConferenceAppService : IApplicationService<Conference, ConferenceDto>, IConferenceAppService
    {

        private readonly IConferenceRepository _conferenceRepository;
        private readonly IHallRepository _hallRepository;

        public ConferenceAppService(IConferenceRepository conferenceRepository, IHallRepository hallRepository)
        {
            this._conferenceRepository = conferenceRepository;
            this._hallRepository = hallRepository;
        }

        void IConferenceAppService.CreateConference(ConferenceDto dto)
        {
            var conference = CreateEntity(dto);
            _conferenceRepository.Add(conference);
            if (_conferenceRepository.SaveChanges() == 0)
            {
                throw new Exception();
            }
            //  加入推送事件
        }

        public Conference CreateEntity(ConferenceDto dto)
        {
            var hall = _hallRepository.FindOne(h => h.ID == dto.HallID);
            return new Conference(dto, hall);
        }

        public IList<Conference> GetListByPage(Page page)
        {
            return _conferenceRepository.FindList((c => 1 == 1), page);
        }
    }
}