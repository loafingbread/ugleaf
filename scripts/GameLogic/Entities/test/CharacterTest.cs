using Godot;
using GdUnit4;
using static GdUnit4.Assertions;
using System;

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