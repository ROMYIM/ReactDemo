using ReactDemo.Domain.Models.Conference;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Infrastructure.Repositories
{
    public class HallRepository : Repository<Hall>, IHallRepository
    {
        public HallRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
            this.Entities = _databaseContext.Halls;
        }
    }
}