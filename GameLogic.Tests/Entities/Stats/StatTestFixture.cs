namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Stats;
using GameLogic.Registry;

public class StatTestFixture
{
    public ReferenceSpec ValueStatStrengthRecord { get; }
    public ReferenceSpec ResourceStatHealthRecord { get; }
    public StatBlockSpec TestStatBlockSpec { get; }

    public StatTestFixture()
    {
        this.ValueStatStrengthRecord = JsonConfigLoader.LoadFromFile<ReferenceSpec>(
            ConfigPaths.Stat.ValueStatStrength
        );
        this.ResourceStatHealthRecord = JsonConfigLoader.LoadFromFile<ReferenceSpec>(
            ConfigPaths.Stat.ResourceStatHealth
        );
        this.TestStatBlockSpec = JsonConfigLoader.LoadFromFile<StatBlockSpec>(
            ConfigPaths.Stat.TestStatBlock
        );
    }
}
