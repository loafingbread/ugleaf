namespace GameLogic.Entities.Stats;

using System.Diagnostics.CodeAnalysis;

public record StatBlockRecord
{
    public required List<StatRecord> Stats { get; init; } = new();
}

public class StatBlockConfig
{
    private readonly List<StatConfig> _configs;

    [SetsRequiredMembers]
    public StatBlockConfig(StatBlockRecord record)
    {
        this._configs = record.Stats.Select(stat => new StatConfig(stat)).ToList();
    }

    public List<StatConfig> GetAll()
    {
        return this._configs;
    }

    public StatConfig Get(string id)
    {
        return this._configs.FirstOrDefault(config => config.Id == id, null);
    }
}
