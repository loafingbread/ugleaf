using GameLogic.Entities;
using GameLogic.Combat.TurnBased;
using Xunit;

namespace GameLogic.TurnBasedTests
{
    class TurnQueueTestData
    {
        public List<Character> Players;
        public List<Character> Enemies;

        public TurnQueueTestData()
        {
            this.Players = new List<Character> { new Character("Ash"), new Character("Brock") };
            this.Enemies = new List<Character> { new Character("Missy") };
        }
    }

    public class TurnQueueTest
    {
        [Fact]
        public void GetCurrentTurn_ReturnsFirstCharacter()
        {
            TurnQueueTestData td = new TurnQueueTestData();
            TurnQueue sut = new TurnQueue(td.Players, td.Enemies);

            Character currentTurn = sut.GetCurrentTurn();

            Assert.Equal("Ash", currentTurn.Name);
        }

        [Fact]
        public void GetAlliesForCharacter_ReturnsPlayersForPlayer()
        {
            TurnQueueTestData td = new TurnQueueTestData();
            TurnQueue sut = new TurnQueue(td.Players, td.Enemies);

            List<Character> allies = sut.GetAlliesForCharacter(td.Players[0]);

            Assert.Equal(allies, td.Players);
        }

        [Fact]
        public void GetAlliesForCharacter_ReturnsEnemiesForEnemy()
        {
            TurnQueueTestData td = new TurnQueueTestData();
            TurnQueue sut = new TurnQueue(td.Players, td.Enemies);

            List<Character> allies = sut.GetAlliesForCharacter(td.Enemies[0]);

            Assert.Equal(allies, td.Enemies);
        }

        [Fact]
        public void GetEnemiesForCharacter_ReturnsEnemiesForPlayer()
        {
            TurnQueueTestData td = new TurnQueueTestData();
            TurnQueue sut = new TurnQueue(td.Players, td.Enemies);

            List<Character> allies = sut.GetEnemiesForCharacter(td.Players[0]);

            Assert.Equal(allies, td.Enemies);
        }

        [Fact]
        public void GetEnemiesForCharacter_ReturnsPlayersForEnemy()
        {
            TurnQueueTestData td = new TurnQueueTestData();
            TurnQueue sut = new TurnQueue(td.Players, td.Enemies);

            List<Character> allies = sut.GetEnemiesForCharacter(td.Enemies[0]);

            Assert.Equal(allies, td.Players);
        }
    }
}
