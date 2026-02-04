namespace GameLogic.Entities.Stats;

using GameLogic.Registry;

public static class StatFactory
{
    public static StatModel CreateStatModelFromData(StatData data, BuildContext buildContext)
    {
        IBounds? bounds = CreateBoundsFromData(data.Capabilities.Bounds);
        IMaxStat? max = CreateMaxStatFromData(data.Capabilities.Max, buildContext);
        IMutable? mutableValue = CreateMutableFromData(data.Capabilities.MutableValue);
        IImmutable? immutableValue = CreateImmutableFromData(data.Capabilities.ImmutableValue);

        return new StatModel(bounds, max, mutableValue, immutableValue);
    }

    public static BoundsCapability? CreateBoundsFromData(BoundsData? bounds)
    {
        if (bounds is null)
        {
            return null;
        }

        return new BoundsCapability(bounds.LowerBound, bounds.UpperBound);
    }

    public static MaxCapability? CreateMaxStatFromData(MaxData? max, BuildContext buildContext)
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

    public static MutableCapability? CreateMutableFromData(MutableData? mutableData)
    {
        if (mutableData is null)
        {
            return null;
        }

        return new MutableCapability(mutableData.Value);
    }

    public static ImmutableCapability? CreateImmutableFromData(ImmutableData? immutableData)
    {
        if (immutableData is null)
        {
            return null;
        }

        return new ImmutableCapability(new StatFormula(immutableData));
    }

    public static Stat CreateStatFromData(StatData data, BuildContext buildContext)
    {
        return new Stat(data.ReferenceId, StatFactory.CreateStatModelFromData(data, buildContext));
    }

    public static IStatConfigData CopyStatConfig(IStatConfigData record)
    {
        switch (record)
        {
            case ValueStatConfigData:
                var valueConfigRecord = record as ValueStatConfigData;
                if (valueConfigRecord is null)
                {
                    throw new ArgumentException($"Invalid stat config type: {record.GetType()}");
                }

                return new ValueStatConfigData
                {
                    BaseValueCap = valueConfigRecord.BaseValueCap,
                    CurrentValueCap = valueConfigRecord.CurrentValueCap,
                    BaseValueFormula = valueConfigRecord.BaseValueFormula,
                };
            case ResourceStatConfigData:
                var resourceConfigRecord = record as ResourceStatConfigData;
                if (resourceConfigRecord is null)
                {
                    throw new ArgumentException($"Invalid stat config type: {record.GetType()}");
                }

                return new ResourceStatConfigData
                {
                    BaseCapacityCap = resourceConfigRecord.BaseCapacityCap,
                    CurrentCapacityCap = resourceConfigRecord.CurrentCapacityCap,
                    BaseCapacityFormula = resourceConfigRecord.BaseCapacityFormula,
                    StartingCurrentValue = resourceConfigRecord.StartingCurrentValue,
                };
        }

        throw new ArgumentException($"Invalid stat config type: {record.GetType()}");
    }
}
