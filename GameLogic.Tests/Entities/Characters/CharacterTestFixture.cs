namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Characters;

public class CharacterTestFixture
{
    public CharacterRecord AliceRecord { get; }
    public CharacterRecord AshRecord { get; }
    public CharacterRecord BrockRecord { get; }
    public CharacterRecord GoblinRecord { get; }
    public CharacterRecord MissyRecord { get; }

    public CharacterTestFixture()
    {
        AliceRecord = JsonConfigLoader.LoadFromFile<CharacterRecord>(ConfigPaths.Character.Alice);
        AshRecord = JsonConfigLoader.LoadFromFile<CharacterRecord>(ConfigPaths.Character.Ash);
        BrockRecord = JsonConfigLoader.LoadFromFile<CharacterRecord>(ConfigPaths.Character.Brock);
        GoblinRecord = JsonConfigLoader.LoadFromFile<CharacterRecord>(ConfigPaths.Character.Goblin);
        MissyRecord = JsonConfigLoader.LoadFromFile<CharacterRecord>(ConfigPaths.Character.Missy);
    }
}
