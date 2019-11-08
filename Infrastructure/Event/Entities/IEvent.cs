using System;

namespace ReactDemo.Infrastructure.Event.Entities
{
    public interface IEvent
    {
        Guid ID { get; }

        DateTime TriggerTime { set; get; }

        // TSource GetSource<TSource>() where TSource : class;

        object GetSource();
    }
}