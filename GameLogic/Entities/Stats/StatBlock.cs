namespace GameLogic.Entities.Stats;

public class StatBlock : IConfigurable<StatBlockConfig>
{
    private StatBlockConfig _config { get; set; }
    public List<IStat> Stats { get; } = new();

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
