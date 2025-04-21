namespace GameLogic.Events
{
    // Derivable class for event category configuration
    public class EventCategoryConfig
    {
        public string DisplayName { get; set; } = "";
        public List<string> Tags { get; set; } = new();
    }

    public interface IEventCategory<out T>
        where T : EventCategoryConfig
    {
        public string Id { get; }
        public T Config { get; }
    }

    public class BuiltInEventCategory : IEventCategory<EventCategoryConfig>
    {
        public static readonly BuiltInEventCategory Combat = new(
            "combat",
            new EventCategoryConfig()
            {
                DisplayName = "Combat",
                Tags = new List<string>() { "combat" },
            }
        );
        public static readonly BuiltInEventCategory Menu = new(
            "menu",
            new EventCategoryConfig()
            {
                DisplayName = "Menu",
                Tags = new List<string>() { "menu" },
            }
        );
        public static readonly BuiltInEventCategory UI = new(
            "ui",
            new EventCategoryConfig()
            {
                DisplayName = "UI",
                Tags = new List<string>() { "ui" },
            }
        );
        public static readonly BuiltInEventCategory Inventory = new(
            "inventory",
            new EventCategoryConfig()
            {
                DisplayName = "Inventory",
                Tags = new List<string>() { "inventory" },
            }
        );
        public static readonly BuiltInEventCategory Entity = new(
            "entity",
            new EventCategoryConfig()
            {
                DisplayName = "Entity",
                Tags = new List<string>() { "entity" },
            }
        );
        public static readonly BuiltInEventCategory Skill = new(
            "skill",
            new EventCategoryConfig()
            {
                DisplayName = "Skill",
                Tags = new List<string>() { "skill" },
            }
        );

        public string Id { get; }
        public EventCategoryConfig Config { get; }

        private BuiltInEventCategory(string id, EventCategoryConfig config)
        {
            this.Id = id;
            this.Config = config;
        }
    }
}
