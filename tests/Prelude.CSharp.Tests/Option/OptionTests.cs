using FluentAssertions;
using Hedgehog.Xunit;


namespace Prelude.CSharp.Tests.OptionTests;

[Properties(typeof(GeneratorsConfig))]
public sealed class OptionTests
{
    static OptionTests()
    {
        AssertionOptions.AssertEquivalencyUsing(options => options.RespectingRuntimeTypes());
    }

    [Property]
    public void Select_should_map_value(byte input) =>
        Option.Some(input)
            .Select(x => x + 1)
            .Select(x => x * 2)
            .GetValueOrDefault(0)
            .Should().Be((input + 1) * 2);

    [Property]
    public void SelectMany_should_bind_value(byte input) =>
        Option.Some(input)
            .SelectMany(x => Option.Some(x + 1))
            .SelectMany(x => Option.Some(x * 2))
            .GetValueOrDefault(0)
            .Should().Be((input + 1) * 2);

    [Property]
    public void Switch_statement_should_work(Option<int> input, int defaultValue)
    {
        switch (input)
        {
            case {IsSome: true} some:
                some.GetValueOrDefault(0).Should().Be(input.GetValueOrDefault(1));
                break;

            default:
                input.GetValueOrDefault(defaultValue).Should().Be(defaultValue);
                break;
        }
    }

    [Property]
    public void CollectValues_should_remove_nones(List<int> inputs)
    {
        var listWithNones =
            inputs.SelectMany(x => new List<Option<int>> { Option.Some(x), Option.None<int>() });

        var result = listWithNones.CollectValues().ToList();

        result.Should().BeEquivalentTo(inputs);
    }

    [Property]
    public void Flatten_should_squash_nested_options(int value)
    {
        var inner = Option.Some(value);
        var result = Option.Some(inner).Flatten();
        Assert.Equivalent(inner, result);
    }

    [Property]
    public void Traverse_should_work(List<string> values)
    {
        var result = values.Traverse(Option.Some);
        result.Should().BeEquivalentTo(Option.Some(values));
    }

    [Property]
    public void Traverse_should_return_none_when_any_value_is_none(List<int> values, int threshold)
    {
        var result = values.Traverse(x => x > threshold ? Option.None<int>() : Option.Some(x));
        if (values.Any(x => x > threshold))
            result.Should().BeEquivalentTo(Option.None<IEnumerable<int>>());
        else
            result.Should().BeEquivalentTo(Option.Some(values));
    }

    [Property]
    public void Traverse_should_early_return(NonEmptyList<int> values)
    {
        var threshold = values.Count / 2 + 1;
        var result = values
            .Zip(Enumerable.Range(1, values.Count()))
            .Traverse(x => x.Second > threshold ? throw new Exception() : Option.None<int>());
        result.Should().BeEquivalentTo(Option.None<int>());
    }
}
