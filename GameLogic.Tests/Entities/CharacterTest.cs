using Xunit;
using GameLogic.Entities;

namespace GameLogic.EntitiesTests
{
    public class CharacterTest
    {
        [Fact]
        public void CharacterNameIsSet()
        {
            Character character = new Character("Jack");
            Assert.Equal("Jack", character.Name);
        }
    }
}

