namespace GameLogic.Tests;

using GameLogic.Entities.Skills;
using GameLogic.Targeting;
using Xunit;

public class SkillTest : IClassFixture<SkillTestFixture>
{
    private readonly SkillTestFixture _fixture;

    public SkillTest(SkillTestFixture fixture)
    {
        this._fixture = fixture;
    }

    [Fact]
    public void Skill_CanLoadFromFile()
    {
        Skill facePalm = new Skill(this._fixture.FacePalmConfig);

        Assert.Equal("skill_facepalm", facePalm.Id);
        Assert.Equal("Face Palm", facePalm.Name);
        Assert.Equal(1, this._fixture.FacePalmConfig.Targeter?.Count);
        Assert.Equal(ETargetQuantity.Count, this._fixture.FacePalmConfig.Targeter?.TargetQuantity);
        Assert.Equal(
            [EFactionRelationship.Self],
            this._fixture.FacePalmConfig.Targeter?.AllowedTargets
        );
    }
}
