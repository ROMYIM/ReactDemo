using System.Threading.Tasks;
using ReactDemo.Infrastructure.Event.Entities;

namespace ReactDemo.Infrastructure.Event.Buses
{
    public interface IEventBus : IPublisher
    {
        void Register<TEvent>() where TEvent : IEvent;
    }

    public interface IPublisher
    {
        void Publish<TEvent>(TEvent @event) where TEvent : IEvent;

        Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}