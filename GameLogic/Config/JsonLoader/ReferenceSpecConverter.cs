namespace GameLogic.Config.JsonLoader;

using System.Text.Json;
using System.Text.Json.Serialization;
using GameLogic.Entities.Skills;
using GameLogic.Registry;
using GameLogic.Actions.Usables;
using GameLogic.Actions.Effects;

public sealed class ReferenceSpecConverter : JsonConverter<ReferenceSpec>
{
    public override ReferenceSpec Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        Console.WriteLine("ReferenceSpecConverter.Read called");
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        using JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader);
        JsonElement rootElement = jsonDocument.RootElement;

        ReferenceMetadata referenceMetadata = JsonConverter.GetProperty<ReferenceMetadata>(
            rootElement,
            "ReferenceMetadata",
            options
        );

        switch (referenceMetadata.Kind)
        {
            case ETemplateKind.Ref:
            {
                return JsonSerializer.Deserialize<ReferenceSpec>(
                    rootElement.GetRawText(),
                    options
                )!;
            }
            case ETemplateKind.Override:
            case ETemplateKind.Inline:
            case ETemplateKind.Instance:
            {
                (Type valueType, Type patchType) = TypeMaps.GetReferenceTypes(
                    referenceMetadata.TemplateType
                );
                Type genericType = typeof(TypedReferenceSpec<,>).MakeGenericType(
                    valueType,
                    patchType
                );

                return (ReferenceSpec)
                    JsonSerializer.Deserialize(rootElement.GetRawText(), genericType, options)!;
            }
            default:
            {
                throw new JsonException($"Invalid kind: {referenceMetadata.Kind}");
            }
        }
    }

    private ETemplateKind ParseKind(JsonElement rootElement)
    {
        if (!rootElement.TryGetProperty("Kind", out JsonElement kindElement))
            throw new JsonException("Expected 'Kind' property");

        string kindString =
            kindElement.GetString()
            ?? throw new JsonException("Expected 'Kind' property to be a string");

        ETemplateKind kind = Enum.Parse<ETemplateKind>(kindString);
        return kind;
    }

    private ReferenceMetadata ParseReferenceMetadata(
        JsonElement rootElement,
        JsonSerializerOptions options
    )
    {
        if (!rootElement.TryGetProperty("Metadata", out JsonElement referenceMetadataElement))
            throw new JsonException("Expected 'Metadata' property");

        ReferenceMetadata? referenceMetadata = JsonSerializer.Deserialize<ReferenceMetadata>(
            referenceMetadataElement.GetRawText(),
            options
        );
        if (referenceMetadata is null)
            throw new JsonException(
                "Expected 'Metadata' property to be a valid reference metadata"
            );

        return referenceMetadata;
    }

    public override void Write(
        Utf8JsonWriter writer,
        ReferenceSpec value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
