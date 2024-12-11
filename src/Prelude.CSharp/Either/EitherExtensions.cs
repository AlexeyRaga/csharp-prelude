using System.Runtime.CompilerServices;

namespace Prelude;

/// <summary>
/// Provides a set of extension methods for <see cref="Either{_,_}"/> type.
/// </summary>
public static class EitherExtensions
{
    /// <summary>
    /// Folds the value of the <see cref="Either{_,_}"/> into a single value by converting
    /// the value on either <see cref="Either{_,_}.Left"/> or <see cref="Either{_,_}.Right"/> side
    /// into a value of type <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The either instance to fold.</param>
    /// <param name="left">The function to apply if the either contains a left value.</param>
    /// <param name="right">The function to apply if the either contains a right value.</param>
    /// <returns>The result of applying the appropriate function based on the either's state.</returns>
    public static TResult Fold<TLeft, TRight, TResult>(this Either<TLeft, TRight> source, Func<TLeft, TResult> left,
        Func<TRight, TResult> right) =>
        source switch
        {
            Either<TLeft, TRight>.Left l => left(l.Value),
            Either<TLeft, TRight>.Right r => right(r.Value),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Returns the value contained in the <see cref="Either{_,_}"/> if it is in the "Right" state;
    /// otherwise, returns the specified default value.
    /// </summary>
    /// <param name="source">The either instance to extract the value from.</param>
    /// <param name="defaultValue">The value to return if the value is in the "Left" state.</param>
    /// <returns>The contained value if "Right"; otherwise, the specified default value.</returns>
    public static TRight GetRightOrDefault<TLeft, TRight>(this Either<TLeft, TRight> source, TRight defaultValue) =>
        source.Fold(_ => defaultValue, x => x);

    /// <summary>
    /// Returns the value contained in the <see cref="Either{_,_}"/> if it is in the "Right" state;
    /// otherwise, returns the specified default value.
    /// </summary>
    /// <param name="source">The either instance to extract the value from.</param>
    /// <param name="defaultValue">The value to return if the value is in the "Left" state.</param>
    /// <returns>The contained value if "Right"; otherwise, the specified default value.</returns>
    public static TRight GetRightOrDefault<TLeft, TRight>(this Either<TLeft, TRight> source, Func<TRight> defaultValue) =>
        source.Fold(_ => defaultValue(), x => x);

    /// <summary>
    /// Returns the value contained in the <see cref="Either{_,_}"/> if it is in the "Right" state;
    /// otherwise, returns the specified default value.
    /// </summary>
    /// <param name="source">The either instance to extract the value from.</param>
    /// <param name="defaultValue">The value to return if the value is in the "Right" state.</param>
    /// <returns>The contained value if "Left"; otherwise, the specified default value.</returns>
    public static TLeft GetLeftOrDefault<TLeft, TRight>(this Either<TLeft, TRight> source, TLeft defaultValue) =>
        source.Fold(x => x, _ => defaultValue);

    /// <summary>
    /// Returns the value contained in the <see cref="Either{_,_}"/> if it is in the "Left" state;
    /// otherwise, returns the specified default value.
    /// </summary>
    /// <param name="source">The either instance to extract the value from.</param>
    /// <param name="defaultValue">The value to return if the value is in the "Right" state.</param>
    /// <returns>The contained value if "Left"; otherwise, the specified default value.</returns>
    public static TLeft GetLeftOrDefault<TLeft, TRight>(this Either<TLeft, TRight> source, Func<TLeft> defaultValue) =>
        source.Fold(x => x, _ => defaultValue());

    /// <summary>
    /// Converts the <see cref="Either{_,_}.Right"/> value into an <see cref="Option{TRight}"/>.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="source">The either instance to convert to an option.</param>
    /// <returns>An option containing the right value, or None if the either contains a left value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TRight> ToOption<TLeft, TRight>(this Either<TLeft, TRight> source) =>
        source.Fold(_ => Option.None<TRight>(), Option.Some);

    /// <summary>
    /// Maps the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The either instance to transform.</param>
    /// <param name="selector">The function that transforms the right value.</param>
    /// <returns>A new either with the transformed right value or the original left value.</returns>
    public static Either<TLeft, TResult> Select<TLeft, TRight, TResult>(this Either<TLeft, TRight> source,
        Func<TRight, TResult> selector) =>
        source switch
        {
            Either<TLeft, TRight>.Left left => Either.Left<TLeft, TResult>(left.Value),
            Either<TLeft, TRight>.Right right => Either.Right<TLeft, TResult>(selector(right.Value)),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Projects both the <see cref="Either{_,_}.Left"/> and <see cref="Either{_,_}.Right"/> values into their new forms.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TLeft1">The type of the new left value.</typeparam>
    /// <typeparam name="TRight1">The type of the new right value.</typeparam>
    /// <param name="source">The either instance to transform.</param>
    /// <param name="leftSelector">The function that transforms the left value.</param>
    /// <param name="rightSelector">The function that transforms the right value.</param>
    /// <returns>A new either with the transformed left and right values.</returns>
    public static Either<TLeft1, TRight1> BiSelect<TLeft, TRight, TLeft1, TRight1>(
        this Either<TLeft, TRight> source,
        Func<TLeft, TLeft1> leftSelector,
        Func<TRight, TRight1> rightSelector) =>
        source switch
        {
            Either<TLeft, TRight>.Left left => Either.Left<TLeft1, TRight1>(leftSelector(left.Value)),
            Either<TLeft, TRight>.Right right => Either.Right<TLeft1, TRight1>(rightSelector(right.Value)),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The either instance to project into a new form.</param>
    /// <param name="selector">The function that returns a new either.</param>
    /// <returns>A new either resulting from the projection or the original left value.</returns>
    public static Either<TLeft, TResult> SelectMany<TLeft, TRight, TResult>(this Either<TLeft, TRight> source,
        Func<TRight, Either<TLeft, TResult>> selector) =>
        source switch
        {
            Either<TLeft, TRight>.Left left => Either.Left<TLeft, TResult>(left.Value),
            Either<TLeft, TRight>.Right right => selector(right.Value),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The either instance to transform.</param>
    /// <param name="selector">The function that transforms the right value.</param>
    /// <returns>A new either with the transformed right value or the original left value.</returns>
    public static Either<TLeft, TResult> Bind<TLeft, TRight, TResult>(this Either<TLeft, TRight> source,
        Func<TRight, Either<TLeft, TResult>> selector) =>
        source switch
        {
            Either<TLeft, TRight>.Left left => Either.Left<TLeft, TResult>(left.Value),
            Either<TLeft, TRight>.Right right => selector(right.Value),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <typeparam name="TValue">The type of the intermediate result.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="source">The either instance to project into a new form.</param>
    /// <param name="eitherSelector">The function that returns a new either.</param>
    /// <param name="resultSelector">The function that combines the right value and the result of the projection.</param>
    /// <returns>A new either containing the final result or the original left value.</returns>
    public static Either<TLeft, TResult> SelectMany<TLeft, TRight, TValue, TResult>(
        this Either<TLeft, TRight> source,
        Func<TRight, Either<TLeft, TValue>> eitherSelector,
        Func<TRight, TValue, TResult> resultSelector) =>
        source switch
        {
            Either<TLeft, TRight>.Left left => Either.Left<TLeft, TResult>(left.Value),
            Either<TLeft, TRight>.Right right =>
                eitherSelector(right.Value).Select(value => resultSelector(right.Value, value)),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Joins nested <see cref="Either{_,_}"/> into a single <see cref="Either{_,_}"/>.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="source">The nested either to flatten.</param>
    /// <returns>A flattened either with the right value or the original left value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Either<TLeft, TRight> Flatten<TLeft, TRight>(this Either<TLeft, Either<TLeft, TRight>> source) =>
        source.SelectMany(x => x);

    /// <summary>
    /// Maps the value of a <see cref="Either{_,_}.Left"/> into a new form.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TResult">The type of the transformed left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="source">The either instance to transform.</param>
    /// <param name="selector">The function that transforms the left value.</param>
    /// <returns>A new either with the transformed left value or the original right value.</returns>
    public static Either<TResult, TRight> SelectLeft<TResult, TLeft, TRight>(this Either<TLeft, TRight> source,
        Func<TLeft, TResult> selector) =>
        source switch
        {
            Either<TLeft, TRight>.Left left => Either.Left<TResult, TRight>(selector(left.Value)),
            Either<TLeft, TRight>.Right right => Either.Right<TResult, TRight>(right.Value),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Returns the value from the <see cref="Either{_,_}.Right"/> case or a default value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="source">The either instance to extract the right value from.</param>
    /// <param name="defaultValue">The default value to return if the either contains a left value.</param>
    /// <returns>The right value or the default value if the either contains a left value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TRight FromRight<TLeft, TRight>(this Either<TLeft, TRight> source, TRight defaultValue) =>
        source.Fold(_ => defaultValue, x => x);


    /// <summary>
    /// Returns the value from the <see cref="Either{_,_}.Left"/> case or compensates for the left value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="source">The either instance to extract the right value from.</param>
    /// <param name="compensate">The function that returns a value to compensate for the left value.</param>
    /// <returns>The right value or the result of the compensate function if the either contains a left value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TRight FromRight<TLeft, TRight>(this Either<TLeft, TRight> source, Func<TLeft, TRight> compensate) =>
        source.Fold(compensate, x => x);

    /// <summary>
    /// Converts <see cref="Option{_}"/> to <see cref="Either{_, _}"/>.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="source">The option to convert to an either.</param>
    /// <param name="defaultLeft">The value to use for the left case if the option is None.</param>
    /// <returns>An either with the option's value as the right value or the provided left value if the option is None.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Either<TLeft, TRight> ToEither<TLeft, TRight>(this Option<TRight> source, TLeft defaultLeft) =>
        source.Fold(Either.Right<TLeft, TRight>, () => Either.Left<TLeft, TRight>(defaultLeft));

    /// <summary>
    /// Returns the value from the <see cref="Either{_,_}.Left"/> case or a default value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="source">The either instance to extract the left value from.</param>
    /// <param name="defaultValue">The default value to return if the either contains a right value.</param>
    /// <returns>The left value or the default value if the either contains a right value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TLeft FromLeft<TLeft, TRight>(this Either<TLeft, TRight> source, TLeft defaultValue) =>
        source.Fold(x => x, _ => defaultValue);

    /// <summary>
    /// Returns the value from the <see cref="Either{_,_}.Right"/> case or compensates for the right value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="source">The either instance to extract the left value from.</param>
    /// <param name="compensate">The function that returns a value to compensate for the right value.</param>
    /// <returns>The left value or the result of the compensate function if the either contains a right value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TLeft FromLeft<TLeft, TRight>(this Either<TLeft, TRight> source, Func<TRight, TLeft> compensate) =>
        source.Fold(x => x, compensate);

    /// <summary>
    /// Swaps the <see cref="Either{_,_}.Left"/> and <see cref="Either{_,_}.Right"/> cases.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="either">The either instance to swap.</param>
    /// <returns>A new either with the left and right cases swapped.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Either<TRight, TLeft> Swap<TLeft, TRight>(this Either<TLeft, TRight> either) =>
        either.Fold(Either.Right<TRight, TLeft>, Either.Left<TRight, TLeft>);

    private static void Do<TLeft, TRight>(this Either<TLeft, TRight> source, Action<TLeft> left, Action<TRight> right)
    {
        switch (source)
        {
            case Either<TLeft, TRight>.Left l:
                left(l.Value);
                break;
            case Either<TLeft, TRight>.Right r:
                right(r.Value);
                break;
            default:
                throw new InvalidOperationException("Invalid state");
        }
    }

    /// <summary>
    /// Partitions a sequence of <see cref="Either{_,_}"/> into two sequences.
    /// All the <see cref="Either{_,_}.Left"/> elements are extracted, in order, to the first component of the output.
    /// Similarly, the  <see cref="Either{_,_}.Right"/> elements are extracted to the second component of the output.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left values.</typeparam>
    /// <typeparam name="TRight">The type of the right values.</typeparam>
    /// <param name="source">The sequence of eithers to partition.</param>
    /// <returns>A tuple containing two sequences: one of all the left values and one of all the right values.</returns>
    public static (IEnumerable<TLeft>, IEnumerable<TRight>) PartitionEithers<TLeft, TRight>(
        this IEnumerable<Either<TLeft, TRight>> source)
    {
        var lefts = new List<TLeft>();
        var rights = new List<TRight>();

        foreach (var value in source)
        {
            value.Do(lefts.Add, rights.Add);
        }

        return (lefts, rights);
    }

    /// <summary>
    /// Extracts from a sequence of <see cref="Either{_,_}"/> all the <see cref="Either{_,_}.Left"/> elements.
    /// All the <see cref="Either{_,_}.Left"/> elements are extracted in order.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left values.</typeparam>
    /// <typeparam name="TRight">The type of the right values.</typeparam>
    /// <param name="source">The sequence of eithers to extract left values from.</param>
    /// <returns>A sequence of all the left values.</returns>
    public static IEnumerable<TLeft> CollectLefts<TLeft, TRight>(this IEnumerable<Either<TLeft, TRight>> source)
    {
        foreach (var either in source)
        {
            switch (either)
            {
                case Either<TLeft, TRight>.Left left:
                    yield return left.Value;
                    break;
            }
        }
    }

    /// <summary>
    /// Extracts from a sequence of <see cref="Either{_,_}"/> all the <see cref="Either{_,_}.Right"/> elements.
    /// All the <see cref="Either{_,_}.Right"/> elements are extracted in order.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left values.</typeparam>
    /// <typeparam name="TRight">The type of the right values.</typeparam>
    /// <param name="source">The sequence of eithers to extract right values from.</param>
    /// <returns>A sequence of all the right values.</returns>
    public static IEnumerable<TRight> CollectRights<TLeft, TRight>(this IEnumerable<Either<TLeft, TRight>> source)
    {
        foreach (var either in source)
        {
            switch (either)
            {
                case Either<TLeft, TRight>.Right right:
                    yield return right.Value;
                    break;
            }
        }
    }

    /// <summary>
    /// Throws the <see cref="Either{_,_}.Left"/> value if it exists.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value, which must be an exception.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="source">The either instance to throw if it contains a left value.</param>
    /// <returns>The right value if the either contains a right value, or throws the left value if it contains a left value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TRight ThrowLeft<TLeft, TRight>(this Either<TLeft, TRight> source) where TLeft : Exception =>
        source.Fold(x => throw x, x => x);

    /// <summary>
    /// Throws the <see cref="Either{_,_}.Right"/> value if it exists.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value, which must be an exception.</typeparam>
    /// <param name="source">The either instance to throw if it contains a right value.</param>
    /// <returns>The left value if the either contains a left value, or throws the right value if it contains a right value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TLeft ThrowRight<TLeft, TRight>(this Either<TLeft, TRight> source) where TRight : Exception =>
        source.Fold(x => x, x => throw x);

    /// <summary>
    /// Traverses the function <paramref name="transform"/> over the <paramref name="source"/> sequence
    /// and returns a result when all the elements of the sequence are transformed to <see cref="Either{_,_}.Right"/>.
    /// Otherwise, returns the first <see cref="Either{_,_}.Left"/> value.
    /// </summary>
    /// <typeparam name="TValue">The type of the values in the source sequence.</typeparam>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="source">The sequence of values to transform.</param>
    /// <param name="transform">The function that transforms each value into an either.</param>
    /// <returns>An either containing a sequence of right values if all transformations succeed, or the first left value encountered.</returns>
    public static Either<TLeft, IEnumerable<TRight>> Traverse<TValue, TLeft, TRight>(
        this IEnumerable<TValue> source,
        Func<TValue, Either<TLeft, TRight>> transform)
    {
        var result = new List<TRight>();
        foreach (var value in source)
        {
            switch (transform(value))
            {
                case Either<TLeft, TRight>.Left left:
                    return Either.Left<TLeft, IEnumerable<TRight>>(left.Value);
                case Either<TLeft, TRight>.Right right:
                    result.Add(right.Value);
                    break;
                default:
                    throw new InvalidOperationException("Invalid state");
            }
        }

        return Either.Right<TLeft, IEnumerable<TRight>>(result.AsEnumerable());
    }

    /// <summary>
    /// Returns a sequence of elements if each element in the <paramref name="source"/>
    /// sequence is <see cref="Either{_,_}.Right"/>.
    /// Otherwise, returns the first <see cref="Either{_,_}.Left"/> value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="source">The sequence of eithers to process.</param>
    /// <returns>An either containing a sequence of right values if all eithers contain right values, or the first left value encountered.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Either<TLeft, IEnumerable<TRight>> Sequence<TLeft, TRight>(
        this IEnumerable<Either<TLeft, TRight>> source) =>
        source.Traverse(x => x);

    /// <summary>
    /// Filters the Right value based on a predicate.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="source">The either instance to filter.</param>
    /// <param name="predicate">The condition to check for the right value.</param>
    /// <param name="leftValue">The value to return if the predicate fails.</param>
    /// <returns>The original value if the <paramref name="predicate"/> matches the Right value
    /// inside <see cref="Either{_,_}"/>, otherwise <see cref="Either{_,_}.Left"/> with the specified default value
    /// </returns>
    public static Either<TLeft, TRight> Satisfy<TLeft, TRight>(
        this Either<TLeft, TRight> source,
        Func<TRight, bool> predicate,
        TLeft leftValue) =>
        source switch
        {
            Either<TLeft, TRight>.Right right when predicate(right.Value) => source,
            _ => Either.Left<TLeft, TRight>(leftValue)
        };

    /// <summary>
    /// Combines two <see cref="Either{TLeft, TRight}"/> instances by applying the specified combine function
    /// to their <see cref="Either{TLeft, TRight}.Right"/> values if both are in the "Right" state.
    /// </summary>
    /// <typeparam name="TLeft">The type of the "Left" value, representing an error or alternative state.</typeparam>
    /// <typeparam name="TRight">The type of the "Right" value, representing a successful result.</typeparam>
    /// <param name="first">
    /// The first <see cref="Either{TLeft, TRight}"/> to combine.
    /// </param>
    /// <param name="second">
    /// The second <see cref="Either{TLeft, TRight}"/> to combine.
    /// </param>
    /// <param name="combine">
    /// A function that specifies how to combine the <see cref="Either{TLeft, TRight}.Right"/> values
    /// of <paramref name="first"/> and <paramref name="second"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="Either{TLeft, TRight}"/>:
    /// <list type="bullet">
    /// <item><description>
    /// If both <paramref name="first"/> and <paramref name="second"/> are in the "Right" state, returns a "Right"
    /// containing the result of the <paramref name="combine"/> function.
    /// </description></item>
    /// <item><description>
    /// If either <paramref name="first"/> or <paramref name="second"/> is in the "Left" state, returns the first "Left" encountered.
    /// </description></item>
    /// </list>
    /// </returns>
    /// <remarks>
    /// This method does not handle accumulation of "Left" values by default. If you need to accumulate errors,
    /// ensure <typeparamref name="TLeft"/> supports such operations (e.g., a list of errors).
    /// </remarks>
    public static Either<TLeft, TRight> Combine<TLeft, TRight>(
        this Either<TLeft, TRight> first,
        Either<TLeft, TRight> second,
        Func<TRight, TRight, TRight> combine) => Either.Combine(first, second, combine);
}
