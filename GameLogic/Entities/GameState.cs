namespace GameLogic.Entities;

using GameLogic.Combat.TurnBased;
using GameLogic.Entities.Characters;
using GameLogic.Registry;

public class GameState
{
    public PlayerState PlayerState { get; private set; }
    public EnemyState EnemyState { get; private set; }
    public Combat? ActiveCombat { get; set; }

    public GameState(GameStateRecord gameStateRecord)
    {
        this.PlayerState = new PlayerState(gameStateRecord.PlayerState);
        this.EnemyState = new EnemyState(gameStateRecord.EnemyState);
    }
}

public record GameStateRecord
{
    public PlayerStateRecord PlayerState { get; init; } = new();
    public EnemyStateRecord EnemyState { get; init; } = new();
}

public record PlayerState
{
    public List<Character> Characters { get; init; } = new();

    private static readonly CharacterTemplateFactory _characterFactory = new();

    public PlayerState(PlayerStateRecord playerStateRecord)
    {
        foreach (CharacterTemplateData data in playerStateRecord.Characters)
        {
            CharacterTemplate template = _characterFactory.Create(ReferenceId.New(), data);
            Characters.Add(new Character(template, new CharacterInstanceData { TemplateId = data.Id }));
        }
    }
}

public record PlayerStateRecord
{
    public List<CharacterTemplateData> Characters { get; init; } = new();
}

public record EnemyState
{
    public List<Character> Characters { get; init; } = new();

    private static readonly CharacterTemplateFactory _characterFactory = new();

    public EnemyState(EnemyStateRecord enemyStateRecord)
    {
        foreach (CharacterTemplateData data in enemyStateRecord.Characters)
        {
            CharacterTemplate template = _characterFactory.Create(ReferenceId.New(), data);
            Characters.Add(new Character(template, new CharacterInstanceData { TemplateId = data.Id }));
        }
    }
}

public record EnemyStateRecord
{
    public List<CharacterTemplateData> Characters { get; init; } = new();
}

public class GameStateFactory
{
    public static GameState CreateFromRecord(GameStateRecord gameStateRecord)
    {
        return new GameState(gameStateRecord);
    }

    public static PlayerState CreatePlayerStateFromRecord(PlayerStateRecord playerStateRecord)
    {
        return new PlayerState(playerStateRecord);
    }
}
