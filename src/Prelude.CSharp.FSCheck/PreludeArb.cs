using FsCheck;
using FsCheck.Fluent;

namespace Prelude.CSharp.FSCheck;

/// <summary>
/// Provides FSCheck arbitrary instances for the Prelude types
/// </summary>
public static class PreludeArb
{
    /// <summary>
    /// Provides an arbitrary instance for the <see cref="Either{_, _}"/> type
    /// </summary>
    public static Arbitrary<Either<TLeft, TRight>> EitherArb<TLeft, TRight>() =>
        Gen.OneOf(
            ArbMap.Default.GeneratorFor<TLeft>().Select(Either.Left<TLeft, TRight>),
            ArbMap.Default.GeneratorFor<TRight>().Select(Either.Right<TLeft, TRight>)).ToArbitrary();

    /// <summary>
    /// Provides an arbitrary instance for the <see cref="Option{_}"/> type
    /// </summary>
    public static Arbitrary<Option<T>> OptionArb<T>() =>
        Gen.OneOf(
            ArbMap.Default.GeneratorFor<T>().Select(Option.Some),
            Gen.Constant(Option.None<T>())).ToArbitrary();

    /// <summary>
    /// Provides an arbitrary instance for the <see cref="NonEmptyList{T}"/> type
    /// </summary>
    public static Arbitrary<NonEmptyList<T>> NonEmptyListArb<T>() =>
        ArbMap.Default
            .GeneratorFor<T>()
            .NonEmptyListOf()
            .Select(xs => new NonEmptyList<T>(xs[0], xs.Skip(1)))
            .ToArbitrary();
}
