using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.Conference;

namespace ReactDemo.Application.Services
{
    public interface IConferenceAppService : IApplicationService<Conference, ConferenceDto>
    {
        void CreateConference(ConferenceDto dto);

    }
}