using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;

namespace Prelude.CSharp.Tests.EitherTests;

public sealed class EitherTests
{
    static EitherTests()
    {
        AssertionOptions.AssertEquivalencyUsing(options => options.RespectingRuntimeTypes());
    }

    [Property]
    public void SelectLeft_should_map_left_value(byte input) =>
        Either<int, string>.NewLeft(input)
            .SelectLeft(x => x + 1)
            .SelectLeft(x => x * 2)
            .FromLeft(0)
            .Should().Be((input + 1) * 2);

    [Property]
    public void Select_should_map_right_value(byte input) =>
        Either<string, int>.NewRight(input)
            .Select(x => x + 1)
            .Select(x => x * 2)
            .FromRight(0)
            .Should().Be((input + 1) * 2);

    [Property]
    public void SelectMany_should_preserve_left_value(byte input, string error)
    {
        var leftValue = Either<string, int>.NewLeft(error);
        var rightValue = Either<string, int>.NewRight(input);

        rightValue.SelectMany(_ => leftValue).Should().BeEquivalentTo(leftValue);
        leftValue.SelectMany(_ => rightValue).Should().BeEquivalentTo(leftValue);
    }

    [Property]
    public void SelectMany_should_bind_right_value(byte input) =>
        Either<string, int>.NewRight(input)
            .SelectMany(x => Either<string, int>.NewRight(x + 1))
            .SelectMany(x => Either<string, int>.NewRight(x * 2))
            .FromRight(0)
            .Should().Be((input + 1) * 2);

    [Property]
    public void Swap_should_swap_left_and_right(Either<int, string> either)
    {
        var swapped = either.Swap();
        Assert.Equivalent(swapped.Swap(), either);

        swapped.FromRight(0).Should().Be(either.FromLeft(0));
        swapped.FromLeft("").Should().Be(either.FromRight(""));
    }

    [Property]
    public void Switch_statement_should_work(Either<int, string> either)
    {
        switch (either)
        {
            case Either<int, string>.Left left:
                left.Value.Should().Be(either.FromLeft(0));
                break;

            case Either<int, string>.Right right:
                right.Value.Should().Be(either.FromRight(""));
                break;
        }
    }

    [Property]
    public void Switch_expression_should_work(Either<int, string> either)
    {
        var result = either switch
        {
            Either<int, string>.Left left => left.Value.ToString(),
            Either<int, string>.Right right => right.Value,
            _ => throw new InvalidProgramException("What else can it be?!")
        };

        result.Should().Be(either.Fold(x => x.ToString(), x => x));
    }

    [Property]
    public void PartitionEithers_should_separate_lefts_from_rights(List<Either<int, string>> values) =>
        values
            .PartitionEithers()
            .Should()
            .BeEquivalentTo((values.CollectLefts(), values.CollectRights()));

    [Property]
    public void Throw_left_should_throw_exceptions(Either<string, string> value)
    {
        var act = () =>
        {
            var exceptional = value
                .SelectLeft(x => new InvalidOperationException(x + "!"))
                .Select(x => new InvalidOperationException(x + "!"));

            if (exceptional.IsLeft) exceptional.ThrowLeft();
            else exceptional.ThrowRight();
        };


        act.Should()
            .Throw<InvalidOperationException>()
            .Where(x => x.Message.EndsWith("!"));
    }


    [Property(MaxTest = 3)]
    public async Task BiSelect_should_transform_left_and_right(Either<int, string> value)
    {
        var result = await value
            .Select(async x => await Task.FromResult(x + "!"))
            .SelectLeft(async x => await Task.FromResult(x + "!"));

        var expected = value.BiSelect(x => x + "!", x => x + "!");

        Assert.Equivalent(expected, result);
    }

    [Property]
    public void Flatten_should_handle_nested_values(Either<string, string> value)
    {
        var result = Either<string, Either<string, string>>.NewRight(value).Flatten();
        Assert.Equivalent(value, result);
    }

    [Property]
    public void Traverse_should_return_right_when_all_elements_are_right(List<string> values)
    {
        var result = values.Traverse(Either<bool, string>.NewRight);
        result.Should().BeEquivalentTo(Either<bool, List<string>>.NewRight(values));
    }

    [Property]
    public void Traverse_should_return_left_when_any_value_is_left(NonEmptyArray<int> values)
    {
        var threshold = values.Average();
        var result = values.Traverse(x => x >= threshold ? Either<bool, int>.NewLeft(true) : Either<bool, int>.NewRight(x));
        result.Should().BeEquivalentTo(Either<bool, List<string>>.NewLeft(true));
    }

    [Property]
    public void Traverse_should_early_return(NonEmptyArray<int> values)
    {
        var threshold = values.Count() / 2 + 1;
        var result = values
            .Zip(Enumerable.Range(1, values.Count()))
            .Traverse(x => x.Second > threshold ? throw new Exception() : Either<bool, int>.NewLeft(true));
        result.Should().BeEquivalentTo(Either<bool, List<string>>.NewLeft(true));
    }
}
