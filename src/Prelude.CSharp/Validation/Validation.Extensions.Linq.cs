using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Prelude;

public static class ValidationLinqExtensions
{
    public static Validation<TError, TResult> Select<TError, TSuccess, TResult>(
        this Validation<TError, TSuccess> validation,
        Func<TSuccess, TResult> selector) => new(validation.Either.Select(selector));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Validation<TLeft, TRight> Flatten<TLeft, TRight>(
        this Validation<TLeft, Validation<TLeft, TRight>> source) => source.Bind(x => x);

    public static Validation<TError, TSuccess> FlattenErrors<TError, TSuccess>(
        this Validation<IEnumerable<TError>, TSuccess> source) =>
            new(source.Either.SelectLeft(x => x.SelectMany(y => y).ToImmutableList()));

    public static Validation<TResult, TRight> SelectLeft<TResult, TLeft, TRight>(
        this Validation<TLeft, TRight> source,
        Func<TLeft, TResult> selector) => new(source.Either.SelectLeft(x => x.Select(selector).ToImmutableList()));
}
