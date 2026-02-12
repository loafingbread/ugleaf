namespace GameLogic.Entities.Stats.Factories;

using GameLogic.Entities.Stats.Stat;
using GameLogic.Entities.Stats.StatBlock;
using GameLogic.Registry;

public static class StatBlockFactory
{
    public static StatBlock ReferenceToStatBlock(
        StatBlockSpec data,
        Func<ReferenceId?, StatData> getStatData,
        BuildContext buildContext
    )
    {
        List<StatData> statsData = ReferenceToStatsData(data, getStatData);
        return StatsDataToStatBlock(statsData, buildContext);
    }

    public static List<StatData> ReferenceToStatsData(
        StatBlockSpec data,
        Func<ReferenceId?, StatData> getStatData
    )
    {
        List<StatData> statsData = data
            .Stats.Select((ReferenceSpec statSpec) => StatResolver.ToData(statSpec, getStatData))
            .ToList();

        return statsData;
    }

    public static StatBlock StatsDataToStatBlock(
        List<StatData> statsData,
        BuildContext buildContext
    )
    {
        List<Stat> stats = statsData
            .Select(statData => StatFactory.CreateStatFromData(statData, buildContext))
            .ToList();
        return new StatBlock(stats);
    }
}
