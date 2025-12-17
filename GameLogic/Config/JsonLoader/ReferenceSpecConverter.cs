namespace GameLogic.Config.JsonLoader;

using System.Text.Json;
using System.Text.Json.Serialization;
using GameLogic.Entities.Skills;
using GameLogic.Registry;
using GameLogic.Usables;
using GameLogic.Usables.Effects;

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
            "Metadata",
            options
        );

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

    private ReferenceSpec DeserializeRefSpec(
        JsonElement rootElement,
        JsonSerializerOptions options,
        ReferenceMetadata referenceMetadata
    )
    {
        return JsonSerializer.Deserialize<RefSpec>(rootElement.GetRawText(), options)!;
    }

    private ReferenceSpec DeserializeOverrideSpec(
        JsonElement rootElement,
        JsonSerializerOptions options,
        ReferenceMetadata referenceMetadata
    )
    {
        (Type templateType, Type overrideType) = TemplateTypeMaps.ETemplateTypeToOverrideType[
            referenceMetadata.TemplateType
        ];
        Type genericType = typeof(OverrideSpec<,>).MakeGenericType(templateType, overrideType);

        return (ReferenceSpec)
            JsonSerializer.Deserialize(rootElement.GetRawText(), genericType, options)!;
    }

    private ReferenceSpec DeserializeInlineSpec(
        JsonElement element,
        JsonSerializerOptions options,
        ReferenceMetadata referenceMetadata
    )
    {
        Console.WriteLine(
            $"DeserializeInlineSpec called: {referenceMetadata.TemplateType.ToString()}"
        );
        switch (referenceMetadata.TemplateType)
        {
            case ETemplateType.Skill:
            {
                return this.CreateInlineSpec(
                    element,
                    referenceMetadata,
                    typeof(SkillTemplateSpec),
                    options
                );
            }
            case ETemplateType.Usable:
            {
                return this.CreateInlineSpec(
                    element,
                    referenceMetadata,
                    typeof(UsableTemplateSpec),
                    options
                );
            }
            case ETemplateType.Effect:
            {
                return this.CreateInlineSpec(
                    element,
                    referenceMetadata,
                    typeof(EffectTemplateSpec),
                    options
                );
            }
            default:
                throw new NotSupportedException(
                    $"Template type {referenceMetadata.TemplateType} is not supported"
                );
        }
    }

    private ReferenceSpec CreateInlineSpec(
        JsonElement element,
        ReferenceMetadata referenceMetadata,
        Type templateType,
        JsonSerializerOptions options
    )
    {
        Type inlineSpecType = typeof(InlineSpec<>).MakeGenericType(templateType);
        object inlineSpec =
            Activator.CreateInstance(inlineSpecType)
            ?? throw new JsonException("Failed to create instance of InlineSpec");

        inlineSpecType.GetProperty("Metadata")?.SetValue(inlineSpec, referenceMetadata);

        object template = JsonConverter.GetProperty(element, "Template", templateType, options);
        inlineSpecType.GetProperty("Template")?.SetValue(inlineSpec, template);

        return (ReferenceSpec)inlineSpec;
    }

    private ReferenceSpec DeserializeInstanceSpec(
        JsonElement rootElement,
        JsonSerializerOptions options,
        ReferenceMetadata referenceMetadata
    )
    {
        (Type templateType, Type overrideType) = TemplateTypeMaps.ETemplateTypeToOverrideType[
            referenceMetadata.TemplateType
        ];
        Type genericType = typeof(InstanceSpec<,>).MakeGenericType(templateType, overrideType);

        return (ReferenceSpec)
            JsonSerializer.Deserialize(rootElement.GetRawText(), genericType, options)!;
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
