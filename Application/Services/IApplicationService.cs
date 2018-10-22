using ReactDemo.Domain.Models;

namespace ReactDemo.Application.Services
{
    public interface IApplicationService<TEntity, TDto> where TEntity : Entity
    {
        TEntity CreateEntity(TDto dto);
    }
}