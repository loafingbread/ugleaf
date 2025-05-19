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
        Stat stat = StatBlockFactory.CreateStatFromRecord(this._fixture.StrengthRecord);

        Assert.Equal("stat_strength", stat.GetConfig().Id);
        Assert.Equal(10, stat.GetConfig().BaseValue);
    }

    [Fact]
    public void StatBlock_CanLoadFromFile()
    {
        StatBlock statBlock = StatBlockFactory.CreateFromRecord(this._fixture.TestStatBlockRecord);

        Stat strengthStat = statBlock.Stats[0];

        Stat vitalityStat = statBlock.Stats[1];

        Assert.Equal("stat_strength", strengthStat.GetConfig().Id);
        Assert.Equal(10, strengthStat.GetConfig().BaseValue);

        Assert.Equal("stat_vitality", vitalityStat.GetConfig().Id);
        Assert.Equal(20, vitalityStat.GetConfig().BaseValue);
    }
}
