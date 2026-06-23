namespace GameLogic.Entities.Characters;

using GameLogic.Registry;

public class CharacterInstanceFactory : IInstanceFactory<CharacterTemplate, CharacterInstanceData, Character>
{
    public Character Create(CharacterTemplate template, CharacterInstanceData data) => new(template, data);
}
