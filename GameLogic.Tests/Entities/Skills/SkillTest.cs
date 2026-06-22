namespace GameLogic.Tests;

using GameLogic.Entities.Skills;
using GameLogic.Targeting;
using GameLogic.Usables;
using GameLogic.Usables.Effects;
using GameLogic.Usables.Effects.Variants;
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

        UsableTemplate usable = ignite.Usables[0];
        Assert.Equal(ETargetQuantity.Count, usable.Targeter.TargetQuantity);
        Assert.Equal([EFactionRelationship.Enemy], usable.Targeter.AllowedTargets);
        Assert.Equal(1, usable.Targeter.Count);

        EffectTemplate burnEffect = usable.Effects[1];
        Assert.Equal("Status", burnEffect.Type);
        Assert.Equal("Burn", burnEffect.Subtype);
        Assert.Equal("DOT", burnEffect.VariantName);
        Assert.IsType<BurnStatusVariant>(burnEffect.EffectVariant);

        EffectTemplateData burnData = burnEffect.ToData();
        Assert.Equal(5.0f, burnData.Value);
        Assert.Equal(3.0f, burnData.Duration);
    }
}
