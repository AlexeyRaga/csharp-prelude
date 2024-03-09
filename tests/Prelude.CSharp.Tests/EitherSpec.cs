using FluentAssertions;
using FsCheck.Xunit;

namespace Prelude.CSharp.Tests;

public sealed class EitherSpec
{
    [Property]
    public void ShouldMapLeft(byte input) =>
        Either<int, string>.NewLeft(input)
            .SelectLeft(x => x + 1)
            .SelectLeft(x => x * 2)
            .FromLeft(0)
            .Should().Be((input + 1) * 2);

    [Property]
    public void ShouldMapRight(byte input) =>
        Either<string, int>.NewRight(input)
            .Select(x => x + 1)
            .Select(x => x * 2)
            .FromRight(0)
            .Should().Be((input + 1) * 2);

    [Property]
    public void ShouldBindLeft(byte input, string error)
    {
        var leftValue = Either<string, int>.NewLeft(error);
        var rightValue = Either<string, int>.NewRight(input);

        rightValue.SelectMany(_ => leftValue).Should().BeEquivalentTo(leftValue);
        leftValue.SelectMany(_ => rightValue).Should().BeEquivalentTo(leftValue);
    }

    [Property]
    public void ShouldBindRight(byte input) =>
        Either<string, int>.NewRight(input)
            .SelectMany(x => Either<string, int>.NewRight(x + 1))
            .SelectMany(x => Either<string, int>.NewRight(x * 2))
            .FromRight(0)
            .Should().Be((input + 1) * 2);

    [Property]
    public void ShouldSwap(Either<int, string> either)
    {
        var swapped = either.Swap();
        Assert.Equivalent(swapped.Swap(), either);

        swapped.FromRight(0).Should().Be(either.FromLeft(0));
        swapped.FromLeft("").Should().Be(either.FromRight(""));
    }

    [Property]
    public void ShouldPatternMatch(Either<int, string> either)
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
    public void ShouldPatternMatchExpression(Either<int, string> either)
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
    public void ShouldSeparateLeftsAndRights(List<Either<int, string>> values) =>
        values
            .PartitionEithers()
            .Should()
            .BeEquivalentTo((values.CollectLefts(), values.CollectRights()));

    [Property]
    public void ShouldThrowLeft(Either<string, string> value)
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
    public async Task ShouldSelectAsync(Either<int, string> value)
    {
        async Task<T> GetValue<T>(T data) => await Task.FromResult(data);

        var result = await value
            .Select(async x => await GetValue(x + "!"))
            .SelectLeft(async x => await GetValue(x + "!"));

        var expected = value.BiSelect(x => x + "!", x => x + "!");

        Assert.Equivalent(expected, result);
    }

    [Property]
    public async Task ShouldSelectManyAsync(Either<string, string> value)
    {
        async Task<Either<L, R>> GetLeftValue<L, R>(L data) => await Task.FromResult(Either<L, R>.NewLeft(data));
        async Task<Either<L, R>> GetRightValue<L, R>(R data) => await Task.FromResult(Either<L, R>.NewRight(data));

        var result = await value
            .Select(async x => await GetRightValue<string, string>(x + "!").ConfigureAwait(false))
            .SelectManyLeft(async x => await GetLeftValue<string, string>(x + "!").ConfigureAwait(false));

        var expected = value.BiSelect(x => x + "!", x => x + "!");
        Assert.Equivalent(expected, result);
    }

    [Property]
    public void ShouldFlattenNestedEither(Either<string, string> value)
    {
        var result = Either<string, Either<string, string>>.NewRight(value).Flatten();
        Assert.Equivalent(value, result);
    }
}
