using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ReactDemo.Domain.Models
{
    public class AggregateRoot : Entity, IAggregateRoot
    {
        protected AggregateRoot(ILazyLoader lazyLoader) : base(lazyLoader)
        {
        }

        protected AggregateRoot()
        {
            
        }
    }
}