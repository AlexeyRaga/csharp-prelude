namespace Prelude;

/// <summary>
/// Provides a set of extension methods for <see cref="Either{_,_}"/> type.
/// </summary>
public static class EitherExtensionsAsync
{
    /// <summary>
    /// Maps the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    public static async Task<Either<TLeft, TResult>> Select<TLeft, TRight, TResult>(
        this Either<TLeft, TRight> either,
        Func<TRight, Task<TResult>> selector) =>
        either switch
        {
            Either<TLeft, TRight>.Left left => Either.Left<TLeft, TResult>(left.Value),
            Either<TLeft, TRight>.Right right => Either.Right<TLeft, TResult>(await selector(right.Value)),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    public static async Task<Either<TLeft, TResult>> Select<TLeft, TRight, TValue, TResult>(
        this Either<TLeft, TRight> either,
        Func<TRight, Task<Either<TLeft, TValue>>> eitherSelector,
        Func<TRight, TValue, TResult> resultSelector) =>
        either switch
        {
            Either<TLeft, TRight>.Left left => Either.Left<TLeft, TResult>(left.Value),
            Either<TLeft, TRight>.Right right => (await eitherSelector(right.Value)).Select(value =>
                resultSelector(right.Value, value)),
            _ => throw new InvalidOperationException("Invalid state")
        };


    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    public static Task<Either<TLeft, TResult>> Select<TLeft, TRight, TResult>(
        this Either<TLeft, TRight> either,
        Func<TRight, Task<Either<TLeft, TResult>>> selector) =>
        either.Select(selector, (_, x) => x);

    /// <summary>
    /// Maps the value of a <see cref="Either{_,_}.Left"/> into a new form.
    /// </summary>
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
    public static Task<Either<TResult, TRight>> SelectLeft<TResult, TLeft, TRight>(
        this Either<TLeft, TRight> either,
        Func<TLeft, Task<TResult>> selector) =>
        Task.FromResult(either).SelectLeft(selector);

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    public static async Task<Either<TLeft, TResult>> SelectMany<TLeft, TRight, TValue, TResult>(
        this Either<TLeft, TRight> either,
        Func<TRight, Task<Either<TLeft, TValue>>> eitherSelector,
        Func<TRight, TValue, TResult> resultSelector) =>
        either switch
        {
            Either<TLeft, TRight>.Left left => Either.Left<TLeft, TResult>(left.Value),
            Either<TLeft, TRight>.Right right =>
                (await eitherSelector(right.Value)).Select(value => resultSelector(right.Value, value)),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    public static async Task<Either<TResult, TRight>> SelectManyLeft<TLeft, TRight, TValue, TResult>(
        this Task<Either<TLeft, TRight>> either,
        Func<TLeft, Task<Either<TValue, TRight>>> eitherSelector,
        Func<TLeft, TValue, TResult> resultSelector) =>
        await either switch
        {
            Either<TLeft, TRight>.Left left =>
                (await eitherSelector(left.Value)).SelectLeft(value => resultSelector(left.Value, value)),
            Either<TLeft, TRight>.Right right => Either.Right<TResult, TRight>(right.Value),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    public static Task<Either<TResult, TRight>> SelectManyLeft<TLeft, TRight, TValue, TResult>(
        this Either<TLeft, TRight> either,
        Func<TLeft, Task<Either<TValue, TRight>>> eitherSelector,
        Func<TLeft, TValue, TResult> resultSelector) =>
        Task.FromResult(either).SelectManyLeft(eitherSelector, resultSelector);

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    public static Task<Either<TResult, TRight>> SelectManyLeft<TLeft, TRight, TResult>(
        this Task<Either<TLeft, TRight>> either,
        Func<TLeft, Task<Either<TResult, TRight>>> selector) =>
        either.SelectManyLeft(selector, (_, x) => x);

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    public static Task<Either<TResult, TRight>> SelectManyLeft<TLeft, TRight, TResult>(
        this Either<TLeft, TRight> either,
        Func<TLeft, Task<Either<TResult, TRight>>> selector) =>
        Task.FromResult(either).SelectManyLeft(selector);
}
