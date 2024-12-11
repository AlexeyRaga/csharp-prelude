using System.Runtime.CompilerServices;

namespace Prelude;

/// <summary>
/// Experimental Async extensions for <see cref="Either{_,_}"/> type.
/// </summary>
public static class TaskEitherExtensions
{
    /// <summary>
    /// Maps the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The task containing the either instance to transform.</param>
    /// <param name="selector">The asynchronous function that transforms the right value.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result is <see cref="Either{_,_}"/> with the transformed right value or the original left value.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Either<TLeft, TResult>> Select<TLeft, TRight, TResult>(
        this Task<Either<TLeft, TRight>> source,
        Func<TRight, Task<TResult>> selector) =>
        await (await source).Select(selector);

    /// <summary>
    /// Maps the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The configured task containing the either instance to transform.</param>
    /// <param name="selector">The asynchronous function that transforms the right value.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result is <see cref="Either{_,_}"/> with the transformed right value or the original left value.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Either<TLeft, TResult>> Select<TLeft, TRight, TResult>(
        this ConfiguredTaskAwaitable<Either<TLeft, TRight>> source,
        Func<TRight, Task<TResult>> selector) =>
        await (await source).Select(selector);

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TValue">The type of the intermediate result.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The task containing the either instance to project into a new form.</param>
    /// <param name="eitherSelector">The asynchronous function that returns a new either.</param>
    /// <param name="resultSelector">The function that combines the right value and the intermediate result.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result is a new <see cref="Either{_,_}"/> with the final result or the original left value.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Either<TLeft, TResult>> SelectMany<TLeft, TRight, TValue, TResult>(
        this Task<Either<TLeft, TRight>> source,
        Func<TRight, Task<Either<TLeft, TValue>>> eitherSelector,
        Func<TRight, TValue, TResult> resultSelector) =>
        await (await source).SelectMany(eitherSelector, resultSelector);

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TValue">The type of the intermediate result.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The configured task containing the either instance to project into a new form.</param>
    /// <param name="eitherSelector">The asynchronous function that returns a new either.</param>
    /// <param name="resultSelector">The function that combines the right value and the intermediate result.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result is a new <see cref="Either{_,_}"/> with the final result or the original left value.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Either<TLeft, TResult>> SelectMany<TLeft, TRight, TValue, TResult>(
        this ConfiguredTaskAwaitable<Either<TLeft, TRight>> source,
        Func<TRight, Task<Either<TLeft, TValue>>> eitherSelector,
        Func<TRight, TValue, TResult> resultSelector) =>
        await (await source).SelectMany(eitherSelector, resultSelector);

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The task containing the either instance to project into a new form.</param>
    /// <param name="selector">The asynchronous function that returns a new either.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result is a new <see cref="Either{_,_}"/> with the final result or the original left value.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Either<TLeft, TResult>> SelectMany<TLeft, TRight, TResult>(
        this Task<Either<TLeft, TRight>> source,
        Func<TRight, Task<Either<TLeft, TResult>>> selector) =>
        source.SelectMany(selector, (_, res) => res);

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The configured task containing the either instance to project into a new form.</param>
    /// <param name="selector">The asynchronous function that returns a new either.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result is a new <see cref="Either{_,_}"/> with the final result or the original left value.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Either<TLeft, TResult>> SelectMany<TLeft, TRight, TResult>(
        this ConfiguredTaskAwaitable<Either<TLeft, TRight>> source,
        Func<TRight, Task<Either<TLeft, TResult>>> selector) =>
        source.SelectMany(selector, (_, res) => res);

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TValue">The type of the intermediate result.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The task containing the either instance to project into a new form.</param>
    /// <param name="eitherSelector">The asynchronous function that returns a new either for the left value.</param>
    /// <param name="resultSelector">The function that combines the left value and the intermediate result.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result is a new <see cref="Either{_,_}"/> with the final result or the original right value.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Either<TResult, TRight>> SelectManyLeft<TLeft, TRight, TValue, TResult>(
        this Task<Either<TLeft, TRight>> source,
        Func<TLeft, Task<Either<TValue, TRight>>> eitherSelector,
        Func<TLeft, TValue, TResult> resultSelector) =>
        await (await source).SelectManyLeft(eitherSelector, resultSelector);

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TValue">The type of the intermediate result.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The configured task containing the either instance to project into a new form.</param>
    /// <param name="eitherSelector">The asynchronous function that returns a new either for the left value.</param>
    /// <param name="resultSelector">The function that combines the left value and the intermediate result.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result is a new <see cref="Either{_,_}"/> with the final result or the original right value.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Either<TResult, TRight>> SelectManyLeft<TLeft, TRight, TValue, TResult>(
        this ConfiguredTaskAwaitable<Either<TLeft, TRight>> source,
        Func<TLeft, Task<Either<TValue, TRight>>> eitherSelector,
        Func<TLeft, TValue, TResult> resultSelector) =>
        await (await source).SelectManyLeft(eitherSelector, resultSelector);

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The task containing the either instance to project into a new form.</param>
    /// <param name="selector">The asynchronous function that returns a new either for the left value.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result is a new <see cref="Either{_,_}"/> with the final result or the original right value.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Either<TResult, TRight>> SelectManyLeft<TLeft, TRight, TResult>(
        this Task<Either<TLeft, TRight>> source,
        Func<TLeft, Task<Either<TResult, TRight>>> selector) =>
        source.SelectManyLeft(selector, (_, x) => x);

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The configured task containing the either instance to project into a new form.</param>
    /// <param name="selector">The asynchronous function that returns a new either for the left value.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result is a new <see cref="Either{_,_}"/> with the final result or the original right value.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Either<TResult, TRight>> SelectManyLeft<TLeft, TRight, TResult>(
        this ConfiguredTaskAwaitable<Either<TLeft, TRight>> source,
        Func<TLeft, Task<Either<TResult, TRight>>> selector) =>
        source.SelectManyLeft(selector, (_, x) => x);

    /// <summary>
    /// Converts possibly null value to an <see cref="Either{_,_}"/>
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value, which is a reference type.</typeparam>
    /// <param name="source">The task containing a possibly null right value.</param>
    /// <param name="leftValue">The value to use for the left case if the right value is null.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result is <see cref="Either{_,_}"/> with the right value or the left value if the right value is null.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Either<TLeft, TRight>> ToEitherAsync<TLeft, TRight>(
        this Task<TRight?> source,
        TLeft leftValue) where TRight : class =>
        Either.FromNullable(leftValue, await source);

    /// <summary>
    /// Converts possibly null value to an <see cref="Either{_,_}"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Either<TLeft, TRight>> ToEitherAsync<TLeft, TRight>(
        this ConfiguredTaskAwaitable<TRight?> source,
        TLeft leftValue) where TRight : class =>
        Either.FromNullable(leftValue, await source);

    /// <summary>
    /// Converts possibly null value to an <see cref="Either{_,_}"/>
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value, which is a reference type.</typeparam>
    /// <param name="source">The configured task containing a possibly null right value.</param>
    /// <param name="leftValue">The value to use for the left case if the right value is null.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result is <see cref="Either{_,_}"/> with the right value or the left value if the right value is null.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Either<TLeft, TRight>> ToEitherAsync<TLeft, TRight>(
        this Task<TRight?> source,
        TLeft leftValue) where TRight : struct =>
        Either.FromNullable(leftValue, await source);

    /// <summary>
    /// Converts possibly null value to an <see cref="Either{_,_}"/>
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value, which is a value type.</typeparam>
    /// <param name="source">The task containing a nullable right value.</param>
    /// <param name="leftValue">The value to use for the left case if the right value is null.</param>
    /// <returns>A task representing the asynchronous operation.
    /// The task result is <see cref="Either{_,_}"/> with the right value or the left value if the right value is null.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Either<TLeft, TRight>> ToEitherAsync<TLeft, TRight>(
        this ConfiguredTaskAwaitable<TRight?> source,
        TLeft leftValue) where TRight : struct =>
        Either.FromNullable(leftValue, await source);

    /// <summary>
    /// Filters the Right value based on a predicate.
    /// </summary>
    /// <returns>The original value if the <paramref name="predicate"/> matches the Right value
    /// inside <see cref="Either{_,_}"/>, otherwise <see cref="Either{_,_}.Left"/>
    /// </returns>
    ///
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Either<TLeft, TRight>> Satisfy<TLeft, TRight>(
        this Task<Either<TLeft, TRight>> source,
        Func<TRight, bool> predicate,
        TLeft leftValue) =>
        (await source).Satisfy(predicate, leftValue);

    /// <summary>
    /// Filters the Right value based on a predicate.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="source">The task containing the either instance to filter.</param>
    /// <param name="predicate">The condition to check for the right value.</param>
    /// <param name="leftValue">The value to use for the left case if the predicate fails.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result is the original <see cref="Either{_,_}"/> if the right value satisfies the predicate,
    /// otherwise a left with the specified value.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Either<TLeft, TRight>> Satisfy<TLeft, TRight>(
        this ConfiguredTaskAwaitable<Either<TLeft, TRight>> source,
        Func<TRight, bool> predicate,
        TLeft leftValue) =>
        (await source).Satisfy(predicate, leftValue);

    /// <summary>
    /// Filters the Right value based on a predicate.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="source">The configured task containing the either instance to filter.</param>
    /// <param name="predicate">The condition to check for the right value.</param>
    /// <param name="leftValue">The value to use for the left case if the predicate fails.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result is the original <see cref="Either{_,_}"/> if the right value satisfies the predicate,
    /// otherwise a left with the specified value.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Either<TLeft, TRight>> Satisfy<TLeft, TRight>(
        this Task<Either<TLeft, TRight>> source,
        Func<TRight, Task<bool>> predicate,
        TLeft leftValue) =>
        await (await source).Satisfy(predicate, leftValue);

    /// <summary>
    /// Filters the Right value based on a predicate.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="source">The configured task containing the either instance to filter.</param>
    /// <param name="predicate">The asynchronous condition to check for the right value.</param>
    /// <param name="leftValue">The value to use for the left case if the predicate fails.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result is the original <see cref="Either{_,_}"/> if the right value satisfies the predicate,
    /// otherwise a left with the specified value.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Either<TLeft, TRight>> Satisfy<TLeft, TRight>(
        this ConfiguredTaskAwaitable<Either<TLeft, TRight>> source,
        Func<TRight, Task<bool>> predicate,
        TLeft leftValue) =>
        await (await source).Satisfy(predicate, leftValue);
}
