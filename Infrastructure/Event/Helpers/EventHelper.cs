using ReactDemo.Infrastructure.Entities;
using ReactDemo.Infrastructure.Event.Buses;
using ReactDemo.Infrastructure.Event.Events;

namespace ReactDemo.Infrastructure.Event.Helpers
{
    public class EventHelper : IEventHelper
    {
        private readonly IEventBus _eventBus;

        public EventHelper(LocalEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        bool IEventHelper.HandleEventData(IEvent @event)
        {
            var source = @event.GetSource() as IGenerateDomainEvents;
            if (source != null)
            {
                source.ClearEvents();
            }

            return true;
        }

        void IEventHelper.Push(IEvent @event)
        {
            throw new System.NotImplementedException();
        }
    }
}