using ReactDemo.Domain.Models.User;
using ReactDemo.Infrastructure.Repositories;

namespace ReactDemo.Domain.Repositories
{
    public interface IUserRepository : IRepository<User, int>
    {
         
    }
}