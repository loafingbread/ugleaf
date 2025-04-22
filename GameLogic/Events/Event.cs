namespace GameLogic.Events;

public interface IEventBase
{
    public string CategoryId { get; }
}

public interface IEvent<TCategory> : IEventBase
    where TCategory : IEventCategory<EventCategoryConfig>
{
    public string Name => GetType().Name; // default implementation
    public DateTime Timestamp => DateTime.UtcNow;

    public void Log() => Console.WriteLine($"[EVENT] {Name} at {Timestamp}");
}

public abstract class GameEvent<TCategory> : IEvent<TCategory>
    where TCategory : IEventCategory<EventCategoryConfig>
{
    public TCategory Category { get; }
    string IEventBase.CategoryId => this.Category.Id;

    protected GameEvent(TCategory category)
    {
        Category = category;
    }
}
