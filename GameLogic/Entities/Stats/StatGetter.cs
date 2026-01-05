namespace GameLogic.Entities.Stats;

using GameLogic.Registry;

/// <summary>
/// A stat getter is a class that can get the base value and current value of a stat.
/// </summary>
public interface IStatGetter
{
    StatBase GetStat(ReferenceId referenceId);
    float GetBaseValue(ReferenceId referenceId);
    float GetCurrentValue(ReferenceId referenceId);
}

public class StatGetter : IStatGetter
{
    private IRegistry registry { get; init; }

    public StatGetter(IRegistry registry)
    {
        this.registry = registry;
    }

    public Stat GetStat(ReferenceId referenceId)
    {
        this.registry.TryGetReference<Stat>(referenceId, out IReference<Stat, ReferenceSpec>? stat);
        return stat is not null
            ? stat.GetValue()
            : throw new InvalidOperationException($"Stat not found: {referenceId}");
    }

    public float GetBaseValue(ReferenceId referenceId)
    {
        return this.GetStat(referenceId).Value;
    }

    public float GetCurrentValue(ReferenceId referenceId)
    {
        return this.GetStat(referenceId).Value;
    }
}
