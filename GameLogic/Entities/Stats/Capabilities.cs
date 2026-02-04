namespace GameLogic.Entities.Stats;

using GameLogic.Registry;

public interface IBounds
{
    float LowerBound { get; set; }
    float UpperBound { get; set; }
    float ApplyBounds(float value);
}

public class BoundsCapability : IBounds
{
    public float LowerBound { get; set; }
    public float UpperBound { get; set; }

    public BoundsCapability(float lowerBound, float upperBound)
    {
        this.LowerBound = lowerBound;
        this.UpperBound = upperBound;
    }

    public float ApplyBounds(float value)
    {
        return System.Math.Clamp(value, this.LowerBound, this.UpperBound);
    }
}

public interface IMaxStat
{
    ReferenceId MaxStatId { get; init; }
    bool ByBaseValue { get; init; } // Whether to use the base value or the current value
    float GetMaxValue();
    float ApplyMaxStat(float value);
}

public class MaxCapability : IMaxStat
{
    public ReferenceId MaxStatId { get; init; }
    public bool ByBaseValue { get; init; }

    private Func<ReferenceId, bool, float> GetMaxValueFunc { get; init; }

    public MaxCapability(
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

    public float ApplyMaxStat(float value)
    {
        return System.Math.Min(value, this.GetMaxValue());
    }
}

public enum MaxReferenceType
{
    BaseValue,
    CurrentValue,
}

public interface IMutable
{
    float Value { get; set; }
    float Add(float delta);
    float Set(float value);
}

public class MutableCapability : IMutable
{
    public float Value { get; set; }

    public MutableCapability(float value)
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

public interface IImmutable
{
    StatFormula Formula { get; set; }
    float CalculateValue();
}

public class ImmutableCapability : IImmutable
{
    public StatFormula Formula { get; set; }

    public ImmutableCapability(StatFormula formula)
    {
        this.Formula = formula;
    }

    public float CalculateValue()
    {
        float value = this.Formula.CalculateValue();
        return value;
    }
}

public interface IRegen
{
    float RegenRate { get; set; }
    float RegenAmount { get; set; }
    float ApplyRegen(float value);
}
