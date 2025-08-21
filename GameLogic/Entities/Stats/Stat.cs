namespace GameLogic.Entities.Stats;

using GameLogic.Config;

public class StatModifiers
{
    public List<StatModifier> Modifiers { get; } = new List<StatModifier>();
    public StatModifiers()
    {
    }

    public void AddModifier(StatModifier modifier)
    {
        this.Modifiers.Add(modifier);
    }

    public bool RemoveModifier(StatModifier modifier)
    {
        this.Modifiers.Remove(modifier);
    }

    public int CalculateValue(int baseValue)
    {
        int finalValue = baseValue;
        finalValue += (int)this.SumModifiers(StatModifierType.Flat);
        finalValue += (int)((float)baseValue * this.SumModifiers(StatModifierType.PercentAdd));
        finalValue *= (int)(1 + this.SumModifiers(StatModifierType.PercentMultiply));

        return finalValue;
    }

    public float SumModifiers(StatModifierType type)
    {
        float value = 0;
        foreach (var modifier in this.Modifiers)
        {
            if (modifier.Type == type)
            {
                value += modifier.Value;
            }
        }
        return value;
    }
}

public class StatModifier
{
    public StatModifierType Type { get; }
    public StatModifierSourceType SourceType { get; }
    public float Value { get; }

    public StatModifier(StatModifierType type, StatModifierSourceType sourceType, float value)
    {
        this.Type = type;
        this.SourceType = sourceType;
        this.Value = value;
    }


    public bool CanApplyOneTime()
    {
        if (this.SourceType != StatModifierSourceType.OneTime)
        {
            return false;
        }

        if (this.Type != StatModifierType.Flat)
        {
            return false;
        }

        return true;
    }

    public int OneTimeApply(int baseValue, int valueCap = System.Int32.MaxValue)
    {
        if (!this.CanApplyOneTime())
        {
            throw new Exception("Cannot apply one-time modifier to non-one-time modifier");
        }

        int newValue = baseValue + (int)this.Value;
        return System.Math.Clamp(newValue, 0, valueCap);
    }
}

public enum StatModifierType
{
    Flat,
    PercentAdd,
    PercentMultiply
}

public enum StatModifierSourceType
{
    Trait,
    Skill,
    OneTime,
}

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

    public int ModifyBaseValue(StatModifier modifier)
    {
        if (modifier.CanApplyOneTime())
        {
            CurrentMaxValue 
            this.BaseValue = modifier.OneTimeApply(this.BaseValue);
        }
        else
        {
            this.Modifiers.AddModifier(modifier);
        if (modifier.SourceType == StatModifierSourceType.OneTime)
        {
            this.BaseValue = modifier.OneTimeApply(this.BaseValue);
        }
        else
        {
            this.Modifiers.AddModifier(modifier);
            this.BaseValue = this.Modifiers.CalculateValue(this.BaseValue);
        }

        return this.BaseValue;
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

public class ValueStat : IStat
{
    private StatConfig _config { get; set; }
    private int _baseValue { get; set; }
    private int _modifiedValue { get; set; }

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

    public int GetCurrentMaxValue()
    {
        return -1;
    }

    public int UpdateCurrentMaxValue(int amount)
    {
        return -1;
    }

    public int UpdateBaseValue(int amount)
    {
        this._baseValue = System.Math.Clamp(amount + this._baseValue, 0, this._config.ValueCap);
        return this._baseValue;
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

public class ResourceStat : IStat
{
    private StatConfig _config { get; set; }
    private int _baseMaxValue { get; set; }
    private int _currentMaxValue { get; set; }
    private int _currentValue { get; set; }

    public Stat(StatConfig config)
    {
        this.ApplyConfig(config);
        this._baseMaxValue = this._config.BaseMaxValueFormula.CalculateValue();
    }

    public int GetBaseMaxValue()
    {
        return this._baseMaxValue;
    }

    public int GetCurrentMaxValue()
    {
        return this._currentMaxValue;
    }

    public int GetCurrentValue()
    {
        return this._currentValue;
    }
}
