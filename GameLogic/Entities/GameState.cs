namespace GameLogic.Entities;

using GameLogic.Entities.Characters;
using GameLogic.Registry;

public class GameState
{
    public PlayerState PlayerState { get; private set; }

    public GameState(GameStateRecord gameStateRecord)
    {
        this.PlayerState = new PlayerState(gameStateRecord.PlayerState);
    }
}

public record GameStateRecord
{
    public PlayerStateRecord PlayerState { get; init; } = new();
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
