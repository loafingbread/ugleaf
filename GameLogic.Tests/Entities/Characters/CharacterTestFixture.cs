namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Characters;

public class CharacterTestFixture
{
    public CharacterConfig AliceConfig { get; }
    public CharacterConfig AshConfig { get; }
    public CharacterConfig BrockConfig { get; }
    public CharacterConfig GoblinConfig { get; }
    public CharacterConfig MissyConfig { get; }

    public CharacterTestFixture()
    {
        AliceConfig = JsonConfigLoader.LoadFromFile<CharacterConfig>(ConfigPaths.Character.Alice);
        AshConfig = JsonConfigLoader.LoadFromFile<CharacterConfig>(ConfigPaths.Character.Ash);
        BrockConfig = JsonConfigLoader.LoadFromFile<CharacterConfig>(ConfigPaths.Character.Brock);
        GoblinConfig = JsonConfigLoader.LoadFromFile<CharacterConfig>(ConfigPaths.Character.Goblin);
        MissyConfig = JsonConfigLoader.LoadFromFile<CharacterConfig>(ConfigPaths.Character.Missy);
    }
}
