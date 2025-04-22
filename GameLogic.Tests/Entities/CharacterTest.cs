namespace GameLogic.EntitiesTests;

using GameLogic.Config;
using GameLogic.Entities;
using GameLogic.EntitiesTest;
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
        Character goblin = GameFactory.CreateFromConfig<Character, ICharacterData>(
            this._fixture.GoblinConfig
        );

        Assert.Equal("char_npc_goblin", goblin.Id);
        Assert.Equal("Goblin", goblin.Name);
        Assert.Equal(50, goblin.Health);
        Assert.Equal(20, goblin.Attack);
        Assert.Equal(10, goblin.Defense);
    }
}
