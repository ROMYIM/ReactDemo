using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReactDemo.Infrastructure.Event.Events;
using ReactDemo.Infrastructure.Event.Handlers;

namespace ReactDemo.Infrastructure.Event.Buses
{
    public class LocalEventBus : IEventBus
    {
        private readonly HandlerFactory _handlerFactory;

        private readonly ILogger _logger;

        public LocalEventBus(
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider)
        {
            _logger = loggerFactory.CreateLogger(GetType());

            _handlerFactory = new HandlerFactory(serviceProvider);
            _handlerFactory.Logger = loggerFactory.CreateLogger(_handlerFactory.GetType());
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
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

        public Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            return Task.Run(() => 
            {
                var handlers = _handlerFactory.GetHandlers<TEvent>();
                foreach (var handler in handlers)
                {
                    if (handler.CanHandle(@event))
                    {
                        @event.TriggerTime = DateTime.Now;
                        handler.HandleAsync(@event);
                    }
                }
            });
        }

        public void Register<TEvent>() where TEvent : IEvent
        {
            _handlerFactory.AddHandler<TEvent>();
        }

        private class HandlerFactory
        {
            private readonly HashSet<Type> _eventTypeSet;

            private readonly IServiceProvider _servicePorvider;

            public ILogger Logger { get; set; }

            internal HandlerFactory(IServiceProvider serviceProvider)
            {
                _servicePorvider = serviceProvider;
                _eventTypeSet = new HashSet<Type>();

                IEnumerable<IEventHandler> handlers = serviceProvider.GetServices<IEventHandler>();
                foreach (var handler in handlers)
                {
                    AddHandler(handler.GetEventType());
                }
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
                if (!_eventTypeSet.Add(typeof(TEvent)))
                {
                    Logger.LogError("{0}  事件添加失败", typeof(TEvent).FullName);
                }
                return this;
            }

            internal HandlerFactory AddHandler(Type eventType)
            {
                if (!_eventTypeSet.Add(eventType))
                {
                    Logger.LogError("{0}  事件添加失败", eventType.FullName);
                }
                return this;
            }
        }
    }
}