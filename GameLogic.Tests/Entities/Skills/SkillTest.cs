namespace GameLogic.Tests;

using System.Data;
using GameLogic.Entities.Skills;
using GameLogic.Targeting;
using GameLogic.Usables;
using GameLogic.Usables.Effects;
using Xunit;
using Xunit.Abstractions;

public class SkillTest : IClassFixture<SkillTestFixture>
{
    private readonly SkillTestFixture _fixture;
    private readonly ITestOutputHelper _output;

    public SkillTest(SkillTestFixture fixture, ITestOutputHelper output)
    {
        this._fixture = fixture;
        this._output = output;
    }

    [Fact]
    public void Skill_CanLoadFromFile()
    {
        Skill facePalm = SkillFactory.CreateFromRecord(this._fixture.FacePalmRecord);

        Assert.Equal("skill_facepalm", facePalm.Id);
        Assert.Equal("Face Palm", facePalm.Name);
        Assert.Equal(1, this._fixture.FacePalmRecord.Targeter?.Count);
        Assert.Equal(ETargetQuantity.Count, this._fixture.FacePalmRecord.Targeter?.TargetQuantity);
        Assert.Equal(
            [EFactionRelationship.Self],
            this._fixture.FacePalmRecord.Targeter?.AllowedTargets
        );
    }

    [Fact]
    public void Skill_CanLoadFullFromFile()
    {
        Skill ignite = SkillFactory.CreateFromRecord(this._fixture.IgniteRecord);

        Assert.Equal("skill_ignite", ignite.Id);
        Assert.Equal("Ignite", ignite.Name);

        UsableConfig usableConfig = (UsableConfig)ignite.Usables[0].GetConfig();
        Assert.Equal(ETargetQuantity.Count, usableConfig.Targeter?.TargetQuantity);
        Assert.Equal([EFactionRelationship.Enemy], usableConfig.Targeter?.AllowedTargets);
        Assert.Equal(1, usableConfig.Targeter?.Count);

        IEffect _effect = usableConfig.Effects[0];
        BurnStatusEffect effect = (BurnStatusEffect)_effect;
        BurnStatusEffectConfig effectConfig = (BurnStatusEffectConfig)effect.GetConfig();
        Assert.Equal("effect_burn_dot", effectConfig.Id);
        Assert.Equal("Status", effectConfig.Type);
        Assert.Equal("Burn", effectConfig.Subtype);
        Assert.Equal("DOT", effectConfig.Variant);
        Assert.Equal(3, effectConfig.Duration);
        Assert.Equal(5, effectConfig.DamagePerTurn);
    }
}
