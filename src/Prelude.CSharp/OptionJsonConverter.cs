using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Prelude;

/// <inheritdoc />
public class OptionJsonConverter : JsonConverterFactory
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Option<>);

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var valueType = typeToConvert.GetGenericArguments()[0];
        return (JsonConverter)Activator.CreateInstance(
            typeof(OptionJsonConverter<>).MakeGenericType([valueType]),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: [options],
            culture: null)!;
    }
}

/// <inheritdoc />
public sealed class OptionJsonConverter<T> : JsonConverter<Option<T>>
{
    private readonly JsonConverter<T> _valueConverter;

    /// <inheritdoc />
    public override bool HandleNull => true;

    /// <inheritdoc />
    public OptionJsonConverter(JsonSerializerOptions options)
    {
        _valueConverter = (JsonConverter<T>)options.GetConverter(typeof(T));
    }

    /// <inheritdoc />
    public override Option<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return Option.None<T>();
        }

        var value = _valueConverter.Read(ref reader, typeof(T), options);
        return value == null ? Option.None<T>() : Option.Some(value);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Option<T> value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case Option<T>.Some some:
                _valueConverter.Write(writer, some.Value, options);
                break;
            case Option<T>.None:
                writer.WriteNullValue();
                break;
        }
    }
}
