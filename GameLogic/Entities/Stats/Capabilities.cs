namespace GameLogic.Entities.Stats;

using GameLogic.Registry;

public interface IHasBounds
{
    float LowerBound { get; set; }
    float UpperBound { get; set; }
    float ApplyBounds(float value);
}

public class StatBounds : IHasBounds
{
    public float LowerBound { get; set; }
    public float UpperBound { get; set; }

    public StatBounds(float lowerBound, float upperBound)
    {
        this.LowerBound = lowerBound;
        this.UpperBound = upperBound;
    }

    public float ApplyBounds(float value)
    {
        return System.Math.Clamp(value, this.LowerBound, this.UpperBound);
    }
}

public interface IHasMaxStat
{
    ReferenceId MaxStatId { get; init; }
    bool ByBaseValue { get; init; } // Whether to use the base value or the current value
    float GetMaxValue();
    float ApplyMaxStat(float value);
}

public class StatMaxStat : IHasMaxStat
{
    public ReferenceId MaxStatId { get; init; }
    public bool ByBaseValue { get; init; }

    private Func<ReferenceId, bool, float> GetMaxValueFunc { get; init; }

    public StatMaxStat(
        ReferenceId maxStatId,
        bool byBaseValue,
        Func<ReferenceId, bool, float> getMaxValueFunc
    )
    {
        this.MaxStatId = maxStatId;
        this.ByBaseValue = byBaseValue;
        this.GetMaxValueFunc = getMaxValueFunc;
    }

    public float GetMaxValue()
    {
        return this.GetMaxValueFunc(this.MaxStatId, this.ByBaseValue);
    }
}

public enum MaxStatType
{
    BaseValue,
    CurrentValue,
}

public interface IMutableValue
{
    float Value { get; set; }
    float Add(float delta);
    float Set(float value);
}

public class MutableValue : IMutableValue
{
    public float Value { get; set; }

    public MutableValue(float value)
    {
        this.Value = value;
    }

    public float Add(float delta)
    {
        this.Value = this.Value + delta;

        return this.Value;
    }

    public float Set(float value)
    {
        this.Value = value;
        return this.Value;
    }
}

public interface IImmutableValue
{
    StatFormula Formula { get; set; }
    float CalculateValue();
}

public class ImmutableValue : IImmutableValue
{
    public StatFormula Formula { get; set; }

    public ImmutableValue(StatFormula formula)
    {
        this.Formula = formula;
    }

    public float CalculateValue()
    {
        float value = this.Formula.CalculateValue();
        return value;
    }
}

public interface ICanRegen
{
    float RegenRate { get; set; }
    float RegenAmount { get; set; }
    float ApplyRegen(float value);
}
