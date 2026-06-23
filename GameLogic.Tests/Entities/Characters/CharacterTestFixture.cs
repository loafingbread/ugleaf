namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Characters;

public class CharacterTestFixture
{
    public CharacterTemplateData AliceRecord { get; }
    public CharacterTemplateData AshRecord { get; }
    public CharacterTemplateData BrockRecord { get; }
    public CharacterTemplateData GoblinRecord { get; }
    public CharacterTemplateData MissyRecord { get; }

    public CharacterTestFixture()
    {
        AliceRecord = JsonConfigLoader.LoadFromFile<CharacterTemplateData>(ConfigPaths.Character.Alice);
        AshRecord = JsonConfigLoader.LoadFromFile<CharacterTemplateData>(ConfigPaths.Character.Ash);
        BrockRecord = JsonConfigLoader.LoadFromFile<CharacterTemplateData>(ConfigPaths.Character.Brock);
        GoblinRecord = JsonConfigLoader.LoadFromFile<CharacterTemplateData>(ConfigPaths.Character.Goblin);
        MissyRecord = JsonConfigLoader.LoadFromFile<CharacterTemplateData>(ConfigPaths.Character.Missy);
    }
}
