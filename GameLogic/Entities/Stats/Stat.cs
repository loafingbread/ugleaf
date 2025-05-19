using GameLogic.Config;

namespace GameLogic.Entities.Stats;

public interface IStat : IConfigurable<StatConfig> { }

public class Stat : IStat
{
    private StatConfig _config { get; set; }

    public Stat(StatConfig config)
    {
        this._config = config;
    }

    public void ApplyConfig(StatConfig config)
    {
        this._config = config;
    }

    public StatConfig GetConfig()
    {
        return this._config;
    }
}

public class StatBlock : IConfigurable<List<Stat>>
{
    private StatBlockConfig _config { get; }
    private List<Stat> _stats { get; } = new();

    public StatBlock(StatBlockConfig config)
    {
        this._config = config;

        foreach (StatConfig statConfig in config.Stats)
        {
            Stat stat = new(statConfig);
            this._stats.Add(stat);
        }
    }

    public void ApplyConfig(List<Stat> stats)
    {
        this._stats.Clear();
        this._stats.AddRange(stats);
    }

    public List<Stat> GetConfig()
    {
        return _stats;
    }
}
