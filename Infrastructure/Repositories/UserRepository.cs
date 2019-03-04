using Microsoft.AspNetCore.Http;
using ReactDemo.Domain.Models.System;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext databaseContext, IHttpContextAccessor httpContextAccessor) : base(databaseContext, httpContextAccessor)
        {
            _entities = _databaseContext.Users;
        }
    }
}