using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ReactDemo.Domain.Models.Events;

namespace ReactDemo.Domain.Models
{
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
    {

        private List<IEvent<object>> _events;

        protected AggregateRoot(ILazyLoader lazyLoader) : base(lazyLoader)
        {
            _events = new List<IEvent<object>>();
        }

        IEnumerable<IEvent<object>> IAggregateRoot<TKey>.DomainEvents => _events;
    }
}