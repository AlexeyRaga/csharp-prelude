using System.Collections.Immutable;

namespace Prelude;

/// <summary>
/// The <see ref="Validation{TError, TSuccess}" /> is like an <see ref="Either{TLeft, TRight}" />
/// with an ability to accumulate
/// </summary>
/// <param name="Either"></param>
/// <typeparam name="TError"></typeparam>
/// <typeparam name="TSuccess"></typeparam>
public record Validation<TError, TSuccess>(Either<ImmutableList<TError>, TSuccess> Either)
{
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
    /// <param name="value">Success value</param>
    /// <typeparam name="TSuccess">Type of the success value</typeparam>
    /// <returns></returns>
    public static Validation<TError, TSuccess> Pure<TSuccess>(TSuccess value) =>
        new(Either.Right<ImmutableList<TError>, TSuccess>(value));
}

public static class Validation
{
    public static Validation<TError, TSuccess> Pure<TError, TSuccess>(TSuccess value) =>
        new(Either.Right<ImmutableList<TError>, TSuccess>(value));

    public static Validation<TError, TSuccess> Fail<TError, TSuccess>(TError errorValue) =>
        new(Either.Left<ImmutableList<TError>, TSuccess>(ImmutableList.Create(errorValue)));

    public static Validation<TError, TSuccess> Validate<TError, T, TSuccess>(
        T value,
        TError errorValue,
        Func<T, Option<TSuccess>> validator) =>
        validator(value).Fold(
            some: Pure<TError, TSuccess>,
            none: () => Fail<TError, TSuccess>(errorValue));

    public static Validation<TError, TSuccess> Validate<TError, T, TSuccess>(
        T value,
        Func<T, Either<TError, TSuccess>> validator) =>
        new(validator(value).SelectLeft(ImmutableList.Create));
}

/// <summary>
/// Extension methods for the Validation type.
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    /// Converts an <see ref="Either{TError, TSuccess}" /> to a <see ref="Validation{TError, TSuccess}" />.
    /// </summary>
    public static Validation<TError, TSuccess> ToValidation<TError, TSuccess>(this Either<TError, TSuccess> source) =>
        new(source.SelectLeft(ImmutableList.Create));

    public static TSuccess GetOrDefault<TError, TSuccess>(
        this Validation<TError, TSuccess> source,
        TSuccess defaultValue) =>
        source.Either.GetRightOrDefault(defaultValue);

    public static TSuccess GetOrDefault<TError, TSuccess>(
        this Validation<TError, TSuccess> source,
        Func<TSuccess> defaultValue) =>
        source.Either.GetRightOrDefault(defaultValue);

    public static TSuccess GetOr<TError, TSuccess>(
        this Validation<TError, TSuccess> source,
        Func<ImmutableList<TError>, TSuccess> compensate) =>
        source.Either.Fold(compensate, x => x);

    public static Validation<TError, TSuccess> Select<TError, T, TSuccess>(
        this Validation<TError, T> source,
        Func<T, TSuccess> selector) =>
        new(source.Either.Select(selector));

    public static Validation<TError, TSuccess> SelectErrors<TError, T, TSuccess>(
        this Validation<T, TSuccess> source,
        Func<T, TError> selector) => new(source.Either.SelectLeft(x => x.Select(selector).ToImmutableList()));

    /// <summary>
    /// binds through a <see ref="Validation{TError, TSuccess}" />, which is useful for
    /// composing Validations sequentially.
    /// Note that despite having a Bind function of the correct type,
    /// <see ref="Validation{TError, TSuccess}" /> is not a monad.
    /// The reason is, this Bind does not accumulate errors, so it does not agree with the Applicative behaviour.
    /// <br/>
    /// There is nothing wrong with using this function, it just does not make a valid Monad,
    /// and therefore <see ref="Validation{TError, TSuccess}" /> doesn't have SelectMany.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="binder"></param>
    /// <typeparam name="TError"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TSuccess"></typeparam>
    /// <returns></returns>
    public static Validation<TError, TSuccess> Bind<TError, T, TSuccess>(
        this Validation<TError, T> source,
        Func<T, Validation<TError, TSuccess>> binder) =>
        new(source.Either.Bind(x => binder(x).Either));

    /// <summary>
    /// Ensures that a validation remains unchanged upon failure, updating a successful validation
    /// with an optional value that could fail otherwise.
    /// </summary>
    public static Validation<TError, TSuccess> Ensure<TError, T, TSuccess>(
        this Validation<TError, T> source,
        TError defaultError,
        Func<T, Option<TSuccess>> validator) =>
        source.Bind(x => Validation.Validate(x, defaultError, validator));

    /// <summary>
    /// Ensures that a validation remains unchanged upon failure, updating a successful validation
    /// with an optional value that could fail otherwise.
    /// </summary>
    public static Validation<TError, TSuccess> Ensure<TError, T, TSuccess>(
        this Validation<TError, T> source,
        Func<T, Either<TError, TSuccess>> validator) =>
        source.Bind(x => Validation.Validate(x, validator));
}
