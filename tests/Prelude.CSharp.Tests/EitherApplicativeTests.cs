using System;
using FluentAssertions;
using FsCheck.Xunit;

namespace Prelude.CSharp.Tests;

public sealed class EitherApplicativeTests
{
    static EitherApplicativeTests()
    {
        AssertionOptions.AssertEquivalencyUsing(options => options.RespectingRuntimeTypes());
    }

    [Property]
    public void Should_return_result_of_applied_function(string strValue, int intValue)
    {
        var calculation = (string a, int b) => $"{a}-{b}";

        var eitherFunc = Either<string>.Right(calculation);

        var result = eitherFunc
            .Apply(Either<string>.Right(strValue))
            .Apply(Either<string>.Right(intValue));

        result
            .FromRight("")
            .Should()
            .BeEquivalentTo(calculation(strValue, intValue));
    }

    [Property]
    public void Should_return_left_if_any_left(string leftValue, string strValue, int intValue)
    {
        var calculation = (string a, int b) => $"{a}-{b}";

        var eitherFunc = Either<string>.Right(calculation);

        var result = eitherFunc
            .Apply(Either.Left<string, string>(leftValue))
            .Apply(Either<string>.Right(intValue));

        result
            .FromLeft("")
            .Should()
            .BeEquivalentTo(leftValue);
    }

    [Property]
    public void Should_return_left_if_function_is_left(string leftValue, string strValue, int intValue)
    {
        var eitherFunc = Either.Left<string, Func<string, int, string>>(leftValue);

        var result = eitherFunc
            .Apply(Either<string>.Right(strValue))
            .Apply(Either<string>.Right(intValue));

        result
            .FromLeft("")
            .Should()
            .BeEquivalentTo(leftValue);
    }

    [Property]
    public void Should_apply_function_with_three_parameters(string strValue, int intValue, double doubleValue)
    {
        var calculation = (string a, int b, double c) => $"{a}-{b}-{c}";

        var eitherFunc = Either<string>.Right(calculation);

        var result = eitherFunc
            .Apply(Either<string>.Right(strValue))
            .Apply(Either<string>.Right(intValue))
            .Apply(Either<string>.Right(doubleValue));

        result
            .FromRight("")
            .Should()
            .BeEquivalentTo(calculation(strValue, intValue, doubleValue));
    }

    [Property]
    public void Should_return_left_if_any_left_with_three_parameters(string leftValue, string strValue, int intValue, double doubleValue)
    {
        var calculation = (string a, int b, double c) => $"{a}-{b}-{c}";

        var eitherFunc = Either<string>.Right(calculation);

        var result = eitherFunc
            .Apply(Either.Left<string, string>(leftValue))
            .Apply(Either<string>.Right(intValue))
            .Apply(Either<string>.Right(doubleValue));

        result
            .FromLeft("")
            .Should()
            .BeEquivalentTo(leftValue);
    }
}
