using System;

namespace ReactDemo.Infrastructure.Entities
{
    public interface IEntity<TKey> 
    {
        TKey ID { get; }

        DateTime CreateTime { get; }
    }

    public interface IEntity : IEntity<int> {}
    
}