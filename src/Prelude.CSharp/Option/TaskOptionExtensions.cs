using System.Runtime.CompilerServices;

namespace Prelude;

/// <summary>
/// Experimental Async extensions for <see cref="Option{T}"/> type.
/// </summary>
public static class TaskOptionExtensions
{
    /// <summary>
    /// Maps the value of the <see cref="Option{T}"/> instance to another <see cref="Option{TResult}"/> instance
    /// </summary>
    /// <typeparam name="TSource">The type of the value inside the option.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The task containing the option to transform.</param>
    /// <param name="transform">The asynchronous function that transforms the value inside the option.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a new option with the transformed value or None if the source is None.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Option<TResult>> Select<TSource, TResult>(
        this Task<Option<TSource>> source,
        Func<TSource, Task<TResult>> transform) =>
        await (await source).Select(transform);

    /// <summary>
    /// Maps the value of the <see cref="Option{T}"/> instance to another <see cref="Option{TResult}"/> instance
    /// </summary>
    /// <typeparam name="TSource">The type of the value inside the option.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The configured task containing the option to transform.</param>
    /// <param name="transform">The asynchronous function that transforms the value inside the option.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a new option with the transformed value or None if the source is None.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Option<TResult>> Select<TSource, TResult>(
        this ConfiguredTaskAwaitable<Option<TSource>> source,
        Func<TSource, Task<TResult>> transform) =>
        await (await source).Select(transform);

    /// <summary>
    /// Maps the value of the <see cref="Option{T}"/> instance to another <see cref="Option{TResult}"/> instance
    /// </summary>
    /// <typeparam name="TSource">The type of the value inside the option.</typeparam>
    /// <typeparam name="TValue">The type of the intermediate result.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The task containing the option to project into a new form.</param>
    /// <param name="binder">The asynchronous function that returns a new option.</param>
    /// <param name="selector">The function that combines the original value and the result of the projection.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a new option with the final result or None if the source is None.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Option<TResult>> SelectMany<TSource, TValue, TResult>(
        this Task<Option<TSource>> source,
        Func<TSource, Task<Option<TValue>>> binder,
        Func<TSource, TValue, TResult> selector) =>
        await (await source).SelectMany(binder, selector);

    /// <summary>
    /// Maps the value of the <see cref="Option{T}"/> instance to another <see cref="Option{TResult}"/> instance
    /// </summary>
    /// <typeparam name="TSource">The type of the value inside the option.</typeparam>
    /// <typeparam name="TValue">The type of the intermediate result.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The configured task containing the option to project into a new form.</param>
    /// <param name="binder">The asynchronous function that returns a new option.</param>
    /// <param name="selector">The function that combines the original value and the result of the projection.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a new option with the final result or None if the source is None.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Option<TResult>> SelectMany<TSource, TValue, TResult>(
        this ConfiguredTaskAwaitable<Option<TSource>> source,
        Func<TSource, Task<Option<TValue>>> binder,
        Func<TSource, TValue, TResult> selector) =>
        await (await source).SelectMany(binder, selector);

    /// <summary>
    /// Maps the value of the <see cref="Option{T}"/> instance to another <see cref="Option{TResult}"/> instance
    /// </summary>
    /// <typeparam name="TSource">The type of the value inside the option.</typeparam>
    /// <typeparam name="TValue">The type of the intermediate result.</typeparam>
    /// <param name="source">The task containing the option to project into a new form.</param>
    /// <param name="binder">The asynchronous function that returns a new option.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a new option with the intermediate result or None if the source is None.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Option<TValue>> SelectMany<TSource, TValue>(
        this Task<Option<TSource>> source,
        Func<TSource, Task<Option<TValue>>> binder) =>
        source.SelectMany(binder, (_, res) => res);

    /// <summary>
    /// Maps the value of the <see cref="Option{T}"/> instance to another <see cref="Option{TResult}"/> instance
    /// </summary>
    /// <typeparam name="TSource">The type of the value inside the option.</typeparam>
    /// <typeparam name="TValue">The type of the intermediate result.</typeparam>
    /// <param name="source">The configured task containing the option to project into a new form.</param>
    /// <param name="binder">The asynchronous function that returns a new option.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a new option with the intermediate result or None if the source is None.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Option<TValue>> SelectMany<TSource, TValue>(
        this ConfiguredTaskAwaitable<Option<TSource>> source,
        Func<TSource, Task<Option<TValue>>> binder) =>
        source.SelectMany(binder, (_, res) => res);

    /// <summary>
    /// Filters the value of the <see cref="Option{T}"/> instance based on a predicate
    /// </summary>
    /// <typeparam name="T">The type of the value inside the option.</typeparam>
    /// <param name="source">The task containing the option to filter.</param>
    /// <param name="predicate">The asynchronous function that determines whether the value satisfies the condition.</param>
    /// <returns>A task representing the asynchronous operation. The task result is the original option if the value satisfies the condition, otherwise None.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Option<T>> Where<T>(
        this Task<Option<T>> source,
        Func<T, Task<bool>> predicate) =>
        await (await source).Where(predicate);

    /// <summary>
    /// Filters the value of the <see cref="Option{T}"/> instance based on a predicate
    /// </summary>
    /// <typeparam name="T">The type of the value inside the option.</typeparam>
    /// <param name="source">The configured task containing the option to filter.</param>
    /// <param name="predicate">The asynchronous function that determines whether the value satisfies the condition.</param>
    /// <returns>A task representing the asynchronous operation. The task result is the original option if the value satisfies the condition, otherwise None.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Option<T>> Where<T>(
        this ConfiguredTaskAwaitable<Option<T>> source,
        Func<T, Task<bool>> predicate) =>
        await (await source).Where(predicate);

    /// <summary>
    /// Filters the value of the <see cref="Option{T}"/> instance based on a predicate
    /// </summary>
    /// <typeparam name="T">The type of the value inside the option.</typeparam>
    /// <param name="source">The task containing the option to filter.</param>
    /// <param name="predicate">The function that determines whether the value satisfies the condition.</param>
    /// <returns>A task representing the asynchronous operation. The task result is the original option if the value satisfies the condition, otherwise None.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Option<T>> Where<T>(
        this Task<Option<T>> source,
        Func<T, bool> predicate) =>
        (await source).Where(predicate);

    /// <summary>
    /// Filters the value of the <see cref="Option{T}"/> instance based on a predicate
    /// </summary>
    /// <typeparam name="T">The type of the value inside the option.</typeparam>
    /// <param name="source">The configured task containing the option to filter.</param>
    /// <param name="predicate">The function that determines whether the value satisfies the condition.</param>
    /// <returns>A task representing the asynchronous operation. The task result is the original option if the value satisfies the condition, otherwise None.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Option<T>> Where<T>(
        this ConfiguredTaskAwaitable<Option<T>> source,
        Func<T, bool> predicate) =>
        (await source).Where(predicate);
}
