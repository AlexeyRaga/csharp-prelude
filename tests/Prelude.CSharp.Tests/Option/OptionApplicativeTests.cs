using FluentAssertions;
using FsCheck.Xunit;

namespace Prelude.CSharp.Tests.OptionTests;

public sealed class OptionApplicativeTests
{
    [Property]
    public void Should_return_result_of_applied_function(string strValue, int intValue)
    {
        var calculation = (string a, int b) => $"{a}-{b}";

        var optionFunc = Option.Some(calculation);

        var result = optionFunc
            .Apply(Option.Some(strValue))
            .Apply(Option.Some(intValue));

        result
            .GetValueOrDefault("")
            .Should()
            .BeEquivalentTo(calculation(strValue, intValue));
    }

    [Property]
    public void Should_return_none_if_any_none(string strValue, int intValue)
    {
        var calculation = (string a, int b) => $"{a}-{b}";

        var optionFunc = Option.Some(calculation);

        var result = optionFunc
            .Apply(Option.None<string>())
            .Apply(Option.Some(intValue));

        result
            .IsNone
            .Should()
            .BeTrue();
    }

    [Property]
    public void Should_return_none_if_function_is_none(string strValue, int intValue)
    {
        var optionFunc = Option.None<Func<string, int, string>>();

        var result = optionFunc
            .Apply(Option.Some(strValue))
            .Apply(Option.Some(intValue));

        result
            .IsNone
            .Should()
            .BeTrue();
    }

    [Property]
    public void Should_apply_function_with_three_parameters(string strValue, int intValue, double doubleValue)
    {
        var calculation = (string a, int b, double c) => $"{a}-{b}-{c}";

        var optionFunc = Option.Some(calculation);

        var result = optionFunc
            .Apply(Option.Some(strValue))
            .Apply(Option.Some(intValue))
            .Apply(Option.Some(doubleValue));

        result
            .GetValueOrDefault("")
            .Should()
            .BeEquivalentTo(calculation(strValue, intValue, doubleValue));
    }

    [Property]
    public void Should_return_none_if_any_none_with_three_parameters(string strValue, int intValue, double doubleValue)
    {
        var calculation = (string a, int b, double c) => $"{a}-{b}-{c}";

        var optionFunc = Option.Some(calculation);

        var result = optionFunc
            .Apply(Option.None<string>())
            .Apply(Option.Some(intValue))
            .Apply(Option.Some(doubleValue));

        result
            .IsNone
            .Should()
            .BeTrue();
    }
}
