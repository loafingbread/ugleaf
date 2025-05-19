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
}