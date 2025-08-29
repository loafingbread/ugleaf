namespace GameLogic.Tests;

using GameLogic.Entities.Characters;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Targeting;
using Xunit;

public class CharacterTest : IClassFixture<CharacterTestFixture>
{
    private readonly CharacterTestFixture _fixture;

    public CharacterTest(CharacterTestFixture fixture)
    {
        this._fixture = fixture;
    }

    [Fact]
    public void Character_CanLoadFromFile()
    {
        Character goblin = CharacterFactory.CreateFromRecord(this._fixture.GoblinRecord);

        CharacterConfig config = goblin.GetConfig();
        Assert.Equal("char_npc_goblin", config.Id);
        Assert.Equal("Goblin", config.Name);
        Assert.Equal(
            80,
            (
                goblin.Stats.GetStat("resource_stat_health", StatType.Resource) as ResourceStat
            ).CurrentCapacity
        );
        Assert.Equal(20, goblin.Stats.GetStat("value_stat_strength", StatType.Value).CurrentValue);
        Assert.Equal(
            10,
            goblin.Stats.GetStat("value_stat_constitution", StatType.Value).CurrentValue
        );
    }

    [Fact]
    public void Character_CanLoadWithSkillFromFile()
    {
        Character ash = CharacterFactory.CreateFromRecord(this._fixture.AshRecord);

        CharacterConfig config = ash.GetConfig();
        Assert.Equal("char_pc_ash", config.Id);

        Skill firstSkill = config.Skills[0];
        Assert.Equal("skill_ignite", firstSkill.Id);

        Assert.Equal(ETargetQuantity.Count, firstSkill.Targeter?.GetConfig().TargetQuantity);

        Assert.Equal("usable_ignite", firstSkill.Usables[0].GetConfig().Id);
        Assert.Equal(
            "effect_burn_dot",
            firstSkill.Usables[0].GetConfig().Effects[0].GetConfig().Id
        );
    }
}
