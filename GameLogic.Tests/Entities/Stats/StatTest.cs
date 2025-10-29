namespace GameLogic.Tests;

using GameLogic.Entities.Stats;
using GameLogic.Registry;
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
        Reference<Stat, Stat> valueStatStrengthReference =
            StatFactory.CreateStatReferenceFromRecord(this._fixture.ValueStatStrengthRecord);
        if (valueStatStrengthReference.Template is null)
        {
            throw new InvalidOperationException("Value Stat Strength template is null");
        }

        Stat valueStatStrength = valueStatStrengthReference.Template;

        Assert.IsType<ValueStat>(valueStatStrength);
        Assert.Equal(StatType.Value, valueStatStrength.Type);

        Assert.Equal("value_stat_strength", valueStatStrength.Metadata.Name);
        Assert.Equal("Strength", valueStatStrength.Metadata.DisplayName);
        Assert.Equal(
            "Strength is a measure of your physical power.",
            valueStatStrength.Metadata.Description
        );
        Assert.Equal(["physical", "strength"], valueStatStrength.Metadata.Tags);

        ValueStatConfigRecord valueStatConfig = ((ValueStat)valueStatStrength).GetConfig()!;
        Assert.Equal(50, valueStatConfig.BaseValueCap);
        Assert.Equal(70, valueStatConfig.CurrentValueCap);
        Assert.Equal(StatFormulaType.Constant, valueStatConfig.BaseValueFormula.Type);
        Assert.Equal(20, valueStatConfig.BaseValueFormula.CalculateValue());
    }

    [Fact]
    public void ResourceStat_CanLoadFromFile()
    {
        Reference<Stat, Stat> resourceStatHealthReference =
            StatFactory.CreateStatReferenceFromRecord(this._fixture.ResourceStatHealthRecord);
        if (resourceStatHealthReference.Template is null)
        {
            throw new InvalidOperationException("Resource Stat Health template is null");
        }

        Stat resourceStatHealth = resourceStatHealthReference.Template;

        Assert.IsType<ResourceStat>(resourceStatHealth);
        Assert.Equal(StatType.Resource, resourceStatHealth.Type);

        Assert.Equal("resource_stat_health", resourceStatHealth.Metadata.Name);
        Assert.Equal("Health", resourceStatHealth.Metadata.DisplayName);
        Assert.Equal(
            "Health is a measure of your healthiness. You die when it reaches 0.",
            resourceStatHealth.Metadata.Description
        );
        Assert.Equal(["physical", "health"], resourceStatHealth.Metadata.Tags);

        ResourceStatConfigRecord resourceStatConfig = (
            (ResourceStat)resourceStatHealth
        ).GetConfig()!;
        Assert.Equal(200, resourceStatConfig.BaseCapacityCap);
        Assert.Equal(300, resourceStatConfig.CurrentCapacityCap);
        Assert.Equal(80, resourceStatConfig.StartingCurrentValue);
        Assert.Equal(StatFormulaType.Constant, resourceStatConfig.BaseCapacityFormula.Type);
        Assert.Equal(100, resourceStatConfig.BaseCapacityFormula.CalculateValue());

        Assert.Equal(resourceStatConfig.StartingCurrentValue, resourceStatHealth.CurrentValue);
    }
}
