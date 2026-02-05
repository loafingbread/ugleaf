namespace GameLogic.Entities.Stats.Capabilities;

using GameLogic.Registry;

public record MaxData
{
    /// <summary>
    /// The reference id of the stat to use as the max value.
    /// </summary>
    public required ReferenceId MaxStatId { get; init; }

    /// <summary>
    /// Whether to use the base value or the current value of the max stat.
    /// </summary>
    public required bool ByBaseValue { get; init; }
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
