using System;

namespace ReactDemo.Domain.Models.Events
{
    public class Event<TSource> : IEvent<TSource>
    {

        private readonly TSource _source;

        private readonly Guid _id;

        private readonly DateTime _createTime;

        public Event(TSource source)
        {
            _source = source;
            _id = Guid.NewGuid();
            _createTime = DateTime.UtcNow;
        }

        public TSource Source => _source;

        public Guid ID => _id;

        public DateTime CreateTime => _createTime;
    }
}