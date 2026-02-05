namespace GameLogic.Entities.Stats.Integrations;

using GameLogic.Entities.Stats.Query;
using GameLogic.Entities.Stats.Stat;
using GameLogic.Registry;

public class StatProvider : IStatProvider
{
    private IRegistry registry { get; init; }

    public StatProvider(IRegistry registry)
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
