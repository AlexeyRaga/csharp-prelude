using System.Text.Json;
using System.Text.Json.Serialization;

namespace Prelude;

/// <inheritdoc />
public class NonEmptyListJsonConverterFactory : JsonConverterFactory
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(NonEmptyList<>);

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var itemType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(NonEmptyListJsonConverter<>).MakeGenericType(itemType);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}

/// <inheritdoc />
public class NonEmptyListJsonConverter<T> : JsonConverter<NonEmptyList<T>>
{
    /// <inheritdoc />
    public override NonEmptyList<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var items = JsonSerializer.Deserialize<List<T>>(ref reader, options);

        if (items == null || items.Count == 0)
            throw new JsonException("Expected a non-empty list.");

        // Create and return the NonEmptyList
        return new NonEmptyList<T>(items.First(), items.Skip(1));
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, NonEmptyList<T> value, JsonSerializerOptions options) =>
        // Serialize the inner list
        JsonSerializer.Serialize(writer, value.ToImmutableList(), options);
}
