using System.Collections.Generic;
using ReactDemo.Domain.Models.Events;

namespace ReactDemo.Domain.Models
{
    public interface IAggregateRoot<TKey> : IEntity<TKey>
    {
        IEnumerable<IEvent> DomainEvents { get; }
    }
}