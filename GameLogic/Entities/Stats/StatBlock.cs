namespace GameLogic.Entities.Stats;

using System.Diagnostics.CodeAnalysis;
using GameLogic.Config;

public class StatBlock : IConfigurable<StatBlockConfig>
{
    private StatBlockConfig _config { get; set; }
    private List<IStat> _stats { get; } = new();

    [SetsRequiredMembers]
    public StatBlock(StatBlockConfig config)
    {
        this.ApplyConfig(config);
    }

    public void ApplyConfig(StatBlockConfig config)
    {
        this._config = config;

        this._stats.Clear();
        foreach (StatConfig statConfig in config.GetAll())
        {
            this._stats.Add((IStat)new Stat(statConfig));
        }
    }

    public StatBlockConfig GetConfig()
    {
        return this._config;
    }

    public IStat GetStat(string id)
    {
        return this._stats.FirstOrDefault(stat => stat.GetConfig().Id == id, null);
    }
}

public static class StatBlockFactory
{
    public static StatBlock CreateFromRecord(StatBlockRecord record)
    {
        StatBlockConfig config = new StatBlockConfig(record);
        return new StatBlock(config);
    }

    public static Stat CreateStatFromRecord(IStatRecord record)
    {
        StatConfig config = new StatConfig(record);
        return new Stat(config);
    }
}
