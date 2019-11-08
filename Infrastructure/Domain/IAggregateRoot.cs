using System.Collections.Generic;
using ReactDemo.Infrastructure.Event.Entities;

namespace ReactDemo.Infrastructure.Domain
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