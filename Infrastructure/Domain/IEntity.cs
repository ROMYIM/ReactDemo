using System;

namespace ReactDemo.Infrastructure.Domain
{
    public interface IEntity<TKey> 
    {
        TKey ID { get; }

        DateTime CreateTime { get; }
    }

    public interface IEntity : IEntity<int> {}
    
}