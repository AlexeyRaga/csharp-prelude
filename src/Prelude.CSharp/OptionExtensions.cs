using System.Runtime.CompilerServices;

namespace Prelude;

/// <summary>
/// Provides a set of extension methods for <see cref="Option{T}"/> type.
/// </summary>
public static class OptionExtensions
{
    /// <summary>
    /// Folds <see cref="Option{_}"/> into a single value by applying the <paramref name="onSome"/> function
    /// to an existing value or using the <paramref name="onNone"/> function in case of a missing value.
    /// </summary>
    public static TResult Fold<TValue, TResult>(this Option<TValue> source, Func<TValue, TResult> onSome,
        Func<TResult> onNone) =>
        source switch
        {
            Option<TValue>.Some some => onSome(some.Value),
            Option<TValue>.None => onNone(),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Returns the value of the <see cref="Option{T}.Some"/> or the <paramref name="defaultValue"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetValueOrDefault<T>(this Option<T> source, T defaultValue) =>
        source.Fold(x => x, () => defaultValue);

    /// <summary>
    /// Returns the value of the <see cref="Option{T}.Some"/> or the result of the <paramref name="defaultValue"/> function
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetValueOrDefault<T>(this Option<T> source, Func<T> defaultValue) =>
        source.Fold(x => x, defaultValue);

    /// <summary>
    /// Converts the <see cref="Option{T}"/> to a nullable reference type.
    /// </summary>
    public static T? ToNullable<T>(this Option<T> source) where T : class =>
        source switch
        {
            Option<T>.Some some => some.Value,
            Option<T>.None => null,
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Converts the <see cref="Option{T}"/> to a nullable value type.
    /// </summary>
    public static T? ToNullableStruct<T>(this Option<T> source) where T : struct =>
        source switch
        {
            Option<T>.Some some => some.Value,
            Option<T>.None => null,
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Converts the <see cref="Option{T}"/> to an <see cref="Either{TLeft, TValue}"/>
    /// providing a function to create a suitable value for the <see cref="Either{TLeft, TValue}.Left"/> case.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Either<TLeft, TValue> ToEither<TLeft, TValue>(this Option<TValue> source, Func<TLeft> defaultLeft) =>
        source.Fold(Either.Right<TLeft, TValue>, () => Either.Left<TLeft, TValue>(defaultLeft()));

    /// <summary>
    /// Maps the value of a <see cref="Option{T}"/> into a new form.
    /// </summary>
    public static Option<TResult> Select<TSource, TResult>(
        this Option<TSource> source,
        Func<TSource, TResult> transform) =>
        source switch
        {
            Option<TSource>.Some some => Option.Some(transform(some.Value)),
            _ => Option.None<TResult>()
        };

    /// <summary>
    /// Projects the value of a <see cref="Option{T}"/> into a new form.
    /// </summary>
    public static Option<TResult> SelectMany<TSource, TResult>(
        this Option<TSource> source,
        Func<TSource, Option<TResult>> transform) =>
        source switch
        {
            Option<TSource>.Some some => transform(some.Value),
            _ => Option.None<TResult>()
        };

    /// <summary>
    /// Projects the value of a <see cref="Option{T}"/> into a new form.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TResult> SelectMany<TSource, TValue, TResult>(
        this Option<TSource> source,
        Func<TSource, Option<TValue>> binder,
        Func<TSource, TValue, TResult> selector) =>
        source.SelectMany(src => binder(src).Select(x => selector(src, x)));

    /// <summary>
    /// Filters the optional value based on a predicate.
    /// </summary>
    /// <returns>The original value if the <paramref name="predicate"/> matches the value
    /// inside <see cref="Option{_}"/>, otherwise <see cref="Option{_}.None"/>
    /// </returns>
    public static Option<T> Where<T>(this Option<T> source, Func<T, bool> predicate) =>
        source switch
        {
            Option<T>.Some some when predicate(some.Value) => source,
            _ => Option.None<T>()
        };

    /// <summary>
    /// Collects the values of the <see cref="Option{T}.Some"/> cases into a sequence.
    /// </summary>
    public static IEnumerable<T> CollectValues<T>(this IEnumerable<Option<T>> values)
    {
        foreach (var value in values)
        {
            switch (value)
            {
                case Option<T>.Some some:
                    yield return some.Value;
                    break;
            }
        }
    }

    /// <summary>
    /// Flattens the nested <see cref="Option{T}"/> into a single <see cref="Option{T}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Flatten<T>(this Option<Option<T>> value) => value.SelectMany(x => x);

    /// <summary>
    /// Traverses the function <paramref name="transform"/> over the <paramref name="source"/> sequence
    /// and returns an optional result when all the elements of the sequence are transformed.
    /// </summary>
    public static Option<IEnumerable<TResult>> Traverse<TSource, TResult>(
        this IEnumerable<TSource> source, Func<TSource, Option<TResult>> transform)
    {
        var results = new List<TResult>();
        foreach (var value in source)
        {
            switch (value)
            {
                case Option<TResult>.Some some:
                    results.Add(some.Value);
                    break;
                case Option<TResult>.None:
                    return Option.None<IEnumerable<TResult>>();
            }
        }

        return Option.Some(results.AsEnumerable());
    }

    /// <summary>
    /// Returns an optional sequence of elements if each element in the <paramref name="source"/>
    /// sequence is <see cref="Option{_}.Some"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<IEnumerable<T>> Sequence<T>(this IEnumerable<Option<T>> source) =>
        source.Traverse(x => x);
}
