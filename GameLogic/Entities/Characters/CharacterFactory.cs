namespace GameLogic.Entities.Characters;

public static class CharacterFactory
{
    public static Character CreateFromRecord(ICharacterRecord record)
    {
        ICharacterConfig config = new CharacterConfig(record);
        return new Character(config);
    }
}
