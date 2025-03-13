using Hedgehog;
using Hedgehog.Linq;
using Gen = Hedgehog.Linq.Gen;
using Range = Hedgehog.Linq.Range;

namespace Prelude.Hedgehog;

/// <summary>
/// Generators for Prelude types
/// </summary>
public sealed class PreludeGen
{
    /// <summary>
    /// Generates an <see cref="Prelude.Option{T}"/> with a 20% chance of being <see cref="Prelude.Option.None{T}"/>
    /// </summary>
    /// <param name="valueGen">The generator for the value type </param>
    public static Gen<Option<T>> Option<T>(Gen<T> valueGen) =>
        Gen.Frequency([
            Tuple.Create(2, Gen.FromValue(Prelude.Option.None<T>())),
            Tuple.Create(8, valueGen.Select(Prelude.Option.Some)),
        ]);

    /// <summary>
    /// Generates an <see cref="Prelude.Either{TLeft, TRight}"/> with a 50% chance of being <see cref="Prelude.Either.Left{TLeft, TRight}"/>
    /// </summary>
    /// <param name="leftGen">The generator for the left type </param>
    /// <param name="rightGen">The generator for the right type </param>
    public static Gen<Either<TLeft, TRight>> Either<TLeft, TRight>(Gen<TLeft> leftGen, Gen<TRight> rightGen) =>
        Gen.Choice([
            leftGen.Select(Prelude.Either.Left<TLeft, TRight>),
            rightGen.Select(Prelude.Either.Right<TLeft, TRight>)]);

    /// <summary>
    /// Generates a <see cref="Prelude.NonEmptyList{T}"/> with a length specified by the range
    /// </summary>
    /// <param name="valueGen">The generator for the value type</param>
    /// <param name="range">The range of the list length</param>
    internal static Gen<NonEmptyList<T>> NonEmptyList<T>(Gen<T> valueGen, Range<int> range) =>
        valueGen
            .List(range.Select(x => Math.Max(x, 1)))
            .Select(x => Prelude.NonEmptyList.Create(x[0], x.Skip(1)));

    /// <summary>
    /// Generates a <see cref="Prelude.NonEmptyList{T}"/> with a length between 1 and 50
    /// </summary>
    /// <param name="valueGen">The generator for the value type</param>
    public static Gen<NonEmptyList<T>> NonEmptyList<T>(Gen<T> valueGen) =>
        NonEmptyList(valueGen, Range.LinearInt32(1, 50));
}

/// <summary>
/// Extension methods providing additional generators for Prelude types
/// </summary>
public static class LinqGenExtensions
{
    /// <summary>
    /// Generates an <see cref="Prelude.Option{T}"/> with a 20% chance of being <see cref="Prelude.Option.None{T}"/>
    /// </summary>
    /// <param name="source">The generator for the value type </param>
    public static Gen<Option<T>> Option<T>(this Gen<T> source) =>
        PreludeGen.Option(source);

    /// <summary>
    /// Generates a <see cref="Prelude.NonEmptyList{T}"/> with a length specified by the range
    /// </summary>
    /// <param name="source">The generator for the value type</param>
    /// <param name="range">The range of the list length</param>
    public static Gen<NonEmptyList<T>> NonEmptyList<T>(this Gen<T> source, Range<int> range) =>
        PreludeGen.NonEmptyList(source, range);
}
