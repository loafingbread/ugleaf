namespace GameLogic.Entities;

using GameLogic.Entities.Characters;

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

    public PlayerState(PlayerStateRecord playerStateRecord)
    {
        // foreach (CharacterData CharacterData in playerStateRecord.Characters)
        // {
        //     Character character = CharacterFactory.CreateCharacterReferenceFromRecord(CharacterData);
        //     this.Characters.Add(character);
        // }
    }
}

public record PlayerStateRecord
{
    public List<CharacterData> Characters { get; init; } = new();
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
