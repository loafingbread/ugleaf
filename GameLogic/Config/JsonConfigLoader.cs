namespace GameLogic.Config;

using System.Text.Json;
using System.Text.Json.Serialization;
using GameLogic.Entities.Stats;

// Reusuable config loader utility
public static class JsonConfigLoader
{
    public static readonly JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(), new StatConfigRecordConverter() },
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

/// <summary>
/// Custom JSON converter for IStatConfigRecord to handle polymorphic deserialization.
/// </summary>
public class StatConfigRecordConverter : JsonConverter<IStatConfigRecord>
{
    public override IStatConfigRecord Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        var rootElement = jsonDocument.RootElement;

        // Determine the type based on the presence of specific properties
        if (
            rootElement.TryGetProperty("BaseMaxValueCap", out _)
            || rootElement.TryGetProperty("BaseCapacityCap", out _)
        )
        {
            return JsonSerializer.Deserialize<ResourceStatConfigRecord>(
                rootElement.GetRawText(),
                options
            )!;
        }

        return JsonSerializer.Deserialize<ValueStatConfigRecord>(
            rootElement.GetRawText(),
            options
        )!;
    }

    public override void Write(
        Utf8JsonWriter writer,
        IStatConfigRecord value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
