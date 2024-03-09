using FluentAssertions;
using FsCheck.Xunit;

namespace Prelude.CSharp.Tests;

public sealed class OptionSpec
{
    [Property]
    public void ShouldMap(byte input) =>
        Option.Some(input)
            .Select(x => x + 1)
            .Select(x => x * 2)
            .GetValueOrDefault(0)
            .Should().Be((input + 1) * 2);

    [Property]
    public void ShouldBind(byte input) =>
        Option.Some(input)
            .SelectMany(x => Option.Some(x + 1))
            .SelectMany(x => Option.Some(x * 2))
            .GetValueOrDefault(0)
            .Should().Be((input + 1) * 2);

    [Property]
    public void ShouldPatternMatch(Option<int> input, int defaultValue)
    {
        switch (input)
        {
            case Option<int>.Some some:
                some.Value.Should().Be(input.GetValueOrDefault(0));
                break;

            case Option<int>.None:
                input.GetValueOrDefault(defaultValue).Should().Be(defaultValue);
                break;
        }
    }

    [Property]
    public void ShouldPatternMatchExpression(Option<int> input, string defaultValue)
    {
        var result = input switch
        {
            Option<int>.Some some => some.Value.ToString(),
            Option<int>.None => defaultValue,
            _ => throw new InvalidProgramException("What else can it be?!")
        };

        result.Should().Be(input.Fold(x => x.ToString(), () => defaultValue));
    }

    [Property]
    public void ShouldCollectValues(List<Option<int>> inputs)
    {
        var result = inputs.CollectValues().ToList();
        var (nay, yeah) = inputs
            .Select(x => x.ToEither<string, int>(() => "Error"))
            .PartitionEithers();

        result.Should().BeEquivalentTo(yeah);
    }

    [Property]
    public void ShouldFlattenNestedOptions(int value)
    {
        var inner = Option.Some(value);
        var result = Option.Some(inner).Flatten();
        Assert.Equivalent(inner, result);
    }
}
