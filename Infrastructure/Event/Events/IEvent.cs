using System;

namespace ReactDemo.Infrastructure.Event.Events
{
    public interface IEvent
    {
        Guid ID { get; }

        DateTime TriggerTime { set; get; }

        TSource GetSource<TSource>() where TSource : class;
    }
}