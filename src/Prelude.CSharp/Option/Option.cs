using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Prelude;

/// <summary>
/// <para>
/// The <see cref="Option{_}"/> type encapsulates an optional value.
/// A value of type <see cref="Option{_}"/> a either contains a value of type <typeparamref name="T"/>
/// (represented as <see cref="Option{_}.Some"/>(<typeparamref name="T"/>)),
/// or it is empty (represented as <see cref="Option{T}.None"/>).
/// </para>
/// <para>
/// Using Maybe is a good way to deal with errors or exceptional cases
/// without resorting to drastic measures such as error.
/// </para>
/// </summary>
/// <remarks>
/// The <see cref="Option{_}"/> type is also a monad (has LINQ Select/SelectMany).
/// It is a simple kind of error monad, where all errors are represented by Nothing.
/// A richer error monad can be built using the <see cref="Either{_, _}"/> type.
/// </remarks>
[JsonConverter(typeof(OptionJsonConverter))]
public readonly record struct Option<T>
{
    private readonly T _value;

    internal T UnsafeValue => _value;

    /// <summary>
    /// Gets a value indicating whether the option contains a value.
    /// </summary>
    public bool IsSome { get; }

    /// <summary>
    /// Gets a value indicating whether the option does not contain a value.
    /// </summary>
    public bool IsNone => !IsSome;

    /// <summary>
    /// Initializes a new instance of the <see cref="Option{T}"/> struct with a specified value.
    /// </summary>
    /// <param name="value">The value to initialize the option with.</param>
    private Option(T value)
    {
        _value = value;
        IsSome = true;
    }

    /// <summary>
    /// Creates a new <see cref="Option{T}"/> instance that contains a value.
    /// </summary>
    /// <param name="value">The value to store in the option.</param>
    /// <returns>An <see cref="Option{T}"/> in the "Some" state containing the specified value.</returns>
    public static Option<T> Some(T value) => new(value);

    /// <summary>
    /// Creates a new <see cref="Option{T}"/> instance in the "None" state.
    /// </summary>
    /// <returns>An <see cref="Option{T}"/> in the "None" state.</returns>
    public static Option<T> None() => default;

    /// <summary>
    /// Folds <see cref="Option{_}"/> into a single value by applying the <paramref name="some"/> function
    /// to an existing value or using the <paramref name="none"/> function in case of a missing value.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="some">How to transform value into a result.</param>
    /// <param name="none">How to compensate with a missing value.</param>
    /// <returns>The result of applying the appropriate function to the optional value.</returns>
    public TResult Fold<TResult>(
        Func<T, TResult> some,
        Func<TResult> none) =>
        IsSome ? some(_value) : none();

    /// <summary>
    /// Returns the value contained in the <see cref="Option{T}"/> if it is in the "Some" state;
    /// otherwise, returns the specified default value.
    /// </summary>
    /// <param name="defaultValue">The value to return if the option is in the "None" state.</param>
    /// <returns>The contained value if "Some"; otherwise, the specified default value.</returns>
    public T GetValueOrDefault(T defaultValue) => IsSome ? _value : defaultValue;

    /// <summary>
    /// Returns the value contained in the <see cref="Option{T}"/> if it is in the "Some" state;
    /// otherwise, returns a value produced by the specified factory function.
    /// </summary>
    /// <param name="defaultValueFactory">A function that produces the default value if the option is in the "None" state.</param>
    /// <returns>The contained value if "Some"; otherwise, the value produced by the <paramref name="defaultValueFactory"/>.</returns>
    public T GetValueOrDefault(Func<T> defaultValueFactory) => IsSome ? _value : defaultValueFactory();

    /// <inheritdoc />
    public override string ToString() => IsSome ? $"Some({_value})" : "None";

    /// <summary>
    /// Attempts to retrieve the value from the <see cref="Option{T}"/> if it is in the "Some" state.
    /// </summary>
    /// <param name="value">
    /// When this method returns, contains the value of the <see cref="Option{T}"/> if it is in the "Some" state;
    /// otherwise, the default value of <typeparamref name="T"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the <see cref="Option{T}"/> is in the "Some" state and contains a value;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool TryGetValue(out T value)
    {
        value = _value;
        return IsSome;
    }
}

