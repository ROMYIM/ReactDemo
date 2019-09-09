using System;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ReactDemo.Domain.Models
{
    public interface IEntity<TKey>
    {
        TKey ID { get; }

        DateTime CreateTime { get; }
    }
}