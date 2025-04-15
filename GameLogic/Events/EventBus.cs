using Microsoft.VisualBasic;

namespace GameLogic.Events
{
    public class EventBus
    {
        private class EventHandler
        {
            public Func<IEvent, Task> Handler { get; }
            // Priority is used to determine the order of execution
            public int Priority { get; }

            public EventHandler(Func<IEvent, Task> handler, int priority)
            {
                Handler = handler;
                Priority = priority;
            }
        }

        private readonly Dictionary<Type, List<EventHandler>> _subscribers = new();

        public void Subscribe<TEvent>(Func<TEvent, Task> handler, int priority = 0) where TEvent : IEvent
        {
            var eventType = typeof(TEvent);
            if (!_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType] = new List<EventHandler>();
            }

            // Need to cast from IEvent to TEvent
            Func<IEvent, Task> handlerWrapper = e => handler((TEvent)e);
            _subscribers[eventType].Add(new EventHandler(handlerWrapper, priority));
            _subscribers[eventType].Sort((EventHandler a, EventHandler b) => b.Priority.CompareTo(a.Priority));
        }

        public void Unsubscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : IEvent
        {
            var eventType = typeof(TEvent);
            if (_subscribers.TryGetValue(eventType, out var eventHandlers))
            {
                eventHandlers.RemoveAll((EventHandler h) => h.Handler.Equals(handler));
            }
        }

        public async void Publish<TEvent>(TEvent evt) where TEvent : IEvent
        {
            var eventType = typeof(TEvent);
            if (_subscribers.TryGetValue(eventType, out var eventHandlers))
            {
                foreach (var handler in eventHandlers)
                {
                    try
                    {
                        await handler.Handler(evt);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error handling event {evt.Name}: {ex.Message}");
                    }
                }
            }
        }
    }
}