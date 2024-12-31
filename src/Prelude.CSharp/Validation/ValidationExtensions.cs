using System.Collections.Immutable;

namespace Prelude;

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

    /// <summary>
    /// Gets the success value or a default value if the validation failed.
    /// </summary>
    public static TSuccess GetOrDefault<TError, TSuccess>(
        this Validation<TError, TSuccess> source,
        TSuccess defaultValue) =>
        source.Either.GetRightOrDefault(defaultValue);

    /// <summary>
    /// Gets the success value or a default value if the validation failed.
    /// </summary>
    public static TSuccess GetOrDefault<TError, TSuccess>(
        this Validation<TError, TSuccess> source,
        Func<TSuccess> defaultValue) =>
        source.Either.GetRightOrDefault(defaultValue);

    /// <summary>
    /// Gets the success value or compensates with a function if the validation failed.
    /// </summary>
    public static TSuccess GetOr<TError, TSuccess>(
        this Validation<TError, TSuccess> source,
        Func<ImmutableList<TError>, TSuccess> compensate) =>
        source.Either.Fold(compensate, x => x);

    /// <summary>
    /// Projects the success value of a validation into a new form.
    /// </summary>
    public static Validation<TError, TSuccess> Select<TError, T, TSuccess>(
        this Validation<TError, T> source,
        Func<T, TSuccess> selector) =>
        new(source.Either.Select(selector));

    /// <summary>
    /// Projects the error value of a validation into a new form.
    /// </summary>
    public static Validation<TError, TSuccess> SelectErrors<TError, T, TSuccess>(
        this Validation<T, TSuccess> source,
        Func<T, TError> selector) => new(source.Either.SelectLeft(x => x.Select(selector).ToImmutableList()));

    /// <summary>
    /// Binds through a <see ref="Validation{TError, TSuccess}" />, which is useful for
    /// composing Validations sequentially.
    /// Note that despite having a Bind function of the correct type,
    /// <see ref="Validation{TError, TSuccess}" /> is not a monad.
    /// The reason is, this Bind does not accumulate errors, so it does not agree with the Applicative behaviour.
    /// <br/>
    /// There is nothing wrong with using this function, it just does not make a valid Monad,
    /// and therefore <see ref="Validation{TError, TSuccess}" /> doesn't have SelectMany.
    /// </summary>
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
    /// with a value that could fail otherwise.
    /// </summary>
    public static Validation<TError, TSuccess> Ensure<TError, T, TSuccess>(
        this Validation<TError, T> source,
        Func<T, Either<TError, TSuccess>> validator) =>
        source.Bind(x => Validation.Validate(x, validator));
}
