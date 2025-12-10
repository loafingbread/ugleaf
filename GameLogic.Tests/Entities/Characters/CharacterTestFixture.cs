namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Characters;
using GameLogic.Registry;

public class CharacterTestFixture
{
    public ReferenceSpec AliceRecord { get; }
    public ReferenceSpec AshRecord { get; }
    public ReferenceSpec BrockRecord { get; }
    public ReferenceSpec GoblinRecord { get; }
    public ReferenceSpec MissyRecord { get; }

    public CharacterTestFixture()
    {
        AliceRecord = JsonConfigLoader.LoadFromFile<ReferenceSpec>(
            ConfigPaths.CharacterTemplate.Alice
        );
        AshRecord = JsonConfigLoader.LoadFromFile<ReferenceSpec>(
            ConfigPaths.CharacterTemplate.Ash
        );
        BrockRecord = JsonConfigLoader.LoadFromFile<ReferenceSpec>(
            ConfigPaths.CharacterTemplate.Brock
        );
        GoblinRecord = JsonConfigLoader.LoadFromFile<ReferenceSpec>(
            ConfigPaths.CharacterTemplate.Goblin
        );
        MissyRecord = JsonConfigLoader.LoadFromFile<ReferenceSpec>(
            ConfigPaths.CharacterTemplate.Missy
        );
    }
}
