using System.Collections.Generic;
using System.Threading.Tasks;
using ReactDemo.Application.Dtos;
using ReactDemo.Infrastructure.Domain;

namespace ReactDemo.Application.Services
{
    public interface IApplicationService<TEntity, TKey> where TEntity : Entity<TKey>
    {
        Task<List<TEntity>> GetListByPageAsync(Page page);
    }

    
}