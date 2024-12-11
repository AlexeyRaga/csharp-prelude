using System.Runtime.CompilerServices;

namespace Prelude;

/// <summary>
/// Provides a set of extension methods for <see cref="Either{_,_}"/> type.
/// </summary>
public static class EitherExtensionsAsync
{
    /// <summary>
    /// Maps the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The either instance to transform.</param>
    /// <param name="selector">The asynchronous function that transforms the right value.</param>
    /// <returns>A task representing the asynchronous operation. The task result is an either with the transformed right value or the original left value.</returns>
    public static async Task<Either<TLeft, TResult>> Select<TLeft, TRight, TResult>(
        this Either<TLeft, TRight> source,
        Func<TRight, Task<TResult>> selector) =>
        source switch
        {
            Either<TLeft, TRight>.Left left => Either.Left<TLeft, TResult>(left.Value),
            Either<TLeft, TRight>.Right right => Either.Right<TLeft, TResult>(await selector(right.Value)),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TValue">The type of the intermediate result.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The either instance to project into a new form.</param>
    /// <param name="eitherSelector">The asynchronous function that returns a new either.</param>
    /// <param name="resultSelector">The function that combines the right value and the intermediate result.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a new either with the final result or the original left value.</returns>
    public static async Task<Either<TLeft, TResult>> Select<TLeft, TRight, TValue, TResult>(
        this Either<TLeft, TRight> source,
        Func<TRight, Task<Either<TLeft, TValue>>> eitherSelector,
        Func<TRight, TValue, TResult> resultSelector) =>
        source switch
        {
            Either<TLeft, TRight>.Left left => Either.Left<TLeft, TResult>(left.Value),
            Either<TLeft, TRight>.Right right => (await eitherSelector(right.Value)).Select(value =>
                resultSelector(right.Value, value)),
            _ => throw new InvalidOperationException("Invalid state")
        };


    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The either instance to project into a new form.</param>
    /// <param name="selector">The asynchronous function that returns a new either.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a new either with the final result or the original left value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Either<TLeft, TResult>> Select<TLeft, TRight, TResult>(
        this Either<TLeft, TRight> source,
        Func<TRight, Task<Either<TLeft, TResult>>> selector) =>
        source.Select(selector, (_, x) => x);

    /// <summary>
    /// Maps the value of a <see cref="Either{_,_}.Left"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TResult">The type of the transformed left value.</typeparam>
    /// <param name="either">The task containing the either instance to transform.</param>
    /// <param name="selector">The asynchronous function that transforms the left value.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a new either with the transformed left value or the original right value.</returns>
    public static async Task<Either<TResult, TRight>> SelectLeft<TResult, TLeft, TRight>(
        this Task<Either<TLeft, TRight>> either,
        Func<TLeft, Task<TResult>> selector) =>
        await either switch
        {
            Either<TLeft, TRight>.Left left => Either.Left<TResult, TRight>(await selector(left.Value)),
            Either<TLeft, TRight>.Right right => Either.Right<TResult, TRight>(right.Value),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Maps the value of a <see cref="Either{_,_}.Left"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TResult">The type of the transformed left value.</typeparam>
    /// <param name="source">The either instance to transform.</param>
    /// <param name="selector">The asynchronous function that transforms the left value.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a new either with the transformed left value or the original right value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Either<TResult, TRight>> SelectLeft<TResult, TLeft, TRight>(
        this Either<TLeft, TRight> source,
        Func<TLeft, Task<TResult>> selector) =>
        Task.FromResult(source).SelectLeft(selector);

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TValue">The type of the intermediate result.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The either instance to project into a new form.</param>
    /// <param name="eitherSelector">The asynchronous function that returns a new either.</param>
    /// <param name="resultSelector">The function that combines the right value and the intermediate result.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a new either with the final result or the original left value.</returns>
    public static async Task<Either<TLeft, TResult>> SelectMany<TLeft, TRight, TValue, TResult>(
        this Either<TLeft, TRight> source,
        Func<TRight, Task<Either<TLeft, TValue>>> eitherSelector,
        Func<TRight, TValue, TResult> resultSelector) =>
        source switch
        {
            Either<TLeft, TRight>.Left left => Either.Left<TLeft, TResult>(left.Value),
            Either<TLeft, TRight>.Right right =>
                (await eitherSelector(right.Value)).Select(value => resultSelector(right.Value, value)),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The either instance to project into a new form.</param>
    /// <param name="eitherSelector">The asynchronous function that returns a new either.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a new either with the final result or the original left value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Either<TLeft, TResult>> SelectMany<TLeft, TRight, TResult>(
        this Either<TLeft, TRight> source,
        Func<TRight, Task<Either<TLeft, TResult>>> eitherSelector) =>
        await source.SelectMany(eitherSelector, (_, x) => x);

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TValue">The type of the intermediate result.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The either instance to project into a new form.</param>
    /// <param name="eitherSelector">The asynchronous function that returns a new either.</param>
    /// <param name="resultSelector">The function that combines the left value and the intermediate result.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a new either with the final result or the original right value.</returns>
    public static async Task<Either<TResult, TRight>> SelectManyLeft<TLeft, TRight, TValue, TResult>(
        this Either<TLeft, TRight> source,
        Func<TLeft, Task<Either<TValue, TRight>>> eitherSelector,
        Func<TLeft, TValue, TResult> resultSelector) =>
        source switch {
            Either<TLeft, TRight>.Left left =>
                (await eitherSelector(left.Value)).SelectLeft(value => resultSelector(left.Value, value)),
            Either<TLeft, TRight>.Right right =>
                Either.Right<TResult, TRight>(right.Value),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The either instance to project into a new form.</param>
    /// <param name="selector">The asynchronous function that returns a new either.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a new either with the final result or the original right value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Either<TResult, TRight>> SelectManyLeft<TLeft, TRight, TResult>(
        this Either<TLeft, TRight> source,
        Func<TLeft, Task<Either<TResult, TRight>>> selector) =>
        source.SelectManyLeft(selector, (_, x) => x);

    /// <summary>
    /// Filters the Right value based on a predicate.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="source">The either instance to filter.</param>
    /// <param name="predicate">The asynchronous condition to check for the right value.</param>
    /// <param name="leftValue">The value to return if the predicate fails.</param>
    /// <returns>The original value if the <paramref name="predicate"/> matches the Right value
    /// inside <see cref="Either{_,_}"/>, otherwise <see cref="Either{_,_}.Left"/>
    /// </returns>
    public static async Task<Either<TLeft, TRight>> Satisfy<TLeft, TRight>(
        this Either<TLeft, TRight> source,
        Func<TRight, Task<bool>> predicate,
        TLeft leftValue) =>
        source switch
        {
            Either<TLeft, TRight>.Right right when await predicate(right.Value) => source,
            _ => Either.Left<TLeft, TRight>(leftValue)
        };
}
