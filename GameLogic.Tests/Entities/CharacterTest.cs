namespace GameLogic.EntitiesTests;

using GameLogic.Entities;
using Xunit;

public class CharacterTest
{
    [Fact]
    public void CharacterNameIsSet()
    {
        Character character = new Character("Jack");
        Assert.Equal("Jack", character.Name);
    }
}
