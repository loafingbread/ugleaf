using GdUnit4;
using static GdUnit4.Assertions;

namespace GodotDriver.Resources
{
   [TestSuite]
   public class CharacterResourceTest
   {
      [TestCase]
      public void CharacterNameIsSet()
      {
         CharacterResource character = new CharacterResource("Jack");
         AssertString(character.Name).IsEqual("Jack");
      }
   }
}