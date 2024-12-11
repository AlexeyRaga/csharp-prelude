using System.Runtime.CompilerServices;

namespace Prelude;

/// <summary>
/// Provides a set of extension methods for <see cref="Option{T}"/> type.
/// </summary>
public static class OptionExtensions
{
    /// <summary>
    /// Converts the <see cref="Option{T}"/> to a nullable reference type.
    /// </summary>
    /// <typeparam name="T">The type of the value in the option.</typeparam>
    /// <param name="source">The option to convert.</param>
    /// <returns>A nullable reference type representing the value inside the option or null if the option is empty.</returns>
    public static T? ToNullable<T>(this Option<T> source) where T : class =>
        source.Fold(T? (x) => x, () => null);

    /// <summary>
    /// Converts the <see cref="Option{T}"/> to a nullable value type.
    /// </summary>
    /// <typeparam name="T">The type of the value in the option.</typeparam>
    /// <param name="source">The option to convert.</param>
    /// <returns>A nullable value type representing the value inside the option or null if the option is empty.</returns>
    public static T? ToNullableStruct<T>(this Option<T> source) where T : struct =>
        source.Fold(T? (x) => x, () => null);

    /// <summary>
    /// Converts the <see cref="Option{T}"/> to an <see cref="Either{TLeft, TValue}"/>
    /// providing a function to create a suitable value for the <see cref="Either{TLeft, TValue}.Left"/> case.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TValue">The type of the right value in the option.</typeparam>
    /// <param name="source">The option to convert to either.</param>
    /// <param name="defaultLeft">A function that provides the left value if the option has no value.</param>
    /// <returns>An Either representing the option's value as the right value or the result of the default left function as the left value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Either<TLeft, TValue> ToEither<TLeft, TValue>(this Option<TValue> source, Func<TLeft> defaultLeft) =>
        source.Fold(Either.Right<TLeft, TValue>, () => Either.Left<TLeft, TValue>(defaultLeft()));

    /// <summary>
    /// Maps the value of a <see cref="Option{T}"/> into a new form.
    /// </summary>
    /// <typeparam name="TSource">The type of the value in the option.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The option to transform.</param>
    /// <param name="transform">The function that transforms the value of the option.</param>
    /// <returns>A new option with the transformed value or an empty option if the source is empty.</returns>
    public static Option<TResult> Select<TSource, TResult>(
        this Option<TSource> source,
        Func<TSource, TResult> transform) =>
        source.Fold(x => Option.Some(transform(x)), Option.None<TResult>);

    /// <summary>
    /// Projects the value of a <see cref="Option{T}"/> into a new form.
    /// </summary>
    /// <typeparam name="TSource">The type of the value in the option.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The option to project into a new form.</param>
    /// <param name="transform">The function that projects the option's value into a new form.</param>
    /// <returns>A new option resulting from the projection or an empty option if the source is empty.</returns>
    public static Option<TResult> SelectMany<TSource, TResult>(
        this Option<TSource> source,
        Func<TSource, Option<TResult>> transform) =>
        source.Fold(transform, Option.None<TResult>);

