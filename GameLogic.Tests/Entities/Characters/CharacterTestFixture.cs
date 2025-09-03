namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Characters;

public class CharacterTestFixture
{
    public CharacterTemplateRecord AliceRecord { get; }
    public CharacterTemplateRecord AshRecord { get; }
    public CharacterTemplateRecord BrockRecord { get; }
    public CharacterTemplateRecord GoblinRecord { get; }
    public CharacterTemplateRecord MissyRecord { get; }

    public CharacterTestFixture()
    {
        AliceRecord = JsonConfigLoader.LoadFromFile<CharacterTemplateRecord>(
            ConfigPaths.CharacterTemplate.Alice
        );
        AshRecord = JsonConfigLoader.LoadFromFile<CharacterTemplateRecord>(
            ConfigPaths.CharacterTemplate.Ash
        );
        BrockRecord = JsonConfigLoader.LoadFromFile<CharacterTemplateRecord>(
            ConfigPaths.CharacterTemplate.Brock
        );
        GoblinRecord = JsonConfigLoader.LoadFromFile<CharacterTemplateRecord>(
            ConfigPaths.CharacterTemplate.Goblin
        );
        MissyRecord = JsonConfigLoader.LoadFromFile<CharacterTemplateRecord>(
            ConfigPaths.CharacterTemplate.Missy
        );
    }
}
