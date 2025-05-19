namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Stats;

public class StatTestFixture
{
    public StatRecord StrengthRecord { get; }

    public StatTestFixture()
    {
        this.StrengthRecord = JsonConfigLoader.LoadFromFile<StatRecord>(ConfigPaths.Stat.Strength);
    }
}
