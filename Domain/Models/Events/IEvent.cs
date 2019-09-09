using System;

namespace ReactDemo.Domain.Models.Events
{
    public interface IEvent<TSource> : IEntity<Guid>
    {
        TSource Source { get; }
    }
}