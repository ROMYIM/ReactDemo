using Microsoft.EntityFrameworkCore.Infrastructure;
using ReactDemo.Domain.Models.Events;

namespace ReactDemo.Domain.Models
{
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
    {
        protected AggregateRoot(ILazyLoader lazyLoader) : base(lazyLoader)
        {
        }

        protected AggregateRoot() {}

    }
}