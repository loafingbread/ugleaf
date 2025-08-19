namespace GameLogic.Entities.Stats;

using GameLogic.Config;

public interface IStat : IConfigurable<StatConfig>
{
    public int GetBaseValue();
    public int GetModifiedValue();
    public int UpdateModifiedValue(int amount);
    public int GetCurrentValue();
    public int UpdateCurrentValue(int amount);
}

public class Stat : IStat
{
    private StatConfig _config { get; set; }
    private int _baseValue { get; set; }
    private int _modifiedValue { get; set; }
    private int _currentValue { get; set; }

    public Stat(StatConfig config)
    {
        this.ApplyConfig(config);
        this._baseValue = this._config.BaseValueFormula.CalculateValue();
    }

    public void ApplyConfig(StatConfig config)
    {
        this._config = config;
        this._baseValue = this._config.BaseValueFormula.CalculateValue();
    }

    public StatConfig GetConfig()
    {
        return this._config;
    }

    public int GetBaseValue()
    {
        return this._baseValue;
    }

    public int GetModifiedValue()
    {
        return this._modifiedValue;
    }

    public int UpdateModifiedValue(int amount)
    {
        this._modifiedValue = System.Math.Clamp(
            amount + this._modifiedValue,
            0,
            this._config.ValueCap
        );
        return this._modifiedValue;
    }

    public int GetCurrentValue()
    {
        return this._currentValue;
    }

    public int UpdateCurrentValue(int amount)
    {
        this._currentValue = System.Math.Clamp(
            amount + this._currentValue,
            0,
            this._config.ValueCap
        );
        return this._currentValue;
    }
}
