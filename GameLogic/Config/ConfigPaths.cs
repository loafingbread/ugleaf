namespace GameLogic.Config;

// Config path constants for safe usage and autocomplete:
// string literals for paths are so icky
public static class ConfigPaths
{
    private static readonly string BasePath = Path.Combine(
        AppContext.BaseDirectory,
        "../../../../"
    );

    public static class Character
    {
        public static readonly string Folder = Path.Combine(
            BasePath,
            "GameLogic.TestData/Entities/Characters/"
        );
        public static readonly string Alice = Path.Combine(Folder, "Alice.json");
        public static readonly string Ash = Path.Combine(Folder, "Ash.json");
        public static readonly string Brock = Path.Combine(Folder, "Brock.json");
        public static readonly string Missy = Path.Combine(Folder, "Missy.json");
        public static readonly string Colonizer = Path.Combine(Folder, "Colonizer.json");
        public static readonly string Goblin = Path.Combine(Folder, "Goblin.json");
        public static readonly string GoblinSlayer = Path.Combine(Folder, "GoblinSlayer.json");
    }

    public static class Skill
    {
        public static readonly string Folder = Path.Combine(
            BasePath,
            "GameLogic.TestData/Entities/Skills/"
        );

        public static readonly string Ignite = Path.Combine(Folder, "Ignite.json");
        public static readonly string FacePalm = Path.Combine(Folder, "FacePalm.json");
        public static readonly string Steal = Path.Combine(Folder, "Steal.json");
        public static readonly string Mug = Path.Combine(Folder, "Mug.json");
        public static readonly string SprayAndPray = Path.Combine(Folder, "SprayAndPray.json");
    }

    public static class Stat
    {
        public static readonly string Folder = Path.Combine(
            BasePath,
            "GameLogic.TestData/Entities/Stats/"
        );

        public static readonly string ValueStatStrength = Path.Combine(
            Folder,
            "ValueStatStrength.json"
        );
        public static readonly string ResourceStatHealth = Path.Combine(
            Folder,
            "ResourceStatHealth.json"
        );
        public static readonly string TestStatBlock = Path.Combine(Folder, "TestStatBlock.json");
    }

    public static class GameState
    {
        public static readonly string Folder = Path.Combine(
            BasePath,
            "GameLogic.TestData/Entities/GameState/"
        );

        public static readonly string Simple = Path.Combine(Folder, "Simple.json");
    }
}
