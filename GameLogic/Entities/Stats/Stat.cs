namespace GameLogic.Entities.Stats;

using GameLogic.Config;
public class Stat : IConfigurable<StatConfig>
{
    public StatConfig Config { get; }
    public StatModifiers Modifiers { get; } = new StatModifiers();
    public int BaseValue { get; }
    public int CurrentValue { get; }
    public int BaseMaxValue { get; }
    public int CurrentMaxValue { get; }

    public Stat(StatConfig config)
    {
        this.Config = config;
        this.BaseValue = config.BaseValueFormula?.CalculateValue() ?? -1;
        this.CurrentValue = config.StartingCurrentValue;
        this.BaseMaxValue = config.BaseMaxValueFormula?.CalculateValue() ?? -1;
    }

    protected int UpdateCurrentValue(int startingValue, StatModifier modifier)
    {
        if (startingValue >= 0)
        {
            this.CurrentValue = startingValue;
        }
        else
        {
            this.CurrentValue = this.BaseValue;
        }
    }



    public int UpdateCurrentValue(int amount)
    {
        this.CurrentValue = System.Math.
    }
    public int GetCurrentMaxValue()
    {
        return this._currentMaxValue;
    }

    public int UpdateBaseValue(int amount);

    public int GetCurrentMaxValue();
    public int UpdateCurrentMaxValue(int amount);

    public int GetCurrentValue();
    public int UpdateCurrentValue(int amount);
}
