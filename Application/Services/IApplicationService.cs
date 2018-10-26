using System.Collections.Generic;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models;

namespace ReactDemo.Application.Services
{
    public interface IApplicationService<TEntity, TDto> where TEntity : Entity
    {
        TEntity CreateEntity(TDto dto);

        IList<TEntity> GetListByPage(Page page);
    }
}