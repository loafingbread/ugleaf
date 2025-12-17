namespace GameLogic.Config.JsonLoader;

using System.Text.Json;
using System.Text.Json.Serialization;
using GameLogic.Entities.Stats;

/// <summary>
/// Custom JSON converter for IStatConfigData to handle polymorphic deserialization.
/// </summary>
public class StatConfigRecordConverter : JsonConverter<IStatConfigData>
{
    public override IStatConfigData Read(
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
            return JsonSerializer.Deserialize<ResourceStatConfigData>(
                rootElement.GetRawText(),
                options
            )!;
        }

        return JsonSerializer.Deserialize<ValueStatConfigData>(
            rootElement.GetRawText(),
            options
        )!;
    }

    public override void Write(
        Utf8JsonWriter writer,
        IStatConfigData value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
