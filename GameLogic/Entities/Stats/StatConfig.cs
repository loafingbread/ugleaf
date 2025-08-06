namespace GameLogic.Entities.Stats;

public interface IStatRecord
{
    // Unique identifier for the stat
    public string Id { get; }

    // Display name for the stat
    public string DisplayName { get; }

    // Description for the stat
    public string Description { get; }

    // Tags for the stat
    public List<string> Tags { get; }

    // Actual base value or max value for resource stats
    public int BaseValue { get; }

    // Current value for the stat
    public int CurrentValue { get; }

    // Value cap for the stat
    public int ValueCap { get; }

    // Resource stats only
    // Resources only: Regeneration rate of the stat
    // public int RegenRate { get; }
    // Derived stats only: Formula for the stat
    // public Formula Formula { get; }
}

public record StatRecord : IStatRecord
{
    public required string Id { get; init; }
    public required string DisplayName { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }
    public required int BaseValue { get; init; }
    public int CurrentValue { get; init; }
    public required int ValueCap { get; init; }
}

// TODO: Add resource and derived stats. Examples of extending the record:
// public record ResourceStatRecord : StatRecord
// {
//     public required int RegenRate { get; init; }
// }

// public record DerivedStatRecord : StatRecord
// {
//     public required Formula Formula { get; init; }
// }

// public record DerivedResourceStatRecord : ResourceStatRecord
// {
//     public required int RegenRate { get; init; }
//     public required Formula Formula { get; init; }
// }

public class StatConfig
{
    public required string Id { get; init; }
    public required string DisplayName { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }
    public required int BaseValue { get; init; }
    public required int CurrentValue { get; init; }
    public required int ValueCap { get; init; }

    [SetsRequiredMembers]
    public StatConfig(IStatRecord record)
    {
        this.Id = record.Id;
        this.DisplayName = record.DisplayName;
        this.Description = record.Description;
        this.Tags = record.Tags ?? new List<string>();
        this.BaseValue = record.BaseValue;
        this.CurrentValue = record.CurrentValue ?? record.BaseValue;
        this.ValueCap = record.ValueCap;
    }
}