/// <summary>
/// Provides a set of static methods for creating instances of <see cref="Option{T}"/> type.
/// </summary>
public static class Option
{
    /// <summary>
    /// A delegate for Try pattern, for example <see cref="System.Guid.TryParse(string?, out Guid)"/>
    /// </summary>
    /// <seealso cref="Option.TryGetValue{_, _}"/>
    public delegate bool OutFunc<in TSource, TResult>(TSource source, out TResult result);

    /// <summary>
    /// A delegate for Try pattern, for example <see cref="System.Guid.TryParseExact(string?, string?, out Guid)"/>
    /// </summary>
    /// <seealso cref="Option.TryGetValue{_, _, _}"/>
    public delegate bool OutFunc<in TSource, in TParam, TResult>(TSource source, TParam param, out TResult result);

    /// <summary>
    /// Creates a new <see cref="Option{T}"/> value with the <see cref="Option{T}.Some"/> case.
    /// </summary>
    /// <typeparam name="T">The type of the value in the option.</typeparam>
    /// <param name="value">The value to wrap in a Some instance.</param>
    /// <returns>A Some instance containing the specified value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Some<T>(T value) => Option<T>.Some(value);

    /// <summary>
    /// Creates a new <see cref="Option{T}"/> value with the <see cref="Option{T}.None"/> case.
    /// </summary>
    /// <typeparam name="T">The type of the value in the option.</typeparam>
    /// <returns>A None instance representing an empty option.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> None<T>() => Option<T>.None();

    /// <summary>
    /// Uses Try pattern to retrieve an optional value
    /// </summary>
    /// <typeparam name="TSource">The type of the source value.</typeparam>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <param name="source">The source value to attempt conversion.</param>
    /// <param name="tryGetValue">The function that attempts to convert the source value into the result value.</param>
    /// <returns>An Option containing the result value if the conversion is successful, otherwise a None instance.</returns>
    /// <example>
    /// <code>
    /// var result = Option.TryGetValue&lt;string, Guid&gt;(rawValue, Guid.TryParse);
    /// </code>
    /// </example>
    public static Option<TResult> TryGetValue<TSource, TResult>(
        TSource source,
        OutFunc<TSource, TResult> tryGetValue) =>
        tryGetValue(source, out var result) ? Some(result) : None<TResult>();

    /// <summary>
    /// Uses Try pattern to retrieve an optional value
    /// </summary>
    /// <typeparam name="TSource">The type of the source value.</typeparam>
    /// <typeparam name="TParam">The type of the additional parameter for conversion.</typeparam>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <param name="source">The source value to attempt conversion.</param>
    /// <param name="param">An additional parameter required for the conversion.</param>
    /// <param name="tryGetValue">The function that attempts to convert the source value into the result value.</param>
    /// <returns>An Option containing the result value if the conversion is successful, otherwise a None instance.</returns>
    /// <example>
    /// <code>
    /// var result = Option.TryGetValue&lt;string, Guid&gt;(rawValue, format, Guid.TryParseExact);
    /// </code>
    /// </example>
    public static Option<TResult> TryGetValue<TSource, TParam, TResult>(
        TSource source, TParam param,
        OutFunc<TSource, TParam, TResult> tryGetValue) =>
        tryGetValue(source, param, out var result) ? Some(result) : None<TResult>();

    /// <summary>
    /// Converts possibly null value to an <see cref="Option{T}"/>
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">A possibly null value to convert to an Option.</param>
    /// <returns>A Some instance if the value is non-null, otherwise a None instance.</returns>
    public static Option<T> FromNullable<T>(T? value) where T : class =>
        value is not null ? Some(value) : None<T>();

    /// <summary>
    /// Converts possibly null value to an <see cref="Option{T}"/>
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">A nullable value to convert to an Option.</param>
    /// <returns>A Some instance if the value has a value, otherwise a None instance.</returns>
    public static Option<T> FromNullable<T>(T? value) where T : struct =>
        value.HasValue ? Some(value.Value) : None<T>();
}
