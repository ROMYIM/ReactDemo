using System.Collections.Generic;
using System.Threading.Tasks;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models;

namespace ReactDemo.Application.Services
{
    public interface IApplicationService<TEntity> where TEntity : Entity
    {
        Task<List<TEntity>> GetListByPageAsync(Page page);
    }
}