using System.Collections.Immutable;

namespace Prelude;

/// <summary>
/// The <see ref="Validation{TError, TSuccess}" /> is like an <see ref="Either{TLeft, TRight}" />
/// with an ability to accumulate errors.
/// </summary>
/// <typeparam name="TError">The type of the error.</typeparam>
/// <typeparam name="TSuccess">The type of the success value.</typeparam>
/// <param name="Either">The underlying Either value.</param>
public record Validation<TError, TSuccess>(Either<ImmutableList<TError>, TSuccess> Either)
{
    /// <summary>
    /// Folds the validation into a single value.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="success">Function to handle the success case.</param>
    /// <param name="error">Function to handle the error case.</param>
    /// <returns>The result of applying the appropriate function.</returns>
    public T Fold<T>(Func<TSuccess, T> success, Func<ImmutableList<TError>, T> error) =>
        Either.Fold(right: success, left: error);
}

/// <summary>
/// Helper class for creating a Validation with a specified error type.
/// </summary>
/// <typeparam name="TError">Error type</typeparam>
public static class Validation<TError>
{
    /// <summary>
    /// Creates a new Validation instance with a success value.
    /// </summary>
    /// <typeparam name="TSuccess">Type of the success value</typeparam>
    /// <param name="value">Success value</param>
    /// <returns>A new Validation instance representing a success.</returns>
    public static Validation<TError, TSuccess> Pure<TSuccess>(TSuccess value) =>
        new(Either.Right<ImmutableList<TError>, TSuccess>(value));
}

/// <summary>
/// Helper class for creating a Validation with a specified error type.
/// </summary>
public static class Validation
{
    /// <summary>
    /// Creates a new Validation instance with a success value.
    /// </summary>
    /// <typeparam name="TError">Type of the error</typeparam>
    /// <typeparam name="TSuccess">Type of the success value</typeparam>
    /// <param name="value">Success value</param>
    /// <returns>A new Validation instance representing a success.</returns>
    public static Validation<TError, TSuccess> Pure<TError, TSuccess>(TSuccess value) =>
        new(Either.Right<ImmutableList<TError>, TSuccess>(value));

    /// <summary>
    /// Creates a new Validation instance with an error value.
    /// </summary>
    /// <typeparam name="TError">Type of the error</typeparam>
    /// <typeparam name="TSuccess">Type of the success value</typeparam>
    /// <param name="errorValue">Error value</param>
    /// <returns>A new Validation instance representing an error.</returns>
    public static Validation<TError, TSuccess> Fail<TError, TSuccess>(TError errorValue) =>
        new(Either.Left<ImmutableList<TError>, TSuccess>(ImmutableList.Create(errorValue)));

    /// <summary>
    /// Validates a value using a validator function that returns an Option.
    /// </summary>
    /// <typeparam name="TError">Type of the error</typeparam>
    /// <typeparam name="T">Type of the value to validate</typeparam>
    /// <typeparam name="TSuccess">Type of the success value</typeparam>
    /// <param name="value">Value to validate</param>
    /// <param name="errorValue">Error value to use if validation fails</param>
    /// <param name="validator">Validator function</param>
    /// <returns>A new Validation instance representing the result of the validation.</returns>
    public static Validation<TError, TSuccess> Validate<TError, T, TSuccess>(
        T value,
        TError errorValue,
        Func<T, Option<TSuccess>> validator) =>
        validator(value).Fold(
            some: Pure<TError, TSuccess>,
            none: () => Fail<TError, TSuccess>(errorValue));

    /// <summary>
    /// Validates a value using a validator function that returns an Either.
    /// </summary>
    /// <typeparam name="TError">Type of the error</typeparam>
    /// <typeparam name="T">Type of the value to validate</typeparam>
    /// <typeparam name="TSuccess">Type of the success value</typeparam>
    /// <param name="value">Value to validate</param>
    /// <param name="validator">Validator function</param>
    /// <returns>A new Validation instance representing the result of the validation.</returns>
    public static Validation<TError, TSuccess> Validate<TError, T, TSuccess>(
        T value,
        Func<T, Either<TError, TSuccess>> validator) =>
        new(validator(value).SelectLeft(ImmutableList.Create));
}
