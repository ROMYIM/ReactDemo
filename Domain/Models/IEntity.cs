using System;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ReactDemo.Domain.Models
{
    public interface IEntity
    {
        int? ID { get; }

        DateTime CreateTime { get; }
    }
}