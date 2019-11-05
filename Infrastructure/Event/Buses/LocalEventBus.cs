using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ReactDemo.Infrastructure.Event.Events;
using ReactDemo.Infrastructure.Event.Handlers;

namespace ReactDemo.Infrastructure.Event.Buses
{
    public class LocalEventBus : IEventBus
    {
        private readonly HandlerFactory _handlerFactory;

        public LocalEventBus(IServiceProvider serviceProvider)
        {
            _handlerFactory = new HandlerFactory(serviceProvider);
        }

        void IPublisher.Publish<TEvent>(TEvent @event)
        {
            var handlers = _handlerFactory.GetHandlers<TEvent>();
            foreach (var handler in handlers)
            {
                if (handler.CanHandle(@event))
                {
                    @event.TriggerTime = DateTime.Now;
                    handler.Handle(@event);
                }
            }
        }

        Task IPublisher.PublishAsync<TEvent>(TEvent @event)
        {
            return Task.Run(() => 
            {
                var handlers = _handlerFactory.GetHandlers<TEvent>();
                foreach (var handler in handlers)
                {
                    if (handler.CanHandle(@event))
                    {
                        @event.TriggerTime = DateTime.Now;
                        handler.Handle(@event);
                    }
                }
            });
        }

        void IEventBus.Register<TEvent>()
        {
            _handlerFactory.AddHandler<TEvent>();
        }

        private class HandlerFactory
        {
            private readonly HashSet<Type> _eventTypeSet;

            private readonly IServiceProvider _servicePorvider;

            internal HandlerFactory(IServiceProvider serviceProvider)
            {
                _servicePorvider = serviceProvider;
                _eventTypeSet = new HashSet<Type>();
            }


            internal IEnumerable<IEventHandler<TEvent>> GetHandlers<TEvent>() where TEvent : IEvent
            {
                if (_eventTypeSet.Contains(typeof(TEvent)))
                {
                    return _servicePorvider.GetServices<IEventHandler<TEvent>>();
                }
                else
                {
                    return null;
                }
            }

            internal HandlerFactory AddHandler<TEvent>() where TEvent : IEvent
            {
                _eventTypeSet.Add(typeof(TEvent));
                return this;
            }
        }
    }
}