using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.Meeting;

namespace ReactDemo.Application.Services
{
    public interface IConferenceAppService : IApplicationService<Conference>
    {
        void CreateConference(ConferenceDto dto, int? memberID);

    }
}