namespace GameLogic.Entities.Stats;

using GameLogic.Registry;

public static class StatFactory
{
    public static StatModel CreateStatModelFromSpec(StatSpec spec)
    {
        IHasBounds? bounds = null;
        if (spec.Capabilities.HasBounds)
        {
            bounds = new BoundsData(spec.Capabilities.Bounds.LowerBound, spec.Capabilities.Bounds.UpperBound);
        }
    }

    public static Stat CreateStatFromData(StatData data)
    {
        return new Stat(data.ReferenceId);
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
