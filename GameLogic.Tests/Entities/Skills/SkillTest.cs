namespace GameLogic.Tests;

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
        Skill facePalm = SkillFactory
            .CreateSkillTemplateFromRecord(this._fixture.FacePalmRecord)
            .Instantiate();

        Assert.Equal("skill_facepalm", facePalm.TemplateId.ToString());
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
        Skill ignite = SkillFactory
            .CreateSkillTemplateFromRecord(this._fixture.IgniteRecord)
            .Instantiate();

        Assert.Equal("skill_ignite", ignite.TemplateId.ToString());
        Assert.Equal("Ignite", ignite.Name);

        Usable? igniteUsable = ignite.Usables[0] as Usable;
        Assert.NotNull(igniteUsable);

        Targeter? igniteUsableTargeter = igniteUsable.Targeter as Targeter;
        Assert.NotNull(igniteUsableTargeter);
        Assert.Equal(ETargetQuantity.Count, igniteUsableTargeter.TargetQuantity);
        Assert.Equal([EFactionRelationship.Enemy], igniteUsableTargeter.AllowedTargets);
        Assert.Equal(1, igniteUsableTargeter.Count);

        IEffect secondEffect = igniteUsable.Effects[1];

        BurnStatusEffect? burnEffect = secondEffect as BurnStatusEffect;
        Assert.NotNull(burnEffect);
        Assert.Equal("effect_burn_dot", burnEffect.TemplateId.ToString());
        Assert.Equal(EEffectType.Status, burnEffect.Type);
        Assert.Equal("Burn", burnEffect.Subtype);
        Assert.Equal("DOT", burnEffect.Variant);
        Assert.Equal(3, burnEffect.Duration);
        Assert.Equal(5, burnEffect.Value);
    }
}
