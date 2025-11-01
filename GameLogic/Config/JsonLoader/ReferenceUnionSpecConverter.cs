namespace GameLogic.Config.JsonLoader;

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
        Console.WriteLine("Read called");
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
        Console.WriteLine($"Root: {rootElement.GetRawText()}");
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
        Type inlineSpecType = typeof(InlineSpec<>).MakeGenericType(templateType);
        object inlineSpec =
            Activator.CreateInstance(inlineSpecType)
            ?? throw new JsonException("Failed to create instance of InlineSpec");

        // Extract the Template property from JSON
        if (!rootElement.TryGetProperty("Template", out JsonElement templateElement))
        {
            throw new JsonException("Expected 'Template' property for InlineSpec");
        }

        Console.WriteLine($"Template: {templateElement.GetRawText()}");
        Console.WriteLine($"Template type: {templateType.FullName}");
        Console.WriteLine($"Generic type: {inlineSpecType.FullName}");
        // Console.WriteLine($"Options: {JsonSerializer.Serialize(options)}");

        // // Deserialize the template
        // object? template = JsonSerializer.Deserialize(
        //     templateElement.GetRawText(),
        //     templateType,
        //     options
        // );

        // if (template is null)
        // {
        //     throw new JsonException("Failed to deserialize Template");
        // }

        // Construct the InlineSpec using the record's constructor
        // Records have a constructor that takes all properties as parameters
        // var constructor = genericType.GetConstructor(
        //     new[] { typeof(ReferenceUnionMetadata), templateType }
        // );
        // if (constructor is null)
        // {
        //     throw new JsonException(
        //         $"Failed to find constructor for {genericType.Name} with Metadata and Template parameters"
        //     );
        // }

        // var instance = constructor.Invoke(new object[] { referenceMetadata, template });
        // return (ReferenceUnionSpec)instance;

        inlineSpecType
            .GetProperty("Metadata")
            ?.SetValue(inlineSpec, referenceMetadata);
        inlineSpecType
            .GetProperty("Template")
            ?.SetValue(inlineSpec, rootElement.GetProperty("Template"));

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
        JsonSerializer.Serialize(writer, value, options);
    }
}
