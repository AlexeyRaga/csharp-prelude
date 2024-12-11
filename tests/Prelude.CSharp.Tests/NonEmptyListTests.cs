using System.Text.Json;
using Prelude.CSharp.FSCheck;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;

namespace Prelude.CSharp.Tests;

[Properties(Arbitrary = [typeof(PreludeArb)])]
public sealed class NonEmptyListTests
{
    static NonEmptyListTests()
    {
        AssertionOptions.AssertEquivalencyUsing(options => options.RespectingRuntimeTypes());
    }

    [Property]
    public void Select_should_map_value(NonEmptyList<byte> input) =>
        input
            .Select(x => x + 1)
            .Select(x => x * 2)
            .Should().BeEquivalentTo(input.Select(x => (x + 1) * 2));

    [Property]
    public void Should_roundtrip_json(NonEmptyList<byte> input)
    {
        var serialized = JsonSerializer.Serialize(input);
        var deserialized = JsonSerializer.Deserialize<NonEmptyList<byte>>(serialized);
        deserialized.Should().BeEquivalentTo(input);
    }

    [Fact]
    public void Should_not_deserialize_empty_list()
    {
        var serialized = JsonSerializer.Serialize(new List<byte>());
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<NonEmptyList<byte>>(serialized));
    }

    [Fact]
    public void Should_not_construct_from_empty_list() =>
        new List<byte>().ToNonEmptyList().Should().Be(Option.None<NonEmptyList<byte>>());

    [Property]
    public void Should_construct_from_non_empty_sequence(NonEmptyArray<byte> nel) =>
        nel.Item.ToNonEmptyList().Should().BeEquivalentTo(Option.Some(nel));

    [Property]
    public void Should_remove_last_element_to_none(byte item) =>
        new NonEmptyList<byte>(item, []).Remove(item).Should().Be(Option.None<NonEmptyList<byte>>());

    [Property]
    public void Should_remove_first_element_to_none(byte head, NonEmptyArray<byte> tail) =>
        NonEmptyList
            .Create(head, tail.Item)
            .Remove(head)
            .Should().BeEquivalentTo(Option.Some(tail));

    [Property]
    public void Should_produce_distinct_values(NonEmptyList<byte> input) =>
        input.Distinct().Should().BeEquivalentTo(input.ToList().Distinct());

    [Property]
    public void Should_produce_distinct_values_with_DistinctBy(NonEmptyList<byte> input) =>
        input.DistinctBy(x => x).Should().BeEquivalentTo(input.Distinct());
}
