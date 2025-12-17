namespace GameLogic.Config;

using System.Text.Json;
using System.Text.Json.Serialization;
using GameLogic.Entities.Characters;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Registry;
using GameLogic.Usables;
using GameLogic.Usables.Effects;

// Reusuable config loader utility
public static class JsonConfigLoader
{
    public static readonly JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new JsonStringEnumConverter(),
            // new StatConfigRecordConverter(),
            new GameLogic.Config.JsonLoader.ReferenceSpecConverter(),
        },
    };

    public static T LoadFromFile<T>(string path)
    {
        string json = File.ReadAllText(path);
        return System.Text.Json.JsonSerializer.Deserialize<T>(json, options)
            ?? throw new InvalidOperationException($"Failed to load {typeof(T).Name} from {path}");
    }

    public static List<T> LoadAllFromFolder<T>(string folderPath)
    {
        List<T> list = new();

        foreach (string filePath in Directory.GetFiles(folderPath, "*.json"))
        {
            try
            {
                T config = LoadFromFile<T>(filePath);
                list.Add(config);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Failed to load {filePath}: {ex.Message}");
            }
        }

        return list;
    }
}
