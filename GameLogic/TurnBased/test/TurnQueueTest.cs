using GdUnit4;
using static GdUnit4.Assertions;
using GameLogic.Entities;
using System.Collections.Generic;

namespace GameLogic.TurnBased
{
    class TurnQueueTestData
    {
        public List<Character> Players;
        public List<Character> Enemies;

        public TurnQueueTestData()
        {
            this.Players = new List<Character>{
                new Character("Ash"),
                new Character("Brock"),
            };
            this.Enemies = new List<Character>{
                new Character("Missy"),
            };
        }
    }

    [TestSuite]
    public partial class TurnQueueTest
    {
        [TestCase]
        public void GetCurrentTurn_ReturnsFirstCharacter()
        {
            TurnQueueTestData td = new TurnQueueTestData();
            TurnQueue sut = new TurnQueue(td.Players, td.Enemies);

            Character currentTurn = sut.GetCurrentTurn();

            AssertString(currentTurn.Name).IsEqual("Ash");
        }

        [TestCase]
        public void GetAlliesForCharacter_ReturnsPlayersForPlayer()
        {
            TurnQueueTestData td = new TurnQueueTestData();
            TurnQueue sut = new TurnQueue(td.Players, td.Enemies);

            List<Character> allies = sut.GetAlliesForCharacter(td.Players[0]);

            AssertThat(allies).IsEqual(td.Players);
        }

        [TestCase]
        public void GetAlliesForCharacter_ReturnsEnemiesForEnemy()
        {
            TurnQueueTestData td = new TurnQueueTestData();
            TurnQueue sut = new TurnQueue(td.Players, td.Enemies);

            List<Character> allies = sut.GetAlliesForCharacter(td.Enemies[0]);

            AssertThat(allies).IsEqual(td.Enemies);
        }

        [TestCase]
        public void GetEnemiesForCharacter_ReturnsEnemiesForPlayer()
        {
            TurnQueueTestData td = new TurnQueueTestData();
            TurnQueue sut = new TurnQueue(td.Players, td.Enemies);

            List<Character> allies = sut.GetEnemiesForCharacter(td.Players[0]);

            AssertThat(allies).IsEqual(td.Enemies);
        }

        [TestCase]
        public void GetEnemiesForCharacter_ReturnsPlayersForEnemy()
        {
            TurnQueueTestData td = new TurnQueueTestData();
            TurnQueue sut = new TurnQueue(td.Players, td.Enemies);

            List<Character> allies = sut.GetEnemiesForCharacter(td.Enemies[0]);

            AssertThat(allies).IsEqual(td.Players);
        }
    }
}