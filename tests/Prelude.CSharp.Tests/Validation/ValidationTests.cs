using System.Collections.Immutable;
using FluentAssertions;
using FsCheck.Xunit;

namespace Prelude.CSharp.Tests.ValidationTests;


public sealed class ValidationTests
{
    static ValidationTests()
    {
        AssertionOptions.AssertEquivalencyUsing(options => options.RespectingRuntimeTypes());
    }

    [Property]
    public void Should_return_result_of_applied_function(string strValue, int intValue)
    {
        var calculation = (string a, int b) => $"{a}-{b}";

        var val = Validation<string>.Pure(calculation);

        var validatedResult = val
            .Apply(Either<string>.Pure(strValue))
            .Apply(Either<string>.Pure(intValue))
            .Either;

        validatedResult
            .FromRight("")
            .Should()
            .BeEquivalentTo(calculation(strValue, intValue));
    }

    [Property]
    public void Should_capture_all_errors(string firstErrorValue, int successValue, string lastErrorValue)
    {
        var calculation = (string a, int b, string c) => $"{a}-{b}-{c}";

        var val = Validation<string>.Pure(calculation);

        var validatedResult = val
            .Apply(Either.Left<string, string>(firstErrorValue))
            .Apply(Either<string>.Pure(successValue))
            .Apply(Either.Left<string, string>(lastErrorValue))
            .Either;

        validatedResult
            .FromLeft(defaultValue: [])
            .Should()
            .BeEquivalentTo(firstErrorValue, lastErrorValue);
    }
}
