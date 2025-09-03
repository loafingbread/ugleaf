namespace GameLogic.Tests;

using GameLogic.Entities.Characters;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Targeting;
using GameLogic.Usables;
using GameLogic.Usables.Effects;
using Xunit;

public class CharacterTemplateTest : IClassFixture<CharacterTestFixture>
{
    private readonly CharacterTestFixture _fixture;

    public CharacterTemplateTest(CharacterTestFixture fixture)
    {
        this._fixture = fixture;
    }

    [Fact]
    public void CharacterTemplate_CanLoadFromFile()
    {
        CharacterTemplate goblin = CharacterFactory.CreateCharacterTemplateFromRecord(
            this._fixture.GoblinRecord
        );

        Assert.Equal("char_npc_goblin", goblin.TemplateId.ToString());
        Assert.Equal("Goblin", goblin.Name);

        ResourceStat? healthStat =
            goblin.Stats.GetStat("resource_stat_health", StatType.Resource) as ResourceStat;
        Assert.NotNull(healthStat);
        Assert.Equal(80, (healthStat).CurrentCapacity);

        ValueStat? strengthStat =
            goblin.Stats.GetStat("value_stat_strength", StatType.Value) as ValueStat;
        Assert.NotNull(strengthStat);
        Assert.Equal(20, strengthStat.CurrentValue);

        ValueStat? constitutionStat =
            goblin.Stats.GetStat("value_stat_constitution", StatType.Value) as ValueStat;
        Assert.NotNull(constitutionStat);
        Assert.Equal(25, constitutionStat.CurrentValue);
    }

    [Fact]
    public void Character_CanLoadWithSkillFromFile()
    {
        CharacterTemplate ash = CharacterFactory.CreateCharacterTemplateFromRecord(
            this._fixture.AshRecord
        );

        Assert.Equal("char_pc_ash", ash.TemplateId.ToString());
        Assert.Equal("Ash", ash.Name);

        // Skill firstSkill = ash.Skills[0];
        // Assert.Equal("skill_ignite", firstSkill.InstanceId.ToString());

        // Targeter? firstSkillTargeter = firstSkill.Targeter as Targeter;
        // Assert.NotNull(firstSkillTargeter);
        // Assert.Equal(ETargetQuantity.Count, firstSkillTargeter.TargetQuantity);

        // Usable? igniteUsable = firstSkill.Usables[0] as Usable;
        // Assert.NotNull(igniteUsable);
        // Assert.Equal("usable_ignite", igniteUsable.InstanceId.ToString());

        // Effect? burnDotEffect = igniteUsable.Effects[0] as Effect;
        // Assert.NotNull(burnDotEffect);
        // Assert.Equal("effect_burn_dot", burnDotEffect.TemplateId.ToString());
    }
}
