using System.Diagnostics.CodeAnalysis;

namespace GameLogic.Entities.Stats;

public interface IStatRecord
{
    public string Id { get; }
    public int BaseValue { get; }
    public int CurrentValue { get; }
}

public record StatRecord : IStatRecord
{
    public required string Id { get; init; }
    public required int BaseValue { get; init; }
    public int CurrentValue { get; init; }
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

public interface IStatBlockRecord
{
    public List<IStatRecord> Stats { get; }
}

public record StatBlockRecord
{
    public required List<IStatRecord> Stats { get; init; } = new();
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
