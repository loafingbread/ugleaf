namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Stats;

public class StatTestFixture
{
    public StatRecord ValueStatStrengthRecord { get; }
    public StatRecord ResourceStatHealthRecord { get; }
    public StatBlockRecord TestStatBlockRecord { get; }

    public StatTestFixture()
    {
        this.ValueStatStrengthRecord = JsonConfigLoader.LoadFromFile<StatRecord>(
            ConfigPaths.Stat.ValueStatStrength
        );
        this.ResourceStatHealthRecord = JsonConfigLoader.LoadFromFile<StatRecord>(
            ConfigPaths.Stat.ResourceStatHealth
        );
        this.TestStatBlockRecord = JsonConfigLoader.LoadFromFile<StatBlockRecord>(
            ConfigPaths.Stat.TestStatBlock
        );
    }
}
