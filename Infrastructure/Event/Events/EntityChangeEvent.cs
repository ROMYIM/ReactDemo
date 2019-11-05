using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactDemo.Infrastructure.Event.Events.Domain
{
    public class EntityCreateEvent<TSource> : Event<TSource>
    {
        public EntityCreateEvent(TSource source) : base(source)
        {
        }
    }

    public class EntityUpdateEvent<TSource> : Event<TSource>
    {
        public EntityUpdateEvent(TSource source) : base(source)
        {
        }
    }

    public class EntityDeleteEvent<TSource> : Event<TSource>
    {
        public EntityDeleteEvent(TSource source) : base(source)
        {
        }
    }
}
