namespace Prelude.Experimental;

/// <summary>
/// Experimental Async extensions for <see cref="Either{_,_}"/> type.
/// </summary>
public static class EitherExtensionsAsync
{
    /// <summary>
    /// Maps the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    public static async Task<Either<TLeft, TResult>> Select<TLeft, TRight, TResult>(
        this Task<Either<TLeft, TRight>> either,
        Func<TRight, Task<TResult>> selector) =>
        await (await either).Select(selector);

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    public static async Task<Either<TLeft, TResult>> SelectMany<TLeft, TRight, TValue, TResult>(
        this Task<Either<TLeft, TRight>> either,
        Func<TRight, Task<Either<TLeft, TValue>>> eitherSelector,
        Func<TRight, TValue, TResult> resultSelector) =>
        await (await either).SelectMany(eitherSelector, resultSelector);

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    public static Task<Either<TLeft, TResult>> SelectMany<TLeft, TRight, TResult>(
        this Task<Either<TLeft, TRight>> either,
        Func<TRight, Task<Either<TLeft, TResult>>> selector) =>
        either.SelectMany(selector, (_, res) => res);
}
