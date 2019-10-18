using ReactDemo.Infrastructure.Event.Events;

namespace ReactDemo.Infrastructure.Event.Buses
{
    public interface IEventBus : IPublisher
    {
         
    }

    public interface IPublisher
    {
        void Publish<TEvent>(TEvent @event) where TEvent : IEvent;

        void PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}