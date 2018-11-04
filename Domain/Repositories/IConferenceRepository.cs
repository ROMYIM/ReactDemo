using ReactDemo.Domain.Models.Meeting;

namespace ReactDemo.Domain.Repositories
{
    public interface IConferenceRepository : IRepository<Conference>
    {
        Hall FindHallById(int hallID);
    }
}