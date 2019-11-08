using System.Threading.Tasks;
using ReactDemo.Infrastructure.Entities;
using ReactDemo.Infrastructure.Event.Buses;
using ReactDemo.Infrastructure.Event.Events;

namespace ReactDemo.Infrastructure.Event.Helpers
{
    public class EventHelper : IEventHelper
    {
        private readonly IEventBus _eventBus;

        public EventHelper(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public bool HandleEventData(IEvent @event)
        {
            var source = @event.GetSource() as IGenerateDomainEvents;
            if (source != null)
            {
                source.ClearEvents();
            }

            return true;
        }

        public void Push(IEvent @event)
        {
            _eventBus.Publish(@event);
        }

        public Task PushAsync(IEvent @event)
        {
            return _eventBus.PublishAsync(@event);
        }
    }
}