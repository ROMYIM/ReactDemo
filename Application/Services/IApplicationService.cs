using System.Collections.Generic;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models;

namespace ReactDemo.Application.Services
{
    public interface IApplicationService<TEntity> where TEntity : Entity
    {
        IList<TEntity> GetListByPage(Page page);
    }
}