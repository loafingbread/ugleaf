namespace GameLogic.Tests.Registry;

using GameLogic.Config;
using GameLogic.Entities.Stats;
using GameLogic.Registry;
using Xunit;

public class GameRegistryStatTest
{
    private readonly GameRegistry _registry = new();

    [Fact]
    public void StatBlock_LoadRegisterRetrieve()
    {
        StatBlockRecord record = JsonConfigLoader.LoadFromFile<StatBlockRecord>(
            ConfigPaths.Stat.TestStatBlock
        );
        StatBlock statBlock = StatFactory.CreateStatBlockFromRecord(record);
        ReferenceId id = ReferenceId.From("test-stat-block");

        bool added = _registry.StatBlocks.TryAdd(id, statBlock);
        bool found = _registry.StatBlocks.TryGet(id, out StatBlock? retrieved);

        Assert.True(added);
        Assert.True(found);
        Assert.NotNull(retrieved);
        Assert.NotNull(retrieved!.GetStat("value_stat_strength", StatType.Value));
        Assert.NotNull(retrieved!.GetStat("resource_stat_health", StatType.Resource));
    }

    [Fact]
    public void StatBlock_ToData_RoundTrips()
    {
        StatBlockRecord original = JsonConfigLoader.LoadFromFile<StatBlockRecord>(
            ConfigPaths.Stat.TestStatBlock
        );
        StatBlock statBlock = StatFactory.CreateStatBlockFromRecord(original);

        StatBlockRecord roundTripped = statBlock.ToData();

        Assert.Equal(original.Stats.Count, roundTripped.Stats.Count);

        for (int i = 0; i < original.Stats.Count; i++)
        {
            Assert.Equal(original.Stats[i].Metadata.Name, roundTripped.Stats[i].Metadata.Name);
            Assert.Equal(original.Stats[i].Type, roundTripped.Stats[i].Type);
        }
    }

    [Fact]
    public void Stat_ToData_PreservesMetadataAndConfig()
    {
        StatRecord original = JsonConfigLoader.LoadFromFile<StatRecord>(
            ConfigPaths.Stat.ValueStatStrength
        );
        Stat stat = StatFactory.CreateStatFromRecord(original);

        StatRecord roundTripped = stat.ToData();

        Assert.Equal(original.Metadata.Name, roundTripped.Metadata.Name);
        Assert.Equal(original.Metadata.DisplayName, roundTripped.Metadata.DisplayName);
        Assert.Equal(original.Type, roundTripped.Type);
        Assert.IsType<ValueStatConfigRecord>(roundTripped.Config);
        Assert.Equal(
            ((ValueStatConfigRecord)original.Config).BaseValueCap,
            ((ValueStatConfigRecord)roundTripped.Config).BaseValueCap
        );
    }
}
