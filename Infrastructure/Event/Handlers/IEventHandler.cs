using System.Threading.Tasks;
using ReactDemo.Infrastructure.Event.Events;

namespace ReactDemo.Infrastructure.Event.Handlers
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        bool CanHandle(TEvent @event);

        void Handle(TEvent @event);

        Task HandleAsync(TEvent @event);
    }
}