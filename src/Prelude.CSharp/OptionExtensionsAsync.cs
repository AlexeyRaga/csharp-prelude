namespace Prelude;

/// <summary>
/// Provides a set of extension methods for <see cref="Option{_}"/> type.
/// </summary>
public static class OptionExtensionsAsync
{
    /// <summary>
    /// Maps the value of the <see cref="Option{T}"/> instance to another <see cref="Option{TResult}"/> instance
    /// </summary>
    public static async Task<Option<TResult>> Select<TSource, TResult>(
        this Option<TSource> source,
        Func<TSource, Task<TResult>> transform) =>
        source switch
        {
            Option<TSource>.Some some => Option.Some(await transform(some.Value)),
            _ => Option.None<TResult>()
        };

    /// <summary>
    /// Maps the value of the <see cref="Option{T}"/> instance to another <see cref="Option{TResult}"/> instance
    /// </summary>
    public static async Task<Option<TResult>> SelectMany<TSource, TResult>(
        this Option<TSource> source,
        Func<TSource, Task<Option<TResult>>> transform) =>
        source switch
        {
            Option<TSource>.Some some => await transform(some.Value),
            _ => Option.None<TResult>()
        };

    /// <summary>
    /// Maps the value of the <see cref="Option{T}"/> instance to another <see cref="Option{TResult}"/> instance
    /// </summary>
    public static Task<Option<TResult>> SelectMany<TSource, TValue, TResult>(
        this Option<TSource> source,
        Func<TSource, Task<Option<TValue>>> binder,
        Func<TSource, TValue, TResult> selector) =>
        source.SelectMany(async src => (await binder(src)).Select(x => selector(src, x)));



    /// <summary>
    /// Filters the value of the <see cref="Option{T}"/> instance based on a predicate
    /// </summary>
    public static async Task<Option<T>> Where<T>(this Option<T> source, Func<T, Task<bool>> predicate) =>
        source switch
        {
            Option<T>.Some some when await predicate(some.Value) => source,
            _ => Option.None<T>()
        };
}
