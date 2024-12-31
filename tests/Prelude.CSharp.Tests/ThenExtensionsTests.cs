using FsCheck.Xunit;

namespace Prelude.Tests;

public sealed class ThenExtensionsTests
{
    [Property]
    public void Then_PipelineWithMultipleParameters(int x, int y, int z)
    {
        int AddThreeNumbers(int a, int b, int c) => a + b + c;
        int MultiplyFourNumbers(int a, int b, int c, int d) => a * b * c * d;

        var result = x
            .Then(AddThreeNumbers, y, z)
            .Then(MultiplyFourNumbers, x, y, z);

        var expected = MultiplyFourNumbers(AddThreeNumbers(x, y, z), x, y, z);

        Assert.Equal(expected, result);
    }
}
