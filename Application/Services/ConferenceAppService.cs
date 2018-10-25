using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.Conference;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Application.Services
{
    public class ConferenceAppService : IApplicationService<Conference, ConferenceDto>
    {

        private readonly IConferenceRepository _conferenceRepository;

        Conference IApplicationService<Conference, ConferenceDto>.CreateEntity(ConferenceDto dto)
        {
            throw new System.NotImplementedException();
        }
    }
}