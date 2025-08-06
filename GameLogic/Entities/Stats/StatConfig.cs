using System.Diagnostics.CodeAnalysis;

namespace GameLogic.Entities.Stats;

public interface IStatRecord
{
    public string Id { get; }
    public int BaseValue { get; }
    public int CurrentValue { get; }
    public int MaxValue { get; }
}

public record StatRecord : IStatRecord
{
    public required string Id { get; init; }
    public required int BaseValue { get; init; }
    public int MaxValue { get; init; }
    public int CurrentValue { get; init; }
}

public ValueStatRecord: IStatRecord
{
    public required string Id { get; init; }
    public required int BaseValue { get; init; }
    public int CurrentValue { get; init; }
}

public record ResourceStatRecord : IStatRecord
{
    public required string Id { get; init; }
    public required int BaseValue { get; init; }
    public int CurrentValue { get; init; }
    public required int MaxValue { get; init; }
}

public interface IStatConfig
{
    public string Id { get; }
    public int BaseValue { get; }
    public int CurrentValue { get; }
}

public class ValueStatConfig : IStatConfig
{
    public string Id { get; }
    public int BaseValue { get; }
    public int CurrentValue { get; }
}

public class ResourceStatConfig : IStatConfig
{
    public string Id { get; }
    public int BaseValue { get; }
    public int CurrentValue { get; }
    public int MaxValue { get; }
}

public class StatConfig
{
    public required string Id { get; init; }
    public int BaseValue { get; }
    public int CurrentValue { get; }

    [SetsRequiredMembers]
    public StatConfig(IStatRecord record)
    {
        this.Id = record.Id;
        this.BaseValue = record.BaseValue;
        this.CurrentValue = record.CurrentValue;
    }
}
// 🔸 3. Modifier Condition Extensions (Later)
// Your condition syntax is great. Eventually, consider extending:

// StatAbove

// HasStatus

// TagPresent

// IsInState (e.g. stance, biome, combat phase)

// Just good to plan for flexibility.

// 1. BaseValue
// +  Sum of Flat Modifiers
// +  (BaseValue * Sum of PercentAdd modifiers)
// ×  Product of (1 + PercentMul modifiers)
// = Final Value

// enum StatModifierType {
//     Flat,         // +X
//     PercentAdd,   // +X% of base (additive)
//     PercentMul    // *X% of final value (multiplicative)
// }

public interface IStatBlockRecord
{
    public List<StatRecord> Stats { get; }
}

public record StatBlockRecord : IStatBlockRecord
{
    public required List<StatRecord> Stats { get; init; } = new();
}

public class StatBlockConfig
{
    public required List<StatConfig> Stats { get; init; } = new();

    [SetsRequiredMembers]
    public StatBlockConfig(IStatBlockRecord record)
    {
        foreach (StatRecord statRecord in record.Stats) {
            StatConfig statConfig = new (statRecord);
            this.Stats.Add(statConfig);
        }
    }
}

public static class StatBlockFactory
{
    public static StatBlock CreateFromRecord(IStatBlockRecord record)
    {
        StatBlockConfig config = new(record);
        return new StatBlock(config);
    }

    public static Stat CreateStatFromRecord(IStatRecord record)
    {
        StatConfig config = new(record);
        return new Stat(config);
    }
}
