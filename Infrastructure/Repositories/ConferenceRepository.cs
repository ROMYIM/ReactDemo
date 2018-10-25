using ReactDemo.Domain.Models.Conference;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Infrastructure.Repositories
{
    public class ConferenceRepository : Repository<Conference>, IConferenceRepository
    {
        public ConferenceRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}