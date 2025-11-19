namespace GameLogic.Config.JsonLoader;

using System.Text.Json;

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
