namespace GameLogic.Entities.Characters;

public static class CharacterFactory
{
    public static Character CreateFromRecord(ICharacterRecord record)
    {
        CharacterConfig config = new CharacterConfig(record);
        return new Character(config);
    }
}
