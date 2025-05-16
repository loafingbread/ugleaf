namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Characters;
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
}
