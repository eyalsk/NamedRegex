namespace NamedRegex
{
    using System;

    public sealed class NamedRegexToken
    {
        public NamedRegexToken(Range range, NamedRegexTokenKind kind, ReadOnlyMemory<char> value)
        {
            if (range.Start.Value < 0) throw new ArgumentOutOfRangeException(nameof(range));
            if (range.End.Value < 0 || range.End.Value < range.Start.Value) throw new ArgumentOutOfRangeException(nameof(range));
            if (!Enum.IsDefined(typeof(NamedRegexTokenKind), kind)) throw new ArgumentException(nameof(kind));

            Range = range;
            Kind = kind;
            Value = value;
        }

        public Range Range { get; }

        public NamedRegexTokenKind Kind { get; }

        public ReadOnlyMemory<char> Value { get; }

        public override string ToString() =>
            Kind == NamedRegexTokenKind.EndOfPattern ? string.Empty : Value.ToString();
    }
}
