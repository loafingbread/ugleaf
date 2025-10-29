namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Stats;
using GameLogic.Registry;

public class StatTestFixture
{
    public ReferenceUnionSpec ValueStatStrengthRecord { get; }
    public ReferenceUnionSpec ResourceStatHealthRecord { get; }
    public StatBlockRecord TestStatBlockRecord { get; }

    public StatTestFixture()
    {
        this.ValueStatStrengthRecord = JsonConfigLoader.LoadFromFile<ReferenceUnionSpec>(
            ConfigPaths.Stat.ValueStatStrength
        );
        this.ResourceStatHealthRecord = JsonConfigLoader.LoadFromFile<ReferenceUnionSpec>(
            ConfigPaths.Stat.ResourceStatHealth
        );
        this.TestStatBlockRecord = JsonConfigLoader.LoadFromFile<StatBlockRecord>(
            ConfigPaths.Stat.TestStatBlock
        );
    }
}
