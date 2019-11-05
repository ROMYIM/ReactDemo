using System.Collections.Generic;
using ReactDemo.Domain.Models.Events;
using ReactDemo.Infrastructure.Event.Events;

namespace ReactDemo.Infrastructure.Entities
{
    public interface IAggregateRoot<TKey> : IEntity<TKey>, IGenerateDomainEvents
    {
        
    }

    public interface IAggregateRoot : IAggregateRoot<int> {}

    public interface IGenerateDomainEvents
    {
        IEnumerable<IEvent> DomainEvents { get; }

        void AddEvent(IEvent @event);

        void RemoveEvent(IEvent @event);

        void ClearEvents();
    }
}