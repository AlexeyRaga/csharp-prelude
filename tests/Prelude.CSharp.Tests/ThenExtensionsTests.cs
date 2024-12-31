using FsCheck.Xunit;

namespace Prelude.Tests;

public sealed class ThenExtensionsTests
{
    [Property]
    public void Then_PipelineWithMultipleParameters(int x, int y)
    {
        int IncreaseBy(int a, int b) => a + b;
        int MultiplyBy(int a, int b, int c) => a * b * c;

        var result = x
            .Then(IncreaseBy, y)
            .Then(MultiplyBy, x, y);

        var expected = MultiplyBy(IncreaseBy(x, y), x, y);

        Assert.Equal(expected, result);
    }
}
