namespace GameLogic.Events;

public interface IEventCategory<out T>
    where T : EventCategoryConfig
{
    public string Id { get; }
    public T Config { get; }
}

// Derivable class for event category configuration
public class EventCategoryConfig
{
    public string DisplayName { get; set; } = "";
    public List<string> Tags { get; set; } = [];
}

public class BuiltInEventCategory : IEventCategory<EventCategoryConfig>
{
    public static readonly BuiltInEventCategory Combat = new(
        "combat",
        new EventCategoryConfig() { DisplayName = "Combat", Tags = ["combat"] }
    );
    public static readonly BuiltInEventCategory Menu = new(
        "menu",
        new EventCategoryConfig() { DisplayName = "Menu", Tags = ["menu"] }
    );
    public static readonly BuiltInEventCategory UI = new(
        "ui",
        new EventCategoryConfig() { DisplayName = "UI", Tags = ["ui"] }
    );
    public static readonly BuiltInEventCategory Inventory = new(
        "inventory",
        new EventCategoryConfig() { DisplayName = "Inventory", Tags = ["inventory"] }
    );
    public static readonly BuiltInEventCategory Entity = new(
        "entity",
        new EventCategoryConfig() { DisplayName = "Entity", Tags = ["entity"] }
    );
    public static readonly BuiltInEventCategory Skill = new(
        "skill",
        new EventCategoryConfig() { DisplayName = "Skill", Tags = ["skill"] }
    );

    public string Id { get; }
    public EventCategoryConfig Config { get; }

    private BuiltInEventCategory(string id, EventCategoryConfig config)
    {
        this.Id = id;
        this.Config = config;
    }
}
