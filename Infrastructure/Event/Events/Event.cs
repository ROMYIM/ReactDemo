using System;

namespace ReactDemo.Infrastructure.Event.Events
{
    public class Event<TSource> : EventArgs, IEvent
    {

        private readonly TSource _source;

        private readonly Guid _id;

        public Event(TSource source)
        {
            _source = source;
            _id = Guid.NewGuid();
        }

        public TSource Source => _source;

        public Guid ID => _id;

        public DateTime TriggerTime { get; set; }
    }
}