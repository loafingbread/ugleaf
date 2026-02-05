namespace GameLogic.Entities.Stats.Factories;

using GameLogic.Entities.Stats.Capabilities;
using GameLogic.Entities.Stats.Stat;
using GameLogic.Registry;

public static class CapabilitiesFactory
{
    public static IBounds? CreateBoundsFromData(BoundsData? bounds)
    {
        if (bounds is null)
        {
            return null;
        }

        return new BoundsCapability(bounds.LowerBound, bounds.UpperBound);
    }

    public static IMax? CreateMaxFromData(MaxData? max, BuildContext buildContext)
    {
        if (max is null)
        {
            return null;
        }

        Func<ReferenceId, bool, float> getMaxValueFunc = (
            ReferenceId maxStatId,
            bool byBaseValue
        ) =>
        {
            buildContext.Registry.TryGetReference<Stat>(
                maxStatId,
                out IReference<Stat, ReferenceSpec>? stat
            );

            if (stat is null)
            {
                throw new InvalidOperationException($"Stat not found: {maxStatId}");
            }

            if (byBaseValue == true)
            {
                return stat.GetValue().BaseValue;
            }

            return stat.GetValue().Value;
        };

        return new MaxCapability(max.MaxStatId, max.ByBaseValue, getMaxValueFunc);
    }

    public static IMutable? CreateMutableFromData(MutableData? mutableData)
    {
        if (mutableData is null)
        {
            return null;
        }

        return new MutableCapability(mutableData.Value);
    }

    public static IImmutable? CreateImmutableFromData(ImmutableSpec? immutableSpec)
    {
        if (immutableSpec is null)
        {
            return null;
        }

        return new ImmutableCapability(new StatFormula(immutableSpec, null));
    }
}
