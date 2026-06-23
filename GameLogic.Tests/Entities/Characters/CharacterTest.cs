namespace GameLogic.Tests;

using GameLogic.Entities.Characters;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables.Effects;
using GameLogic.Usables.Effects.Variants;
using Xunit;

public class CharacterTest : IClassFixture<CharacterTestFixture>
{
    private readonly CharacterTestFixture _fixture;
    private readonly CharacterTemplateFactory _factory = new();

    public CharacterTest(CharacterTestFixture fixture)
    {
        this._fixture = fixture;
    }

    private Character MakeCharacter(CharacterTemplateData data)
    {
        CharacterTemplate template = _factory.Create(ReferenceId.New(), data);
        return new Character(template, new CharacterInstanceData { TemplateId = template.ToData().Id });
    }

    [Fact]
    public void Character_CanLoadFromFile()
    {
        Character goblin = MakeCharacter(this._fixture.GoblinRecord);

        Assert.Equal("char_npc_goblin", goblin.Id);
        Assert.Equal("Goblin", goblin.Name);
        Assert.Equal(
            80,
            (goblin.Stats.GetStat("resource_stat_health", StatType.Resource) as ResourceStat).CurrentCapacity
        );
        Assert.Equal(20, goblin.Stats.GetStat("value_stat_strength", StatType.Value).CurrentValue);
        Assert.Equal(25, goblin.Stats.GetStat("value_stat_constitution", StatType.Value).CurrentValue);
    }

    [Fact]
    public void Character_CanLoadWithSkillFromFile()
    {
        Character ash = MakeCharacter(this._fixture.AshRecord);

        Assert.Equal("char_pc_ash", ash.Id);

        Skill firstSkill = ash.Skills[0];
        Assert.Equal("skill_ignite", firstSkill.Id);
        Assert.Equal(ETargetQuantity.Count, firstSkill.Targeter?.TargetQuantity);

        Assert.Equal("usable_ignite", firstSkill.Usables[0].ToData().Id);

        EffectTemplate burnEffect = firstSkill.Usables[0].Effects[0];
        Assert.Equal("Status", burnEffect.Type);
        Assert.Equal("Burn", burnEffect.Subtype);
        Assert.IsType<BurnStatusVariant>(burnEffect.EffectVariant);
        Assert.Equal(5.0f, burnEffect.ToData().Value);
        Assert.Equal(3.0f, burnEffect.ToData().Duration);
    }

    [Fact]
    public void Character_InstanceData_RoundTrips()
    {
        Character ash = MakeCharacter(this._fixture.AshRecord);

        CharacterInstanceData data = ash.ToData();

        Assert.Equal("char_pc_ash", data.TemplateId);
        Assert.Single(data.Skills);
        Assert.Equal("skill_ignite", data.Skills[0].TemplateId);
    }
}
