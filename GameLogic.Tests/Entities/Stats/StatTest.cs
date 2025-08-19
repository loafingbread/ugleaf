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
    public void Stat_CanLoadFromFile()
    {
        IStat stat = StatBlockFactory.CreateStatFromRecord(this._fixture.StrengthRecord);

        Assert.Equal("value_stat_strength", stat.GetConfig().Id);
        Assert.Equal("Strength", stat.GetConfig().DisplayName);
        Assert.Equal("Strength is a measure of your physical power.", stat.GetConfig().Description);
        Assert.Equal(["physical", "strength"], stat.GetConfig().Tags);
        Assert.Equal(StatType.Value, stat.GetConfig().Type);
        Assert.Equal(100, stat.GetConfig().ValueCap);
        Assert.Equal(StatFormulaType.Constant, stat.GetConfig().BaseValueFormula.Type);
        Assert.Equal(10, stat.GetConfig().BaseValueFormula.CalculateValue());
    }

    [Fact]
    public void StatBlock_CanLoadFromFile()
    {
        StatBlock statBlock = StatBlockFactory.CreateFromRecord(this._fixture.TestStatBlockRecord);

        IStat strengthStat = statBlock.GetStat("value_stat_strength");
        Assert.Equal("value_stat_strength", strengthStat.GetConfig().Id);
        Assert.Equal("Strength", strengthStat.GetConfig().DisplayName);
        Assert.Equal(
            "Strength is a measure of your physical power.",
            strengthStat.GetConfig().Description
        );
        Assert.Equal(["physical", "strength"], strengthStat.GetConfig().Tags);
        Assert.Equal(StatType.Value, strengthStat.GetConfig().Type);
        Assert.Equal(100, strengthStat.GetConfig().ValueCap);
        Assert.Equal(StatFormulaType.Constant, strengthStat.GetConfig().BaseValueFormula.Type);
        Assert.Equal(10, strengthStat.GetConfig().BaseValueFormula.CalculateValue());

        IStat agilityStat = statBlock.GetStat("value_stat_agility");
        Assert.Equal("value_stat_agility", agilityStat.GetConfig().Id);
        Assert.Equal("Agility", agilityStat.GetConfig().DisplayName);
        Assert.Equal(
            "Agility is a measure of your movement speed.",
            agilityStat.GetConfig().Description
        );
        Assert.Equal(["physical", "agility"], agilityStat.GetConfig().Tags);
        Assert.Equal(StatType.Value, agilityStat.GetConfig().Type);
        Assert.Equal(100, agilityStat.GetConfig().ValueCap);
        Assert.Equal(StatFormulaType.Constant, agilityStat.GetConfig().BaseValueFormula.Type);
        Assert.Equal(30, agilityStat.GetConfig().BaseValueFormula.CalculateValue());
    }
}
