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
public abstract record Option<T>
{
    private Option() { }
    private static readonly Option<T> NoneSingleton = new None();

    /// <summary>
    /// Determines whether the specified <see cref="Option{T}"/> is <see cref="Option{T}.Some"/>.
    /// </summary>
    public bool IsSome => this is Some;

    /// <summary>
    /// Determines whether the specified <see cref="Option{T}"/> is <see cref="Option{T}.None"/>.
    /// </summary>
    public bool IsNone => this is None;

    /// <summary>
    /// Represents the <see cref="Option{T}.Some"/> case.
    /// </summary>
    public sealed record Some(T Value) : Option<T>;

    /// <summary>
    /// Represents the <see cref="Option{T}.None"/> case.
    /// </summary>
    public sealed record None : Option<T>
    {
        internal None() { }
    };

    /// <summary>
    /// Creates a new <see cref="Option{T}"/> value with the <see cref="Some"/> case.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> NewSome(T value) => new Some(value);


    /// <summary>
    /// Creates a new <see cref="Option{T}"/> value with the <see cref="None"/> case.
    /// </summary>
    public static Option<T> NewNone() => NoneSingleton;
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Some<T>(T value) => Option<T>.NewSome(value);
    /// <summary>
    /// Creates a new <see cref="Option{T}"/> value with the <see cref="Option{T}.None"/> case.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> None<T>() => Option<T>.NewNone();

    /// <summary>
    /// Uses Try pattern to retrieve an optional value
    /// </summary>
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
    public static Option<T> FromNullable<T>(T? value) where T : class =>
        value is not null ? Some(value) : None<T>();

    /// <summary>
    /// Converts possibly null value to an <see cref="Option{T}"/>
    /// </summary>
    public static Option<T> FromNullable<T>(T? value) where T : struct =>
        value.HasValue ? Some(value.Value) : None<T>();
}
