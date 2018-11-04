using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ReactDemo.Domain.Models
{
    public class AggregateRoot : Entity
    {
        protected AggregateRoot(ILazyLoader lazyLoader) : base(lazyLoader)
        {
        }

        protected AggregateRoot()
        {
            
        }
    }
}