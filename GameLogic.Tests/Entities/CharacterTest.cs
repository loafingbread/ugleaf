using GameLogic.Entities;
using Xunit;

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
