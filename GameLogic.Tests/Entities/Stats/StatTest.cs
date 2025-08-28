namespace GameLogic.Tests;

using GameLogic.Entities.Stats;
using Xunit;

public class StatTest : IClassFixture<StatTestFixture>
{
    private readonly StatTestFixture _fixture;

    public StatTest(StatTestFixture fixture)
    {
        this._fixture = fixture;
    }

    [Fact]
    public void ValueStat_CanLoadFromFile()
    {
        Stat stat = StatFactory.CreateStatFromRecord(this._fixture.ValueStatStrengthRecord);

        Assert.IsType<ValueStat>(stat);
        Assert.Equal(StatType.Value, stat.Type);

        Assert.Equal("value_stat_strength", stat.Metadata.Name);
        Assert.Equal("Strength", stat.Metadata.DisplayName);
        Assert.Equal("Strength is a measure of your physical power.", stat.Metadata.Description);
        Assert.Equal(["physical", "strength"], stat.Metadata.Tags);

        ValueStatConfigRecord valueStatConfig = ((ValueStat)stat).GetConfig()!;
        Assert.Equal(50, valueStatConfig.BaseValueCap);
        Assert.Equal(70, valueStatConfig.CurrentValueCap);
        Assert.Equal(StatFormulaType.Constant, valueStatConfig.BaseValueFormula.Type);
        Assert.Equal(20, valueStatConfig.BaseValueFormula.CalculateValue());
    }

    [Fact]
    public void ResourceStat_CanLoadFromFile()
    {
        Stat stat = StatFactory.CreateStatFromRecord(this._fixture.ResourceStatHealthRecord);

        Assert.IsType<ResourceStat>(stat);
        Assert.Equal(StatType.Resource, stat.Type);

        Assert.Equal("resource_stat_health", stat.Metadata.Name);
        Assert.Equal("Health", stat.Metadata.DisplayName);
        Assert.Equal(
            "Health is a measure of your healthiness. You die when it reaches 0.",
            stat.Metadata.Description
        );
        Assert.Equal(["physical", "health"], stat.Metadata.Tags);

        ResourceStatConfigRecord resourceStatConfig = ((ResourceStat)stat).GetConfig()!;
        Assert.Equal(200, resourceStatConfig.BaseCapacityCap);
        Assert.Equal(300, resourceStatConfig.CurrentCapacityCap);
        Assert.Equal(80, resourceStatConfig.StartingCurrentValue);
        Assert.Equal(StatFormulaType.Constant, resourceStatConfig.BaseCapacityFormula.Type);
        Assert.Equal(100, resourceStatConfig.BaseCapacityFormula.CalculateValue());

        Assert.Equal(resourceStatConfig.StartingCurrentValue, stat.CurrentValue);
    }
}
