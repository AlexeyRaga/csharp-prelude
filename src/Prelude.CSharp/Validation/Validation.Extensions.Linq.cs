using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Prelude;

/// <summary>
/// Provides LINQ extension methods for the <see cref="Validation{TError, TSuccess}"/> type.
/// </summary>
public static class ValidationLinqExtensions
{
    /// <summary>
    /// Projects the success value of a <see cref="Validation{TError, TSuccess}"/> into a new form.
    /// </summary>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <typeparam name="TSuccess">The type of the success value.</typeparam>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <param name="validation">The validation instance.</param>
    /// <param name="selector">A transform function to apply to the success value.</param>
    /// <returns>A <see cref="Validation{TError, TResult}"/> containing the result of applying the transform function.</returns>
    public static Validation<TError, TResult> Select<TError, TSuccess, TResult>(
        this Validation<TError, TSuccess> validation,
        Func<TSuccess, TResult> selector) => new(validation.Either.Select(selector));

    /// <summary>
    /// Flattens a nested <see cref="Validation{TLeft, TRight}"/> structure.
    /// </summary>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <typeparam name="TResult">The type of the success value.</typeparam>
    /// <param name="source">The nested validation instance.</param>
    /// <returns>A flattened <see cref="Validation{TLeft, TRight}"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Validation<TError, TResult> Flatten<TError, TResult>(
        this Validation<TError, Validation<TError, TResult>> source) => source.Bind(x => x);

    /// <summary>
    /// Flattens a <see cref="Validation{TError, TSuccess}"/> with a collection of errors into a single validation.
    /// </summary>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <typeparam name="TSuccess">The type of the success value.</typeparam>
    /// <param name="source">The validation instance with a collection of errors.</param>
    /// <returns>A <see cref="Validation{TError, TSuccess}"/> with a flattened collection of errors.</returns>
    public static Validation<TError, TSuccess> FlattenErrors<TError, TSuccess>(
        this Validation<IEnumerable<TError>, TSuccess> source) =>
            new(source.Either.SelectLeft(x => x.SelectMany(y => y).ToImmutableList()));

    /// <summary>
    /// Projects the error value of a <see cref="Validation{TLeft, TRight}"/> into a new form.
    /// </summary>
    /// <typeparam name="TNewError">The type of the result error value.</typeparam>
    /// <typeparam name="TError">The type of the original error.</typeparam>
    /// <typeparam name="TSuccess">The type of the success value.</typeparam>
    /// <param name="source">The validation instance.</param>
    /// <param name="selector">A transform function to apply to the error value.</param>
    /// <returns>A <see cref="Validation{TResult, TRight}"/> containing the result of applying the transform function.</returns>
    public static Validation<TNewError, TSuccess> SelectError<TNewError, TError, TSuccess>(
        this Validation<TError, TSuccess> source,
        Func<TError, TNewError> selector) => new(source.Either.SelectLeft(x => x.Select(selector).ToImmutableList()));
}
