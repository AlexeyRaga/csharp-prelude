using FsCheck.Xunit;
using Prelude.Experimental;

namespace Prelude.CSharp.Tests.Experimental;

public sealed class OptionAsyncSpec
{
    private static Task<Option<T>> SomeAsync<T>(T value) => Task.FromResult(Option.Some(value));

    [Property]
    public async Task ShouldMapAsync(int baseAmount, int multiplier)
    {
        int Compute(int a, int b, int c) => a * b - c;

        var result = await
            from b in SomeAsync(baseAmount)
            from m in SomeAsync(multiplier)
            select Compute(b, m, b);

        var expected = Compute(baseAmount, multiplier, baseAmount);

        Assert.Equivalent(Option.Some(expected), result);
    }

}
