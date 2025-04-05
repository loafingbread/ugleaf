using GdUnit4;
using static GdUnit4.Assertions;
using GameLogic.Entities;
using System.Collections.Generic;

namespace GameLogic.TurnBased
{
    [TestSuite]
    public partial class TurnQueueTest
    {
        [TestCase]
        public void GetCurrentTurn_ReturnsFirstCharacter()
        {
            List<Character> players = new List<Character>
            {
                new Character("Ash"),
                new Character("Brock"),
            };
            List<Character> enemies = new List<Character>{
                new Character("Missy"),
            };

            TurnQueue sut = new TurnQueue(players, enemies);

            Character currentTurn = sut.GetCurrentTurn();

            AssertString(currentTurn.Name).IsEqual("Ash");
        }
    }
}