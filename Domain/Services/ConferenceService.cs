using ReactDemo.Domain.Repositories;

namespace ReactDemo.Domain.Services
{
    public class ConferenceService : IConferenceService
    {
        private readonly IConferenceRepository _conferenceRepository;

        public ConferenceService(IConferenceRepository conferenceRepository)
        {
            this._conferenceRepository = conferenceRepository;
        }
        
    }
}