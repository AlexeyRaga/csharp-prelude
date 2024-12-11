using System.Runtime.CompilerServices;

namespace Prelude;

public static class TaskExtensions
{
    /// <typeparam name="TSource">The type of the value in the task.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The task to transform.</param>
    /// <param name="transform">The function that transforms the result of the task.</param>
    /// <returns>A task representing the asynchronous operation. The task result is the transformed value.</returns>
    public static async Task<TResult> Select<TSource, TResult>(
        this Task<TSource> source,
        Func<TSource, TResult> transform) =>
        transform(await source);

    /// <typeparam name="TSource">The type of the value in the task.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The task to transform.</param>
    /// <param name="transform">The function that transforms the result of the task.</param>
    /// <returns>A task representing the asynchronous operation. The task result is the transformed value.</returns>
    public static async Task<TResult> Select<TSource, TResult>(
        this ConfiguredTaskAwaitable<TSource> source,
        Func<TSource, TResult> transform) =>
        transform(await source);

    /// <typeparam name="TSource">The type of the value in the task.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The task to project into a new form.</param>
    /// <param name="transform">The asynchronous function that returns a new task.</param>
    /// <returns>A task representing the asynchronous operation. The task result is the result of the projection.</returns>
    public static async Task<TResult> SelectMany<TSource, TResult>(
        this Task<TSource> source,
        Func<TSource, Task<TResult>> transform) =>
        await transform(await source);

    /// <typeparam name="TSource">The type of the value in the task.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The task to project into a new form.</param>
    /// <param name="transform">The asynchronous function that returns a new task.</param>
    /// <returns>A task representing the asynchronous operation. The task result is the result of the projection.</returns>
    public static async Task<TResult> SelectMany<TSource, TResult>(
        this ConfiguredTaskAwaitable<TSource> source,
        Func<TSource, Task<TResult>> transform) =>
        await transform(await source);

    /// <typeparam name="TSource">The type of the value in the first task.</typeparam>
    /// <typeparam name="TValue">The type of the intermediate result.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The task to project into a new form.</param>
    /// <param name="binder">The asynchronous function that returns a new task.</param>
    /// <param name="selector">The function that combines the results of the original and new tasks.</param>
    /// <returns>A task representing the asynchronous operation. The task result is the final result.</returns>
    public static async Task<TResult> SelectMany<TSource, TValue, TResult>(
        this Task<TSource> source,
        Func<TSource, Task<TValue>> binder,
        Func<TSource, TValue, TResult> selector) =>
        selector(await source, await binder(await source));

    /// <typeparam name="TSource">The type of the value in the first task.</typeparam>
    /// <typeparam name="TValue">The type of the intermediate result.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The task to project into a new form.</param>
    /// <param name="binder">The asynchronous function that returns a new task.</param>
    /// <param name="selector">The function that combines the results of the original and new tasks.</param>
    /// <returns>A task representing the asynchronous operation. The task result is the final result.</returns>
    public static async Task<TResult> SelectMany<TSource, TValue, TResult>(
        this ConfiguredTaskAwaitable<TSource> source,
        Func<TSource, Task<TValue>> binder,
        Func<TSource, TValue, TResult> selector) =>
        selector(await source, await binder(await source));

    /// <typeparam name="TSource">The type of the values in the source collection.</typeparam>
    /// <typeparam name="TResult">The type of the result values.</typeparam>
    /// <param name="source">The collection of values to transform.</param>
    /// <param name="transform">The asynchronous function that transforms each value in the collection.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a collection of transformed values.</returns>
    public static async Task<IEnumerable<TResult>> Traverse<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, Task<TResult>> transform) =>
        await Task.WhenAll(source.Select(transform));

    /// <typeparam name="T">The type of the values in the tasks.</typeparam>
    /// <param name="source">The collection of tasks to execute.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a collection of results from the tasks.</returns>
    public static async Task<IEnumerable<T>> Sequence<T>(this IEnumerable<Task<T>> source) =>
        await Task.WhenAll(source);
}
