namespace GameLogic.Entities.Stats;

using GameLogic.Config;

public interface IStat : IConfigurable<StatConfig> { }

public class ValueStat : IStat
{
    private StatConfig _config { get; set; }

    public ValueStat(StatConfig config)
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
