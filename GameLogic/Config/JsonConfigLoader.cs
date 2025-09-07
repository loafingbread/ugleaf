namespace GameLogic.Config;

using System.Text.Json;
using System.Text.Json.Serialization;
using GameLogic.Entities.Stats;
using GameLogic.Registry;

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

public class ReferenceUnionSpecConverter : JsonConverter<GameLogic.Registry.ReferenceUnionSpec>
{
    public override GameLogic.Registry.ReferenceUnionSpec Read(
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

        if (rootElement.TryGetProperty("TemplateId", out _))
        {
            return JsonSerializer.Deserialize<RefSpec>(rootElement.GetRawText(), options)!;
        }

        if (rootElement.TryGetProperty("TemplateId", out _))
            if (rootElement.TryGetProperty("TemplateId", out _))
            {
                return JsonSerializer.Deserialize<InlineSpec<TemplateBase>>(
                    rootElement.GetRawText(),
                    options
                )!;
            }

        if (rootElement.TryGetProperty("TemplateId", out _))
            return JsonSerializer.Deserialize<ReferenceUnionSpec>(reader.GetString(), options);
    }

    private ReferenceUnionSpec Parse(JsonElement element, JsonSerializerOptions options)
    {
        bool isRefSpec = element.TryGetProperty("Kind", out JsonElement kind);
        if (!isRefSpec)
        {
            throw new JsonException("Expected 'Kind' property for a ReferenceUnionSpec");
        }

        switch (kind.GetString())
        {
            case "Ref":
                return JsonSerializer.Deserialize<RefSpec>(element.GetRawText(), options)!;
            case "Override":
                return JsonSerializer.Deserialize<OverrideSpec<TTemplate, TOverride>>(element.GetRawText(), options)!;
            case "Inline":
                return JsonSerializer.Deserialize<InlineSpec>(
                    element.GetRawText(),
                    options
                )!;
            case "Instance":
                return JsonSerializer.Deserialize<InstanceSpec>(element.GetRawText(), options)!;
            default:
                throw new JsonException($"Unknown kind: {kind.GetString()}");
        }
    }

    public override void Write(
        Utf8JsonWriter writer,
        GameLogic.Registry.ReferenceUnionSpec value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
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
