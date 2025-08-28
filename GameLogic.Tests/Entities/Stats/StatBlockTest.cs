namespace GameLogic.Tests;

using GameLogic.Entities.Stats;
using Xunit;

public class StatBlockTest : IClassFixture<StatTestFixture>
{
    private readonly StatTestFixture _fixture;

    public StatBlockTest(StatTestFixture fixture)
    {
        this._fixture = fixture;
    }

    [Fact]
    public void StatBlock_CanLoadFromFile()
    {
        StatBlock statBlock = StatFactory.CreateStatBlockFromRecord(
            this._fixture.TestStatBlockRecord
        );

        Stat? strengthStat = statBlock.GetStat("value_stat_strength", StatType.Value);
        Assert.NotNull(strengthStat);
        Assert.Equal("value_stat_strength", strengthStat!.Metadata.Name);

        Stat? agilityStat = statBlock.GetStat("value_stat_agility", StatType.Value);
        Assert.NotNull(agilityStat);
        Assert.Equal("value_stat_agility", agilityStat!.Metadata.Name);

        Stat? healthStat = statBlock.GetStat("resource_stat_health", StatType.Resource);
        Assert.NotNull(healthStat);
        Assert.Equal("resource_stat_health", healthStat!.Metadata.Name);
    }

    [Fact]
    public void StatBlock_GetStatByNameAndType()
    {
        StatBlock statBlock = StatFactory.CreateStatBlockFromRecord(
            this._fixture.TestStatBlockRecord
        );

        Stat? strengthStatIsValueStat = statBlock.GetStat("value_stat_strength", StatType.Value);
        Assert.NotNull(strengthStatIsValueStat);
        Assert.Equal("value_stat_strength", strengthStatIsValueStat!.Metadata.Name);

        Stat? strengthStatIsNotResourceStat = statBlock.GetStat(
            "value_stat_strength",
            StatType.Resource
        );
        Assert.Null(strengthStatIsNotResourceStat);

        Stat? healthStatIsResourceStat = statBlock.GetStat(
            "resource_stat_health",
            StatType.Resource
        );
        Assert.NotNull(healthStatIsResourceStat);
        Assert.Equal("resource_stat_health", healthStatIsResourceStat!.Metadata.Name);
    }

    [Fact]
    public void StatBlock_SetResourceStat()
    {
        StatBlock statBlock = StatFactory.CreateStatBlockFromRecord(
            this._fixture.TestStatBlockRecord
        );

        ResourceStat? healthStat =
            statBlock.GetStat("resource_stat_health", StatType.Resource) as ResourceStat;
        Assert.NotNull(healthStat);
        Assert.Equal(80, healthStat!.CurrentValue);

        ResourceStat? healthStatAfterSet = statBlock.SetResourceStat("resource_stat_health", 50);
        Assert.NotNull(healthStatAfterSet);
        Assert.Equal(50, healthStatAfterSet!.CurrentValue);

        ResourceStat? healthStatAfterMaximumSet = statBlock.SetResourceStat(
            "resource_stat_health",
            9999999
        );
        Assert.NotNull(healthStatAfterMaximumSet);
        Assert.Equal(100, healthStatAfterMaximumSet!.CurrentValue);
    }

    [Fact]
    public void StatBlock_ModifyResourceStat()
    {
        StatBlock statBlock = StatFactory.CreateStatBlockFromRecord(
            this._fixture.TestStatBlockRecord
        );

        ResourceStat? healthStat =
            statBlock.GetStat("resource_stat_health", StatType.Resource) as ResourceStat;
        Assert.NotNull(healthStat);
        Assert.Equal(80, healthStat!.CurrentValue);

        ResourceStat? healthStatAfterModify = statBlock.ModifyResourceStat(
            "resource_stat_health",
            10
        );
        Assert.NotNull(healthStatAfterModify);
        Assert.Equal(90, healthStatAfterModify!.CurrentValue);

        ResourceStat? healthStatAfterMaximumModify = statBlock.ModifyResourceStat(
            "resource_stat_health",
            9999999
        );
        Assert.NotNull(healthStatAfterMaximumModify);
        Assert.Equal(100, healthStatAfterMaximumModify!.CurrentValue);
    }
}
