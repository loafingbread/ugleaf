namespace GameLogic.Entities.Stats;

public static class StatFactory
{
    public static Stat CreateStatFromRecord(StatRecord record)
    {
        switch (record.Type)
        {
            case StatType.Value:
                return new ValueStat(record);
            case StatType.Resource:
                return new ResourceStat(record);
            default:
                throw new ArgumentException($"Invalid stat type: {record.Type}");
        }
    }

    public static StatBlock CreateStatBlockFromRecord(IStatBlockRecord record)
    {
        return new StatBlock(record);
    }

    public static IStatConfigRecord CopyStatConfig(IStatConfigRecord record)
    {
        switch (record)
        {
            case ValueStatConfigRecord:
                var valueConfigRecord = record as ValueStatConfigRecord;
                if (valueConfigRecord is null)
                {
                    throw new ArgumentException($"Invalid stat config type: {record.GetType()}");
                }

                return new ValueStatConfigRecord
                {
                    BaseValueCap = valueConfigRecord.BaseValueCap,
                    CurrentValueCap = valueConfigRecord.CurrentValueCap,
                    BaseValueFormula = valueConfigRecord.BaseValueFormula,
                };
            case ResourceStatConfigRecord:
                var resourceConfigRecord = record as ResourceStatConfigRecord;
                if (resourceConfigRecord is null)
                {
                    throw new ArgumentException($"Invalid stat config type: {record.GetType()}");
                }

                return new ResourceStatConfigRecord
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
