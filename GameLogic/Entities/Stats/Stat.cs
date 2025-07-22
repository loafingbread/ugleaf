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

public class StatBlock : IConfigurable<StatBlockConfig>
{
    private StatBlockConfig _config { get; set; }
    public List<Stat> Stats { get; } = new();

    public StatBlock(StatBlockConfig config)
    {
        this._config = config;
        this.ApplyConfig(config);
    }

    public void ApplyConfig(StatBlockConfig config)
    {
        this._config = config;

        this.Stats.Clear();
        foreach (StatConfig statConfig in config.Stats)
        {
            Stat stat = new(statConfig);
            this.Stats.Add(stat);
        }
    }

    public StatBlockConfig GetConfig()
    {
        return _config;
    }
}
