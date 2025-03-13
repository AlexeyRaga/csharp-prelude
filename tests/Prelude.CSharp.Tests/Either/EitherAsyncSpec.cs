using Hedgehog.Xunit;

namespace Prelude.CSharp.Tests.EitherTests;

[Properties(typeof(GeneratorsConfig))]
public sealed class EitherAsyncSpec
{
    private static Task<Either<TLeft, TRight>> RightAsync<TLeft, TRight>(TRight value) =>
        Task.FromResult(Either.Right<TLeft, TRight>(value));

    private static Task<Either<TLeft, TRight>> LeftAsync<TLeft, TRight>(TLeft value) =>
        Task.FromResult(Either.Left<TLeft, TRight>(value));

    [Property]
    public async Task SelectMany_should_return_computed_value(int baseAmount, int multiplier)
    {
        int Compute(int a, int b, int c) => a * b - c;

        var result = await
            from b in RightAsync<string, int>(baseAmount)
            from m in RightAsync<string, int>(multiplier)
            select Compute(b, m, b);

        var expected = Compute(baseAmount, multiplier, baseAmount);

        Assert.Equivalent(Either.Right<string, int>(expected), result);
    }

    [Property]
    public async Task SelectMany_should_return_left_value(int value, string error)
    {
        var result = await
            from v in RightAsync<string, int>(value)
            from e in LeftAsync<string, int>(error)
            select v + e;

        Assert.Equivalent(Either.Left<string, int>(error), result);
    }

    [Property]
    public async Task BiSelect_should_map_either_value(Either<string, string> value)
    {
        var result = await value
            .Select(async x => await RightAsync<string, string>(x + "!").ConfigureAwait(false))
            .SelectManyLeft(async x => await LeftAsync<string, string>(x + "!").ConfigureAwait(false));

        var expected = value.BiSelect(x => x + "!", x => x + "!");
        Assert.Equivalent(expected, result);
    }
}
