using Microsoft.AspNetCore.Http;
using ReactDemo.Domain.Models.User;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Infrastructure.Repositories
{
    public class UserRepository : Repository<User, uint>, IUserRepository
    {
        public UserRepository(DatabaseContext databaseContext, IHttpContextAccessor httpContextAccessor) : base(databaseContext, httpContextAccessor)
        {
            _entities = _databaseContext.Users;
        }
    }
}