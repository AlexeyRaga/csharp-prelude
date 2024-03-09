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
    public static TResult Fold<TLeft, TRight, TResult>(this Either<TLeft, TRight> either, Func<TLeft, TResult> left,
        Func<TRight, TResult> right) =>
        either switch
        {
            Either<TLeft, TRight>.Left l => left(l.Value),
            Either<TLeft, TRight>.Right r => right(r.Value),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Converts the <see cref="Either{_,_}.Right"/> value into an <see cref="Option{TRight}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TRight> ToOption<TLeft, TRight>(this Either<TLeft, TRight> either) =>
        either.Fold(_ => Option.None<TRight>(), Option.Some);

    /// <summary>
    /// Maps the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    public static Either<TLeft, TResult> Select<TLeft, TRight, TResult>(this Either<TLeft, TRight> either,
        Func<TRight, TResult> selector) =>
        either switch
        {
            Either<TLeft, TRight>.Left left => Either.Left<TLeft, TResult>(left.Value),
            Either<TLeft, TRight>.Right right => Either.Right<TLeft, TResult>(selector(right.Value)),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Projects both the <see cref="Either{_,_}.Left"/> and <see cref="Either{_,_}.Right"/> values into their new forms.
    /// </summary>
    public static Either<TLeft1, TRight1> BiSelect<TLeft, TRight, TLeft1, TRight1>(
        this Either<TLeft, TRight> either,
        Func<TLeft, TLeft1> leftSelector,
        Func<TRight, TRight1> rightSelector) =>
        either switch
        {
            Either<TLeft, TRight>.Left left => Either.Left<TLeft1, TRight1>(leftSelector(left.Value)),
            Either<TLeft, TRight>.Right right => Either.Right<TLeft1, TRight1>(rightSelector(right.Value)),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    public static Either<TLeft, TResult> SelectMany<TLeft, TRight, TResult>(this Either<TLeft, TRight> either,
        Func<TRight, Either<TLeft, TResult>> selector) =>
        either switch
        {
            Either<TLeft, TRight>.Left left => Either.Left<TLeft, TResult>(left.Value),
            Either<TLeft, TRight>.Right right => selector(right.Value),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Projects the value of a <see cref="Either{_,_}.Right"/> into a new form.
    /// </summary>
    public static Either<TLeft, TResult> SelectMany<TLeft, TRight, TValue, TResult>(
        this Either<TLeft, TRight> either,
        Func<TRight, Either<TLeft, TValue>> eitherSelector,
        Func<TRight, TValue, TResult> resultSelector) =>
        either switch
        {
            Either<TLeft, TRight>.Left left => Either.Left<TLeft, TResult>(left.Value),
            Either<TLeft, TRight>.Right right =>
                eitherSelector(right.Value).Select(value => resultSelector(right.Value, value)),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Joins nested <see cref="Either{_,_}"/> into a single <see cref="Either{_,_}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Either<TLeft, TRight> Flatten<TLeft, TRight>(this Either<TLeft, Either<TLeft, TRight>> either) =>
        either.SelectMany(x => x);

    /// <summary>
    /// Maps the value of a <see cref="Either{_,_}.Left"/> into a new form.
    /// </summary>
    public static Either<TResult, TRight> SelectLeft<TResult, TLeft, TRight>(this Either<TLeft, TRight> either,
        Func<TLeft, TResult> selector) =>
        either switch
        {
            Either<TLeft, TRight>.Left left => Either.Left<TResult, TRight>(selector(left.Value)),
            Either<TLeft, TRight>.Right right => Either.Right<TResult, TRight>(right.Value),
            _ => throw new InvalidOperationException("Invalid state")
        };

    /// <summary>
    /// Returns the value from the <see cref="Either{_,_}.Right"/> case or a default value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TRight FromRight<TLeft, TRight>(this Either<TLeft, TRight> either, TRight defaultValue) =>
        either.Fold(_ => defaultValue, x => x);


    /// <summary>
    /// Returns the value from the <see cref="Either{_,_}.Left"/> case or compensates for the left value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TRight FromRight<TLeft, TRight>(this Either<TLeft, TRight> either, Func<TLeft, TRight> compensate) =>
        either.Fold(compensate, x => x);

    /// <summary>
    /// Returns the value from the <see cref="Either{_,_}.Left"/> case or a default value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TLeft FromLeft<TLeft, TRight>(this Either<TLeft, TRight> either, TLeft defaultValue) =>
        either.Fold(x => x, _ => defaultValue);

    /// <summary>
    /// Returns the value from the <see cref="Either{_,_}.Right"/> case or compensates for the right value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TLeft FromLeft<TLeft, TRight>(this Either<TLeft, TRight> either, Func<TRight, TLeft> compensate) =>
        either.Fold(x => x, compensate);

    /// <summary>
    /// Swaps the <see cref="Either{_,_}.Left"/> and <see cref="Either{_,_}.Right"/> cases.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Either<TRight, TLeft> Swap<TLeft, TRight>(this Either<TLeft, TRight> either) =>
        either.Fold(Either.Right<TRight, TLeft>, Either.Left<TRight, TLeft>);

    private static void Do<TLeft, TRight>(this Either<TLeft, TRight> either, Action<TLeft> left, Action<TRight> right)
    {
        switch (either)
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
    /// Similarly the  <see cref="Either{_,_}.Right"/> elements are extracted to the second component of the output.
    /// </summary>
    public static (IEnumerable<TLeft>, IEnumerable<TRight>) PartitionEithers<TLeft, TRight>(
        this IEnumerable<Either<TLeft, TRight>> values)
    {
        var lefts = new List<TLeft>();
        var rights = new List<TRight>();

        foreach (var value in values)
        {
            value.Do(lefts.Add, rights.Add);
        }

        return (lefts, rights);
    }

    /// <summary>
    /// Extracts from a sequence of <see cref="Either{_,_}"/> all the <see cref="Either{_,_}.Left"/> elements.
    /// All the <see cref="Either{_,_}.Left"/> elements are extracted in order.
    /// </summary>
    public static IEnumerable<TLeft> CollectLefts<TLeft, TRight>(this IEnumerable<Either<TLeft, TRight>> values)
    {
        foreach (var either in values)
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
    public static IEnumerable<TRight> CollectRights<TLeft, TRight>(this IEnumerable<Either<TLeft, TRight>> values)
    {
        foreach (var either in values)
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TRight ThrowLeft<TLeft, TRight>(this Either<TLeft, TRight> either) where TLeft : Exception =>
        either.Fold(x => throw x, x => x);

    /// <summary>
    /// Throws the <see cref="Either{_,_}.Right"/> value if it exists.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TLeft ThrowRight<TLeft, TRight>(this Either<TLeft, TRight> either) where TRight : Exception =>
        either.Fold(x => x, x => throw x);

    /// <summary>
    /// Traverses the function <paramref name="transform"/> over the <paramref name="source"/> sequence
    /// and returns a result when all the elements of the sequence are transformed to <see cref="Either{_,_}.Right"/>.
    /// Otherwise returns the first <see cref="Either{_,_}.Left"/> value.
    /// </summary>
    public static Either<TLeft, IEnumerable<TRight>> Traverse<TValue, TLeft, TRight>(
        this IEnumerable<TValue> source,
        Func<TValue, Either<TLeft, TRight>> transform)
    {
        var result = new List<TRight>();
        foreach (var value in source)
        {
            switch (value)
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
    /// Returns an sequence of elements if each element in the <paramref name="source"/>
    /// sequence is <see cref="Either{_,_}.Right"/>.
    /// Otherwise returns the first <see cref="Either{_,_}.Left"/> value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Either<TLeft, IEnumerable<TRight>> Sequence<TLeft, TRight>(
        this IEnumerable<Either<TLeft, TRight>> source) =>
        source.Traverse(x => x);
}
