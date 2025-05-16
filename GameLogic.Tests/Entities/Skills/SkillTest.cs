namespace GameLogic.Tests;

using System.Data;
using GameLobic.Usables;
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

    [Fact]
    public void Skill_CanLoadFullFromFile()
    {
        Skill ignite = new Skill(this._fixture.IgniteConfig);

        if (ignite.Usable?.GetConfig() == null)
        {
            throw new NoNullAllowedException("Config should not be null!");
        }

        Assert.Equal("skill_ignite", ignite.Id);
        Assert.Equal("Ignite", ignite.Name);

        UsableConfig usableConfig = ignite.Usable.GetConfig();
        Assert.Equal(ETargetQuantity.Count, usableConfig.Targeter?.TargetQuantity);
        Assert.Equal([EFactionRelationship.Enemy], usableConfig.Targeter?.AllowedTargets);
        Assert.Equal(1, usableConfig.Targeter?.Count);

        EffectConfig effectConfig = usableConfig.Effects[0];
        Assert.Equal("effect_burn_dot", effectConfig.Id);
        Assert.Equal("Status", effectConfig.Type);
        Assert.Equal("Burn", effectConfig.Subtype);
        Assert.Equal("DOT", effectConfig.Variant);

        // TODO: Assert on specific effects values
    }
}
