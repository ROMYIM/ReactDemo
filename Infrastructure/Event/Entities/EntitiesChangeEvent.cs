using System.Collections.Generic;

namespace ReactDemo.Infrastructure.Event.Entities
{
    public class EntitiesCreateEvent<TSources, TElement> : Event<TSources> where TSources : ICollection<TElement>
    {
	public EntitiesCreateEvent(TSources sources) : base(sources)
	{
	}
    }

    public class EntitiesUpdateEvent<TSources, TElement> : Event<TSources> where TSources : ICollection<TElement>
    {
	public EntitiesUpdateEvent(TSources sources) : base(sources)
	{
	}
    }

    public class EntitiesDeleteEvent<TSources, TElement> : Event<TSources> where TSources : ICollection<TElement>
    {
	public EntitiesDeleteEvent(TSources sources) : base(sources)
	{
	}
    }
}