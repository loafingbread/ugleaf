namespace GameLogic.TurnBasedTests;

using GameLogic.Combat.TurnBased;
using GameLogic.Config;
using GameLogic.Entities.Characters;
using GameLogic.Tests;
using Xunit;

class TurnQueueTestData
{
    private readonly CharacterTestFixture _characters;

    public List<Character> Players;
    public List<Character> Enemies;

    public TurnQueueTestData(CharacterTestFixture characters)
    {
        this._characters = characters;
        this.Players =
        [
            CharacterFactory
                .CreateCharacterTemplateFromRecord(this._characters.AshRecord)
                .Instantiate(),
            CharacterFactory
                .CreateCharacterTemplateFromRecord(this._characters.BrockRecord)
                .Instantiate(),
            CharacterFactory
                .CreateCharacterTemplateFromRecord(this._characters.MissyRecord)
                .Instantiate(),
        ];
        this.Enemies =
        [
            CharacterFactory
                .CreateCharacterTemplateFromRecord(this._characters.MissyRecord)
                .Instantiate(),
        ];
    }
}

public class TurnQueueTest : IClassFixture<CharacterTestFixture>
{
    private readonly CharacterTestFixture _characters;

    public TurnQueueTest(CharacterTestFixture characters)
    {
        this._characters = characters;
    }

    [Fact]
    public void GetCurrentTurn_ReturnsFirstCharacter()
    {
        TurnQueueTestData td = new TurnQueueTestData(this._characters);
        TurnQueue sut = new TurnQueue(td.Players, td.Enemies);

        Character currentTurn = sut.GetCurrentTurn();

        Assert.Equal("Ash", currentTurn.Name);
    }

    [Fact]
    public void GetAlliesForCharacter_ReturnsPlayersForPlayer()
    {
        TurnQueueTestData td = new TurnQueueTestData(this._characters);
        TurnQueue sut = new TurnQueue(td.Players, td.Enemies);

        List<Character> allies = sut.GetAlliesForCharacter(td.Players[0]);

        Assert.Equal(allies, td.Players);
    }

    [Fact]
    public void GetAlliesForCharacter_ReturnsEnemiesForEnemy()
    {
        TurnQueueTestData td = new TurnQueueTestData(this._characters);
        TurnQueue sut = new TurnQueue(td.Players, td.Enemies);

        List<Character> allies = sut.GetAlliesForCharacter(td.Enemies[0]);

        Assert.Equal(allies, td.Enemies);
    }

    [Fact]
    public void GetEnemiesForCharacter_ReturnsEnemiesForPlayer()
    {
        TurnQueueTestData td = new TurnQueueTestData(this._characters);
        TurnQueue sut = new TurnQueue(td.Players, td.Enemies);

        List<Character> allies = sut.GetEnemiesForCharacter(td.Players[0]);

        Assert.Equal(allies, td.Enemies);
    }

    [Fact]
    public void GetEnemiesForCharacter_ReturnsPlayersForEnemy()
    {
        TurnQueueTestData td = new TurnQueueTestData(this._characters);
        TurnQueue sut = new TurnQueue(td.Players, td.Enemies);

        List<Character> allies = sut.GetEnemiesForCharacter(td.Enemies[0]);

        Assert.Equal(allies, td.Players);
    }
}
