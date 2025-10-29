namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Characters;
using GameLogic.Registry;

public class CharacterTestFixture
{
    public ReferenceUnionSpec AliceRecord { get; }
    public ReferenceUnionSpec AshRecord { get; }
    public ReferenceUnionSpec BrockRecord { get; }
    public ReferenceUnionSpec GoblinRecord { get; }
    public ReferenceUnionSpec MissyRecord { get; }

    public CharacterTestFixture()
    {
        AliceRecord = JsonConfigLoader.LoadFromFile<ReferenceUnionSpec>(
            ConfigPaths.CharacterTemplate.Alice
        );
        AshRecord = JsonConfigLoader.LoadFromFile<ReferenceUnionSpec>(
            ConfigPaths.CharacterTemplate.Ash
        );
        BrockRecord = JsonConfigLoader.LoadFromFile<ReferenceUnionSpec>(
            ConfigPaths.CharacterTemplate.Brock
        );
        GoblinRecord = JsonConfigLoader.LoadFromFile<ReferenceUnionSpec>(
            ConfigPaths.CharacterTemplate.Goblin
        );
        MissyRecord = JsonConfigLoader.LoadFromFile<ReferenceUnionSpec>(
            ConfigPaths.CharacterTemplate.Missy
        );
    }
}
