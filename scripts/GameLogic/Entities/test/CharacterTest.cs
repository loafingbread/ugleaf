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
      public void StringToLower()
      {
         AssertString("AbcD".ToLower()).IsEqual("abcd");
      }
   }
}