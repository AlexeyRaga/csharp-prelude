using FsCheck.Xunit;

namespace Prelude.CSharp.Tests;

public sealed class TaskExtensionsTests
{
    [Property]
    public async Task ShouldMapAndBind(int a, int b)
    {
        var result = await
            from x in Task.FromResult(a).ConfigureAwait(false)
            from y in Task.FromResult(b)
            select x + y;

        Assert.Equivalent(result, a + b);
    }

    [Property]
    public async Task ShouldTraverse(List<int> inputs)
    {
        var result = await inputs.Traverse(Task.FromResult);
        Assert.Equivalent(inputs, result);
    }

    [Property]
    public async Task ShouldSequence(List<int> inputs)
    {
        var result = await inputs.Select(Task.FromResult).Sequence();
        Assert.Equivalent(inputs, result);
    }
}
