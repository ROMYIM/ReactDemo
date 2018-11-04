using System;

namespace ReactDemo.Domain.Models
{
    public interface IEntity
    {
        int? ID { get; }
        DateTime CreateTime { get; }
    }
}