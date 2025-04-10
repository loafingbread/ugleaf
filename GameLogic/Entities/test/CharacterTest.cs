using GdUnit4;
using static GdUnit4.Assertions;

namespace GameLogic.Entities
{
   [TestSuite]
   public class CharacterTest
   {
      [TestCase]
      public void CharacterNameIsSet()
      {
         Character character = new Character("Jack");
         AssertString(character.Name).IsEqual("Jack");
      }
   }
}