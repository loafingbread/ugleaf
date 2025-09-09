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

        EReferenceKind kind = ParseKind(rootElement);

        TemplateIdentifier templateIdentifier = ParseTemplateIdentifier(rootElement, options);

        return kind switch
        {
            EReferenceKind.Ref => this.DeserializeRefSpec(rootElement, options),
            EReferenceKind.Override => this.DeserializeOverrideSpec(
                rootElement,
                templateIdentifier.TemplateType,
                options
            ),
            EReferenceKind.Inline => this.DeserializeInlineSpec(
                rootElement,
                templateIdentifier.TemplateType,
                options
            ),
            EReferenceKind.Instance => this.DeserializeInstanceSpec(
                rootElement,
                templateIdentifier.TemplateType,
                options
            ),
            _ => throw new JsonException($"Invalid kind: {kind}"),
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

    private TemplateIdentifier ParseTemplateIdentifier(
        JsonElement rootElement,
        JsonSerializerOptions options
    )
    {
        if (
            !rootElement.TryGetProperty(
                "TemplateIdentifier",
                out JsonElement templateIdentifierElement
            )
        )
            throw new JsonException("Expected 'TemplateIdentifier' property");

        TemplateIdentifier? templateIdentifier = JsonSerializer.Deserialize<TemplateIdentifier>(
            templateIdentifierElement.GetRawText(),
            options
        );
        if (templateIdentifier is null)
            throw new JsonException(
                "Expected 'TemplateIdentifier' property to be a valid template identifier"
            );

        return templateIdentifier;
    }

    private ReferenceUnionSpec DeserializeRefSpec(
        JsonElement rootElement,
        JsonSerializerOptions options
    )
    {
        return JsonSerializer.Deserialize<RefSpec>(rootElement.GetRawText(), options)!;
    }

    private ReferenceUnionSpec DeserializeOverrideSpec(
        JsonElement rootElement,
        ETemplateType eTemplateType,
        JsonSerializerOptions options
    )
    {
        (Type templateType, Type overrideType) = TemplateTypeMaps.ETemplateTypeToOverrideType[
            eTemplateType
        ];
        Type genericType = typeof(OverrideSpec<,>).MakeGenericType(templateType, overrideType);

        return (ReferenceUnionSpec)
            JsonSerializer.Deserialize(rootElement.GetRawText(), genericType, options)!;
    }

    private ReferenceUnionSpec DeserializeInlineSpec(
        JsonElement rootElement,
        ETemplateType eTemplateType,
        JsonSerializerOptions options
    )
    {
        Type templateType = TemplateTypeMaps.ETemplateTypeToTemplateType[eTemplateType];
        Type genericType = typeof(InlineSpec<>).MakeGenericType(templateType);

        return (ReferenceUnionSpec)
            JsonSerializer.Deserialize(rootElement.GetRawText(), genericType, options)!;
    }

    private ReferenceUnionSpec DeserializeInstanceSpec(
        JsonElement rootElement,
        ETemplateType eTemplateType,
        JsonSerializerOptions options
    )
    {
        (Type templateType, Type overrideType) = TemplateTypeMaps.ETemplateTypeToOverrideType[
            eTemplateType
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
