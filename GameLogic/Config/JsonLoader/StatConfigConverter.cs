namespace GameLogic.Config.JsonLoader;

using System.Text.Json;
using System.Text.Json.Serialization;
using GameLogic.Entities.Stats;

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
