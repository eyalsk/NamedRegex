namespace NamedRegex
{
    using System;

    public sealed class NamedRegexLexer
    {
        private const char EndOfString = '\0';

        private readonly ReadOnlyMemory<char> _pattern;

        private int _currentIndex;
        private NamedRegexTokenKind _prevTokenKind;

        public NamedRegexLexer(ReadOnlyMemory<char> pattern)
        {
            _pattern = pattern;
        }

        public NamedRegexLexer(string pattern) : this(pattern.AsMemory()) { }

        public NamedRegexToken Lex()
        {
            var startIndex = _currentIndex;

            if (_pattern.IsEmpty)
            {
                return CreateToken(NamedRegexTokenKind.EmptyPattern);
            }

            while (true)
            {
                var ch = _currentIndex < _pattern.Length ? _pattern.Span[_currentIndex] : EndOfString;

                _currentIndex++;

                switch (ch)
                {
                    case '{':
                    {
                        _prevTokenKind = NamedRegexTokenKind.OpenedCurlyBrace;

                        if (_currentIndex > 0)
                        {
                            return CreateTokenWithRange(NamedRegexTokenKind.RegexPattern, startIndex..(_currentIndex - 1));
                        }

                        break;
                    }
                    case '}':
                    {
                        var range = startIndex..(_currentIndex - 1);
                        var prevTokenKind = _prevTokenKind;

                        _prevTokenKind = NamedRegexTokenKind.ClosedCurlyBrace;

                        if (prevTokenKind == NamedRegexTokenKind.OpenedCurlyBrace)
                        {
                            return CreateTokenWithRange(NamedRegexTokenKind.NamedPattern, range);
                        }

                        return CreateTokenWithRange(NamedRegexTokenKind.Error, range);
                    }
                    case EndOfString:
                    {
                        return CreateToken(NamedRegexTokenKind.EndOfPattern);
                    }
                    default:
                    {
                        if (_prevTokenKind == NamedRegexTokenKind.OpenedCurlyBrace && !char.IsLetterOrDigit(ch))
                        {
                            _prevTokenKind = NamedRegexTokenKind.Error;
                        }
                        else if (_currentIndex >= _pattern.Length)
                        {
                            if (_prevTokenKind is NamedRegexTokenKind.OpenedCurlyBrace or NamedRegexTokenKind.Error)
                            {
                                return CreateToken(NamedRegexTokenKind.Error);
                            }

                            return CreateToken(NamedRegexTokenKind.RegexPattern);
                        }

                        break;
                    }
                }
            }

            NamedRegexToken CreateTokenWithRange(NamedRegexTokenKind tokenKind, Range range)
            {
                var value = tokenKind != NamedRegexTokenKind.EndOfPattern ? _pattern[range] : ReadOnlyMemory<char>.Empty;

                return new NamedRegexToken(range, tokenKind, value);
            }

            NamedRegexToken CreateToken(NamedRegexTokenKind tokenKind) => CreateTokenWithRange(tokenKind, startIndex.._currentIndex);
        }
    }
}
