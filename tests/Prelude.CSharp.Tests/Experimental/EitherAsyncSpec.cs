using Prelude.Experimental;
using FsCheck.Xunit;

namespace Prelude.CSharp.Tests.Experimental;

public sealed class EitherAsyncSpec
{
    private static Task<Either<TLeft, TRight>> RightAsync<TLeft, TRight>(TRight value) =>
        Task.FromResult(Either.Right<TLeft, TRight>(value));

    private static Task<Either<TLeft, TRight>> LeftAsync<TLeft, TRight>(TLeft value) =>
        Task.FromResult(Either.Left<TLeft, TRight>(value));

    [Property]
    public async Task ShouldMapAsync(int baseAmount, int multiplier)
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
    public async Task ShouldExitWitLeft(int value, string error)
    {
        var result = await
            from v in RightAsync<string, int>(value)
            from e in LeftAsync<string, int>(error)
            select v + e;

        Assert.Equivalent(Either.Left<string, int>(error), result);
    }
}
