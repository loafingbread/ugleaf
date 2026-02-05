namespace GameLogic.Entities.Stats.Query;

using GameLogic.Entities.Stats.Stat;
using GameLogic.Registry;

/// <summary>
/// Stat provider that can get the stat, base value, and current value of a stat.
/// Allows for the stat to be resolved from a reference id, without having to
/// know the about the implementation details of getting the stat.
///
/// This is useful for when you need to get the stat, but you don't want to worry
/// about underlying stat storage or lookup logic like registries or data stores.
///
/// </summary>
public interface IStatProvider
{
    Stat GetStat(ReferenceId referenceId);
    float GetBaseValue(ReferenceId referenceId);
    float GetCurrentValue(ReferenceId referenceId);
}
