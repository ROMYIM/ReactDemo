using System;
using System.Collections.Generic;
using ReactDemo.Infrastructure.Event.Events;

namespace ReactDemo.Infrastructure.Event.Subscribers
{
    public interface IEventSubscriber
    {
        IEnumerable<EventHandler<IEvent>> Handlers { get; }

        void Subscribe<TEvent>(EventHandler<TEvent> @event) where TEvent : IEvent;

        void SubscribeAsync<TEvent>(TEvent @event) where TEvent : IEvent;

        void Subscribe<TEvent>(IEnumerable<TEvent> events) where TEvent : IEvent;

        void SubscribeAsync<TEvent>(IEnumerable<TEvent> events) where TEvent : IEvent;

        bool IsSubscribe<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}