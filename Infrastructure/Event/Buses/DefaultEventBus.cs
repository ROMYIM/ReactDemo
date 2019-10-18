using System.Collections.Generic;
using ReactDemo.Infrastructure.Event.Subscribers;

namespace ReactDemo.Infrastructure.Event.Buses
{
    public class DefaultEventBus : IEventBus
    {
        private readonly IEnumerable<IEventSubscriber> _subscribers;

        public DefaultEventBus(IEnumerable<IEventSubscriber> subscribers)
        {
            _subscribers = subscribers;
        }

        void IPublisher.Publish<TEvent>(TEvent @event)
        {
            throw new System.NotImplementedException();
        }

        void IPublisher.PublishAsync<TEvent>(TEvent @event)
        {
            throw new System.NotImplementedException();
        }
    }
}