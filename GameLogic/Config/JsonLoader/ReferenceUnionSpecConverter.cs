namespace GameLogic.Config.JsonLoader;

using System.Text.Json;
using System.Text.Json.Serialization;
using GameLogic.Entities.Skills;
using GameLogic.Registry;
using GameLogic.Usables;
using GameLogic.Usables.Effects;

public static class JsonConverter
{
    // When you know the property type at compile time, use this method.
    public static T GetProperty<T>(
        JsonElement element,
        string propertyName,
        JsonSerializerOptions options
    )
    {
        if (!element.TryGetProperty(propertyName, out JsonElement propertyElement))
        {
            throw new JsonException(
                $"Expected property '{propertyName}' in JSON element '{element.GetRawText()}'"
            );
        }

        try
        {
            T propertyValue = JsonSerializer.Deserialize<T>(propertyElement.GetRawText(), options)!;
            return propertyValue;
        }
        catch (JsonException ex)
        {
            throw new JsonException(
                $"JSON element is not a valid {typeof(T).Name}: \n'{propertyElement.GetRawText()}'",
                ex
            );
        }
    }

    // When you specify the property type at runtime, use this method.
    public static object GetProperty(
        JsonElement element,
        string propertyName,
        Type propertyType,
        JsonSerializerOptions options
    )
    {
        if (!element.TryGetProperty(propertyName, out JsonElement propertyElement))
        {
            throw new JsonException(
                $"Expected property '{propertyName}' in JSON element '{element.GetRawText()}'"
            );
        }

        try
        {
            object? propertyValue = JsonSerializer.Deserialize(
                propertyElement.GetRawText(),
                propertyType,
                options
            );

            if (propertyValue is null)
            {
                throw new JsonException(
                    $"JSON element is not a valid {propertyType.Name}: \n'{propertyElement.GetRawText()}'"
                );
            }

            return propertyValue;
        }
        catch (JsonException ex)
        {
            throw new JsonException(
                $"JSON element is not a valid {propertyType.Name}: \n'{propertyElement.GetRawText()}'",
                ex
            );
        }
    }
}

// public class SkillTemplateRecordConverter : JsonConverter<SkillTemplateRecord>
// {
//     public override SkillTemplateRecord Read(
//         ref Utf8JsonReader reader,
//         Type typeToConvert,
//         JsonSerializerOptions options
//     )
//     {
//         using JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader);
//         JsonElement skillElement = jsonDocument.RootElement;

//         return JsonConverter.GetProperty<SkillTemplateRecord>(skillElement, "Template", options);
//     }
// }

// public class UsableTemplateRecordConverter : JsonConverter<UsableTemplateRecord>
// {
//     public override UsableTemplateRecord Read(
//         ref Utf8JsonReader reader,
//         Type typeToConvert,
//         JsonSerializerOptions options
//     )
//     {

//     }
// }

// public class UsableTemplateRecordConverter : JsonConverter<UsableTemplateRecord>
// {
//     public override SkillTemplateRecord Read(
//         ref Utf8JsonReader reader,
//         Type typeToConvert,
//         JsonSerializerOptions options
//     )
//     {
//         using JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader);
//         JsonElement skillElement = jsonDocument.RootElement;

//         Type templateType = TemplateTypeMaps.ETemplateTypeToTemplateType[
//             referenceMetadata.TemplateType
//         ];
//         Type inlineSpecType = typeof(InlineSpec<>).MakeGenericType(templateType);
//         object inlineSpec =
//             Activator.CreateInstance(inlineSpecType)
//             ?? throw new JsonException("Failed to create instance of InlineSpec");

//         // Extract the Template property from JSON
//         if (!rootElement.TryGetProperty("Template", out JsonElement templateElement))
//         {
//             throw new JsonException("Expected 'Template' property for InlineSpec");
//         }

//         inlineSpecType.GetProperty("Metadata")?.SetValue(inlineSpec, referenceMetadata);

//         SkillTemplateRecord template = GetPropertyFromJsonElement<SkillTemplateRecord>(
//             rootElement,
//             "Template",
//             options
//         );
//         inlineSpecType.GetProperty("Template")?.SetValue(inlineSpec, template);

//         return (ReferenceUnionSpec)inlineSpec;
//     }
// }

public sealed class ReferenceUnionSpecConverter : JsonConverter<ReferenceUnionSpec>
{
    public override ReferenceUnionSpec Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        Console.WriteLine("ReferenceUnionSpecConverter.Read called");
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        using JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader);
        JsonElement rootElement = jsonDocument.RootElement;

        ReferenceUnionMetadata referenceMetadata =
            JsonConverter.GetProperty<ReferenceUnionMetadata>(rootElement, "Metadata", options);

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
        JsonElement element,
        JsonSerializerOptions options,
        ReferenceUnionMetadata referenceMetadata
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
                    typeof(SkillTemplateRecord),
                    options
                );
            }
            case ETemplateType.Usable:
            {
                return this.CreateInlineSpec(
                    element,
                    referenceMetadata,
                    typeof(UsableTemplateRecord),
                    options
                );
            }
            case ETemplateType.Effect:
            {
                return this.CreateInlineSpec(
                    element,
                    referenceMetadata,
                    typeof(EffectTemplateRecord),
                    options
                );
            }
            default:
                throw new NotSupportedException(
                    $"Template type {referenceMetadata.TemplateType} is not supported"
                );
        }
    }

    private ReferenceUnionSpec CreateInlineSpec(
        JsonElement element,
        ReferenceUnionMetadata referenceMetadata,
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

        return (ReferenceUnionSpec)inlineSpec;
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
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
