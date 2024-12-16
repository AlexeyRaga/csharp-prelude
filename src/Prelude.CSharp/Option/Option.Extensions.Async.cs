namespace Prelude;

/// <summary>
/// Provides a set of extension methods for <see cref="Option{_}"/> type.
/// </summary>
public static class OptionExtensionsAsync
{
    /// <summary>
    /// Maps the value of the <see cref="Option{T}"/> instance to another <see cref="Option{TResult}"/> instance
    /// </summary>
    /// <typeparam name="TSource">The type of the value in the option.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The option to transform.</param>
    /// <param name="transform">The asynchronous function that transforms the value of the option.</param>
    /// <returns>A task representing the asynchronous operation. The task result is an option containing the transformed value or an empty option if the source is empty.</returns>
    public static async Task<Option<TResult>> Select<TSource, TResult>(
        this Option<TSource> source,
        Func<TSource, Task<TResult>> transform) =>
        source switch
        {
            {IsSome: true} some => Option.Some(await transform(some.UnsafeValue)),
            _ => Option.None<TResult>()
        };

    /// <summary>
    /// Maps the value of the <see cref="Option{T}"/> instance to another <see cref="Option{TResult}"/> instance
    /// </summary>
    /// <typeparam name="TSource">The type of the value in the option.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The option to project into a new form.</param>
    /// <param name="transform">The asynchronous function that projects the option's value into a new form.</param>
    /// <returns>A task representing the asynchronous operation. The task result is an option resulting from the projection or an empty option if the source is empty.</returns>
    public static async Task<Option<TResult>> SelectMany<TSource, TResult>(
        this Option<TSource> source,
        Func<TSource, Task<Option<TResult>>> transform) =>
        source switch
        {
            {IsSome: true} some => await transform(some.UnsafeValue),
            _ => Option.None<TResult>()
        };

    /// <summary>
    /// Maps the value of the <see cref="Option{T}"/> instance to another <see cref="Option{TResult}"/> instance
    /// </summary>
    /// <typeparam name="TSource">The type of the value in the option.</typeparam>
    /// <typeparam name="TValue">The type of the intermediate result.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The option to project into a new form.</param>
    /// <param name="binder">The asynchronous function that returns a new option.</param>
    /// <param name="selector">The function that combines the values of the original and new options.</param>
    /// <returns>A task representing the asynchronous operation. The task result is an option containing the final result or an empty option if the source is empty.</returns>
    public static Task<Option<TResult>> SelectMany<TSource, TValue, TResult>(
        this Option<TSource> source,
        Func<TSource, Task<Option<TValue>>> binder,
        Func<TSource, TValue, TResult> selector) =>
        source.SelectMany(async src => (await binder(src)).Select(x => selector(src, x)));



    /// <summary>
    /// Filters the value of the <see cref="Option{T}"/> instance based on a predicate
    /// </summary>
    /// <typeparam name="T">The type of the value in the option.</typeparam>
    /// <param name="source">The option to filter.</param>
    /// <param name="predicate">The asynchronous condition that the value must satisfy.</param>
    /// <returns>A task representing the asynchronous operation. The task result is the original option if the value satisfies the predicate, otherwise an empty option.</returns>
    public static async Task<Option<T>> Where<T>(this Option<T> source, Func<T, Task<bool>> predicate) =>
        source switch
        {
            {IsSome: true} some when await predicate(some.UnsafeValue) => source,
            _ => Option.None<T>()
        };
}
