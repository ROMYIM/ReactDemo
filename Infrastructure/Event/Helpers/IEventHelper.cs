using System.Threading.Tasks;
using ReactDemo.Infrastructure.Event.Events;

namespace ReactDemo.Infrastructure.Event.Helpers
{
    public interface IEventHelper
    {
        void Push(IEvent @event);

        Task PushAsync(IEvent @event);

        bool HandleEventData(IEvent @event);
    }
}