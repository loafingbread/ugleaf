namespace GameLogic.Events;

public class EventBus
{
    private class EventHandler
    {
        public Func<IEventBase, Task> Handler { get; }
        public object OriginalHandler { get; }

        // Priority is used to determine the order of execution
        public int Priority { get; }

        public EventHandler(Func<IEventBase, Task> handler, object originalHandler, int priority)
        {
            this.Handler = handler;
            this.OriginalHandler = originalHandler;
            this.Priority = priority;
        }
    }

    private readonly Dictionary<Type, List<EventHandler>> _subscribers = [];

    public void Subscribe<TEvent, TCategory>(Func<TEvent, Task> handler, int priority = 0)
        where TEvent : IEvent<TCategory>, IEventBase
        where TCategory : IEventCategory<EventCategoryConfig>
    {
        var eventType = typeof(TEvent);
        if (!_subscribers.ContainsKey(eventType))
        {
            _subscribers[eventType] = [];
        }

        // Need to cast from IEventBase to TEvent
        Func<IEventBase, Task> handlerWrapper = (IEventBase e) => handler((TEvent)e);
        _subscribers[eventType].Add(new EventHandler(handlerWrapper, handler, priority));
        _subscribers[eventType]
            .Sort((EventHandler a, EventHandler b) => b.Priority.CompareTo(a.Priority));
    }

    public void Unsubscribe<TEvent, TCategory>(Func<TEvent, Task> handler)
        where TEvent : IEvent<TCategory>, IEventBase
        where TCategory : IEventCategory<EventCategoryConfig>
    {
        var eventType = typeof(TEvent);
        if (_subscribers.TryGetValue(eventType, out var eventHandlers))
        {
            eventHandlers.RemoveAll(h => h.OriginalHandler.Equals(handler));
        }
    }

    public async void Publish<TEvent, TCategory>(TEvent evt)
        where TEvent : IEvent<TCategory>, IEventBase
        where TCategory : IEventCategory<EventCategoryConfig>
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
