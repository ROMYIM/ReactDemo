using System;
using System.Threading.Tasks;
using ReactDemo.Infrastructure.Event.Entities;

namespace ReactDemo.Infrastructure.Event.Handlers
{
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : IEvent
    {
        void Handle(TEvent @event);

        Task HandleAsync(TEvent @event);
    }

    public interface IEventHandler
    {
        bool CanHandle(IEvent @event);

        Type GetEventType();
    }
}