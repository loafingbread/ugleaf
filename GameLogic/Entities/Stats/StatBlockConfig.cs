namespace GameLogic.Entities.Stats;

using System.Diagnostics.CodeAnalysis;

public record StatBlockRecord
{
    public required List<ValueStatRecord> ValueStats { get; init; } = new();
    public required List<ResourceStatRecord> ResourceStats { get; init; } = new();
}

public class StatBlockConfig
{
    private readonly List<StatConfig> _configs { get; } = new();

    [SetsRequiredMembers]
    public StatBlockConfig(StatBlockRecord record)
    {
        this._configs.AddRange(record.ValueStats.Select(stat => new ValueStatConfig(stat)));
        this._configs.AddRange(record.ResourceStats.Select(stat => new ResourceStatConfig(stat)));
    }

    public List<StatConfig> GetAll()
    {
        return this._configs;
    }
}