    /// <summary>
    /// Projects the value of a <see cref="Option{T}"/> into a new form.
    /// </summary>
    /// <typeparam name="TSource">The type of the value in the option.</typeparam>
    /// <typeparam name="TValue">The type of the intermediate result.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The option to project into a new form.</param>
    /// <param name="binder">The function that returns a new option.</param>
    /// <param name="selector">The function that combines the values of the original and new options.</param>
    /// <returns>A new option containing the result of the projection or an empty option if the source is empty.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TResult> SelectMany<TSource, TValue, TResult>(
        this Option<TSource> source,
        Func<TSource, Option<TValue>> binder,
        Func<TSource, TValue, TResult> selector) =>
        source.SelectMany(src => binder(src).Select(x => selector(src, x)));

    /// <summary>
    /// Filters the optional value based on a predicate.
    /// </summary>
    /// <typeparam name="T">The type of the value in the option.</typeparam>
    /// <param name="source">The option to filter.</param>
    /// <param name="predicate">The condition that the value must satisfy.</param>
    /// <returns>The original option if the value satisfies the predicate, otherwise an empty option.</returns>
    public static Option<T> Where<T>(this Option<T> source, Func<T, bool> predicate) =>
        source.Fold(x => predicate(x) ? source : Option.None<T>(), Option.None<T>);

    /// <summary>
    /// Collects the values of the <see cref="Option{T}.Some"/> cases into a sequence.
    /// </summary>
    /// <typeparam name="T">The type of the values in the options.</typeparam>
    /// <param name="source">The collection of options to extract values from.</param>
    /// <returns>An enumerable of the values inside the options that have values.</returns>
    public static IEnumerable<T> CollectValues<T>(this IEnumerable<Option<T>> source)
    {
        foreach (var value in source)
            if (value.IsSome) yield return value.UnsafeValue;
    }

    /// <summary>
    /// Flattens the nested <see cref="Option{T}"/> into a single <see cref="Option{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value in the option.</typeparam>
    /// <param name="source">The nested option to flatten.</param>
    /// <returns>A flattened option with the value or an empty option if the nested option is empty.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Flatten<T>(this Option<Option<T>> source) => source.SelectMany(x => x);

    /// <summary>
    /// Traverses the function <paramref name="transform"/> over the <paramref name="source"/> sequence
    /// and returns an optional result when all the elements of the sequence are transformed.
    /// </summary>
    /// <typeparam name="TSource">The type of the values in the source collection.</typeparam>
    /// <typeparam name="TResult">The type of the result values in the collection.</typeparam>
    /// <param name="source">The collection of values to transform.</param>
    /// <param name="transform">The function to apply to each value in the collection.</param>
    /// <returns>An option containing an enumerable of transformed values if all transformations succeed, otherwise an empty option.</returns>
    public static Option<IEnumerable<TResult>> Traverse<TSource, TResult>(
        this IEnumerable<TSource> source, Func<TSource, Option<TResult>> transform)
    {
        var results = new List<TResult>();
        foreach (var value in source)
        {
            if (transform(value) is { IsSome: true } some)
                results.Add(some.UnsafeValue);
            else
                return Option.None<IEnumerable<TResult>>();
        }

        return Option.Some(results.AsEnumerable());
    }

    /// <summary>
    /// Returns an optional sequence of elements if each element in the <paramref name="source"/>
    /// sequence is <see cref="Option{_}.Some"/>
    /// </summary>
    /// <typeparam name="T">The type of the values in the options.</typeparam>
    /// <param name="source">The collection of options to transform into a sequence.</param>
    /// <returns>An option containing an enumerable of values if all options have values, otherwise an empty option.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<IEnumerable<T>> Sequence<T>(this IEnumerable<Option<T>> source) =>
        source.Traverse(x => x);

    /// <summary>
    /// Combines two <see cref="Option{T}"/> instances using the specified combine function,
    /// returning a new <see cref="Option{T}"/> containing the result.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the <see cref="Option{T}"/>.</typeparam>
    /// <param name="first">
    /// The first <see cref="Option{T}"/> to combine.
    /// </param>
    /// <param name="second">
    /// The second <see cref="Option{T}"/> to combine.
    /// </param>
    /// <param name="combine">
    /// A function that specifies how to combine the values of <paramref name="first"/> and <paramref name="second"/>
    /// when both are in the "Some" state.
    /// </param>
    /// <returns>
    /// A new <see cref="Option{T}"/>:
    /// <list type="bullet">
    /// <item><description>
    /// If both <paramref name="first"/> and <paramref name="second"/> are in the "Some" state, returns a "Some" containing
    /// the result of the <paramref name="combine"/> function.
    /// </description></item>
    /// <item><description>
    /// If only one of the options is in the "Some" state, returns that option.
    /// </description></item>
    /// <item><description>
    /// If both options are in the "None" state, returns "None".
    /// </description></item>
    /// </list>
    /// </returns>
    public static Option<T> Combine<T>(this Option<T> first, Option<T> second, Func<T, T, T> combine)
    {
        return (first.TryGetValue(out var fst), second.TryGetValue(out var snd)) switch
        {
            (true, true) => Option.Some(combine(fst, snd)),
            (true, false) => first,
            (false, true) => second,
            _ => Option.None<T>()
        };
    }
}
