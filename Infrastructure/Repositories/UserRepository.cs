using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ReactDemo.Domain.Models.User;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Infrastructure.Repositories
{
    public class UserRepository : Repository<User, int>, IUserRepository
    {
        public UserRepository(DatabaseContext databaseContext, IHttpContextAccessor httpContextAccessor) : base(databaseContext, httpContextAccessor)
        {
            _entities = _databaseContext.Users;
        }

        // public Task<User> FindOneAsync(Expression<Func<User, bool>> predicate)
        // {
        //     return _entities.AsNoTracking().Include(u => u.UserRoles).ThenInclude(ur => ur.Role).SingleAsync(predicate);
        // }
    }
}