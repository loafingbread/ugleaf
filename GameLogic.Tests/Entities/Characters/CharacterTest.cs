namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Characters;
using GameLogic.Entities.Skills;
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

        ICharacterConfig config = goblin.GetConfig();
        Assert.Equal("char_npc_goblin", config.Id);
        Assert.Equal("Goblin", config.Name);
        Assert.Equal(50, config.Health);
        Assert.Equal(20, config.Attack);
        Assert.Equal(10, config.Defense);
    }

    [Fact]
    public void Character_CanLoadWithSkillFromFile()
    {
        Character ash = CharacterFactory.CreateFromRecord(this._fixture.AshRecord);

        ICharacterConfig config = ash.GetConfig();
        Assert.Equal("char_pc_ash", config.Id);

        List<Skill> skills = config.Skills;
        Assert.Equal("skill_ignite", skills[0].Id);
        Assert.Equal(ETargetQuantity.Count, skills[0].Targeter?.GetConfig().TargetQuantity);
        Assert.Equal("usable_ignite", skills[0].Usable?.GetConfig().Id);
        Assert.Equal("effect_burn_dot", skills[0].Usable?.GetConfig().Effects[0].GetConfig().Id);
    }
}
