using System.Threading.Tasks;
using ReactDemo.Infrastructure.Event.Entities;

namespace ReactDemo.Infrastructure.Event.Helpers
{
    public interface IEventHelper
    {
        void Push(IEvent @event);

        Task PushAsync(IEvent @event);

        bool HandleEventData(IEvent @event);
    }
}