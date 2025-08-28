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
}
