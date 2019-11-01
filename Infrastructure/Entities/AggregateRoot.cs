using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ReactDemo.Infrastructure.Event.Events;

namespace ReactDemo.Infrastructure.Entities
{
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
    {

        private List<IEvent> _events;

	protected AggregateRoot(ILazyLoader lazyLoader) : base(lazyLoader)
        {
            _events = new List<IEvent>();
        }

        protected AggregateRoot()
        {
            _events = new List<IEvent>();
        }

        public IEnumerable<IEvent> DomainEvents => _events.AsReadOnly();

	public void AddEvent(IEvent @event)
	{
	    _events.Add(@event);
	}

	public void RemoveEvent(IEvent @event)
	{
	    _events.Remove(@event);
	}

	public void ClearEvents()
	{
	    _events.Clear();
	}
    }
}