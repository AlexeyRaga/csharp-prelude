namespace Prelude.Experimental;

/// <summary>
/// Experimental Async extensions for <see cref="Option{T}"/> type.
/// </summary>
public static class OptionExtensionsAsync
{
    /// <summary>
    /// Maps the value of the <see cref="Option{T}"/> instance to another <see cref="Option{TResult}"/> instance
    /// </summary>
    public static async Task<Option<TResult>> Select<TSource, TResult>(
        this Task<Option<TSource>> source,
        Func<TSource, Task<TResult>> transform) =>
        await (await source).Select(transform);

    /// <summary>
    /// Maps the value of the <see cref="Option{T}"/> instance to another <see cref="Option{TResult}"/> instance
    /// </summary>
    public static async Task<Option<TResult>> SelectMany<TSource, TValue, TResult>(
        this Task<Option<TSource>> source,
        Func<TSource, Task<Option<TValue>>> binder,
        Func<TSource, TValue, TResult> selector) =>
        await (await source).SelectMany(binder, selector);

    /// <summary>
    /// Maps the value of the <see cref="Option{T}"/> instance to another <see cref="Option{TResult}"/> instance
    /// </summary>
    public static Task<Option<TValue>> SelectMany<TSource, TValue>(
        this Task<Option<TSource>> source,
        Func<TSource, Task<Option<TValue>>> binder) =>
        source.SelectMany(binder, (_, res) => res);

    /// <summary>
    /// Filters the value of the <see cref="Option{T}"/> instance based on a predicate
    /// </summary>
    public static async Task<Option<T>> WhereAsync<T>(
        this Task<Option<T>> source,
        Func<T, Task<bool>> predicate) =>
        await (await source).Where(predicate);
}
