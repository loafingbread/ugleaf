namespace GameLogic.Config;

using System.Text.Json;
using System.Text.Json.Serialization;
using GameLogic.Registry;

public sealed class ReferenceUnionSpecConverter : JsonConverter<ReferenceUnionSpec>
{
    public override ReferenceUnionSpec Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        using JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader);
        JsonElement rootElement = jsonDocument.RootElement;

        ReferenceUnionMetadata referenceMetadata = ParseReferenceMetadata(rootElement, options);

        return referenceMetadata.Kind switch
        {
            EReferenceKind.Ref => this.DeserializeRefSpec(rootElement, options, referenceMetadata),
            EReferenceKind.Override => this.DeserializeOverrideSpec(
                rootElement,
                options,
                referenceMetadata
            ),
            EReferenceKind.Inline => this.DeserializeInlineSpec(
                rootElement,
                options,
                referenceMetadata
            ),
            EReferenceKind.Instance => this.DeserializeInstanceSpec(
                rootElement,
                options,
                referenceMetadata
            ),
            _ => throw new JsonException($"Invalid kind: {referenceMetadata.Kind}"),
        };
    }

    private EReferenceKind ParseKind(JsonElement rootElement)
    {
        if (!rootElement.TryGetProperty("Kind", out JsonElement kindElement))
            throw new JsonException("Expected 'Kind' property");

        string kindString =
            kindElement.GetString()
            ?? throw new JsonException("Expected 'Kind' property to be a string");

        EReferenceKind kind = Enum.Parse<EReferenceKind>(kindString);
        return kind;
    }

    private ReferenceUnionMetadata ParseReferenceMetadata(
        JsonElement rootElement,
        JsonSerializerOptions options
    )
    {
        if (!rootElement.TryGetProperty("Metadata", out JsonElement referenceMetadataElement))
            throw new JsonException("Expected 'Metadata' property");

        ReferenceUnionMetadata? referenceMetadata =
            JsonSerializer.Deserialize<ReferenceUnionMetadata>(
                referenceMetadataElement.GetRawText(),
                options
            );
        if (referenceMetadata is null)
            throw new JsonException(
                "Expected 'Metadata' property to be a valid reference metadata"
            );

        return referenceMetadata;
    }

    private ReferenceUnionSpec DeserializeRefSpec(
        JsonElement rootElement,
        JsonSerializerOptions options,
        ReferenceUnionMetadata referenceMetadata
    )
    {
        return JsonSerializer.Deserialize<RefSpec>(rootElement.GetRawText(), options)!;
    }

    private ReferenceUnionSpec DeserializeOverrideSpec(
        JsonElement rootElement,
        JsonSerializerOptions options,
        ReferenceUnionMetadata referenceMetadata
    )
    {
        (Type templateType, Type overrideType) = TemplateTypeMaps.ETemplateTypeToOverrideType[
            referenceMetadata.TemplateType
        ];
        Type genericType = typeof(OverrideSpec<,>).MakeGenericType(templateType, overrideType);

        return (ReferenceUnionSpec)
            JsonSerializer.Deserialize(rootElement.GetRawText(), genericType, options)!;
    }

    private ReferenceUnionSpec DeserializeInlineSpec(
        JsonElement rootElement,
        JsonSerializerOptions options,
        ReferenceUnionMetadata referenceMetadata
    )
    {
        Type templateType = TemplateTypeMaps.ETemplateTypeToTemplateType[
            referenceMetadata.TemplateType
        ];
        Type genericType = typeof(InlineSpec<>).MakeGenericType(templateType);

        return (ReferenceUnionSpec)
            JsonSerializer.Deserialize(rootElement.GetRawText(), genericType, options)!;
    }

    private ReferenceUnionSpec DeserializeInstanceSpec(
        JsonElement rootElement,
        JsonSerializerOptions options,
        ReferenceUnionMetadata referenceMetadata
    )
    {
        (Type templateType, Type overrideType) = TemplateTypeMaps.ETemplateTypeToOverrideType[
            referenceMetadata.TemplateType
        ];
        Type genericType = typeof(InstanceSpec<,>).MakeGenericType(templateType, overrideType);

        return (ReferenceUnionSpec)
            JsonSerializer.Deserialize(rootElement.GetRawText(), genericType, options)!;
    }

    public override void Write(
        Utf8JsonWriter writer,
        ReferenceUnionSpec value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
