using System;
using System.Threading.Tasks;
using ReactDemo.Infrastructure.Event.Entities;
using ReactDemo.Infrastructure.Repositories;

namespace ReactDemo.Infrastructure.Event.Handlers.Domain
{
    public abstract class EntityChangeEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : class, IEvent
    {
        protected readonly DatabaseContext _dbContext;

        public EntityChangeEventHandler(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual bool CanHandle(IEvent @event)
        {
            return @event != null && @event.GetSource() != null && @event is TEvent;
        }

        public virtual Type GetEventType()
        {
            return typeof(TEvent);
        }

        public abstract void Handle(TEvent @event);
        public abstract Task HandleAsync(TEvent @event);
    }

    public class EntityCreateEventHandler<TSource> : EntityChangeEventHandler<EntityCreateEvent<TSource>> where TSource : class
    {
        public EntityCreateEventHandler(DatabaseContext dbContext) : base(dbContext)
        {
        }

        public override void Handle(EntityCreateEvent<TSource> @event)
        {
            var source = @event.Source;
            _dbContext.Set<TSource>().Add(source);
            _dbContext.SaveChanges();
        }

        public override async Task HandleAsync(EntityCreateEvent<TSource> @event)
        {
            var source = @event.Source;
            _dbContext.Set<TSource>().Add(source);
            await _dbContext.SaveChangesAsync();
        }
    }

    public class EntityUpdateEventHandler<TSource> : EntityChangeEventHandler<EntityUpdateEvent<TSource>> where TSource : class
    {
        public EntityUpdateEventHandler(DatabaseContext dbContext) : base(dbContext)
        {
        }

        public override void Handle(EntityUpdateEvent<TSource> @event)
        {
            var source = @event.Source;
            _dbContext.Set<TSource>().Update(source);
            _dbContext.SaveChanges();
        }

        public override async Task HandleAsync(EntityUpdateEvent<TSource> @event)
        {
            var source = @event.Source;
            _dbContext.Set<TSource>().Update(source);
            await _dbContext.SaveChangesAsync();
        }
    }
}