using System.Text.Json;
using FsCheck.Xunit;

namespace Prelude.CSharp.Tests.OptionTests;

public record Person(string FirstName, Option<string> LastName, Option<int> Age);

public sealed class OptionJsonTests
{
    [Property]
    public void ShouldSerialiseSomeString(string value)
    {
        var option = Option.Some(value);
        var serialized = JsonSerializer.Serialize(value);
        var deserialized = JsonSerializer.Deserialize<Option<string>>(serialized);
        Assert.Equivalent(option, deserialized);
    }

    [Property]
    public void ShouldSerialiseSomeInt(int value)
    {
        var option = Option.Some(42);
        var serialized = JsonSerializer.Serialize(option);
        var deserialized = JsonSerializer.Deserialize<Option<int>>(serialized);
        Assert.Equivalent(option, deserialized);
    }

    [Fact]
    public void ShouldSerialiseNoneString()
    {
        var option = Option.None<string>();
        var serialized = JsonSerializer.Serialize(option);
        var deserialized = JsonSerializer.Deserialize<Option<string>>(serialized);
        Assert.Equivalent(option, deserialized);
    }

    [Fact]
    public void ShouldSerialiseNoneInt()
    {
        var option = Option.None<int>();
        var serialized = JsonSerializer.Serialize(option);
        var deserialized = JsonSerializer.Deserialize<Option<int>>(serialized);
        Assert.Equivalent(option, deserialized);
    }

    [Property]
    public void ShouldSerializeComplexInstances(Person person)
    {
        var serialized = JsonSerializer.Serialize(person);
        var deserialized = JsonSerializer.Deserialize<Person>(serialized);
        Assert.Equivalent(person, deserialized);
    }
}
