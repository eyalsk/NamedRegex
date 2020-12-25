namespace NamedRegex.Driver
{
    using System;

    using Shouldly;

    using Xunit;

    public sealed class NamedRegexTokenTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Should_not_throw_when_range_start_above_or_equal_to_zero(int start)
        {
            Should.NotThrow(() =>
                new NamedRegexToken(range: start..(start + 1), kind: NamedRegexTokenKind.None, value: ReadOnlyMemory<char>.Empty));
        }

        [Fact]
        public void Should_throw_when_range_start_below_zero()
        {
            Should.Throw<ArgumentOutOfRangeException>(() =>
                new NamedRegexToken(range: -1..0, kind: NamedRegexTokenKind.None, value: ReadOnlyMemory<char>.Empty));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Should_not_throw_when_range_end_above_or_equal_to_zero(int end)
        {
            Should.NotThrow(() =>
                new NamedRegexToken(range: 0..end, kind: NamedRegexTokenKind.None, value: ReadOnlyMemory<char>.Empty));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Should_throw_when_range_end_below_zero_or_below_start(int end)
        {
            Should.Throw<ArgumentOutOfRangeException>(() =>
                new NamedRegexToken(range: 1..end, kind: NamedRegexTokenKind.None, value: ReadOnlyMemory<char>.Empty));
        }

        [Fact]
        public void Should_not_throw_when_kind_is_defined()
        {
            Should.NotThrow(() =>
                new NamedRegexToken(range: .., kind: NamedRegexTokenKind.None, value: ReadOnlyMemory<char>.Empty));
        }

        [Fact]
        public void Should_throw_when_kind_is_undefined()
        {
            Should.Throw<ArgumentException>(() =>
                new NamedRegexToken(range: .., kind: (NamedRegexTokenKind)int.MinValue, value: ReadOnlyMemory<char>.Empty));
        }
    }
}
