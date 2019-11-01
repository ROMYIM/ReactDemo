using System.Threading.Tasks;
using ReactDemo.Infrastructure.Event.Events;

namespace ReactDemo.Infrastructure.Event
{
    public delegate void EventHandler<in TEventArgs>(object sender, TEventArgs args);

    public delegate Task AsyncEventHandler<in TEventArgs> (object sender, TEventArgs args);
}