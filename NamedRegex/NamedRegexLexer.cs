namespace NamedRegex
{
    using System;

    public sealed class NamedRegexLexer
    {
        private const char EndOfString = '\0';
        private const char EscapeChar = '\\';

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
                var prevChar = _currentIndex > 0 ? _pattern.Span[_currentIndex - 1] : ch;

                _currentIndex++;

                switch (ch)
                {
                    case '{':
                    {
                        if (_prevTokenKind is NamedRegexTokenKind.OpenedCurlyBrace)
                        {
                            return CreateToken(NamedRegexTokenKind.InvalidCurlyBrace);
                        }
                        else
                        {
                            _prevTokenKind = prevChar is EscapeChar
                                ? NamedRegexTokenKind.RegexCharacter
                                : NamedRegexTokenKind.OpenedCurlyBrace;
                        }

                        return CreateToken(_prevTokenKind);
                    }
                    case '}':
                    {
                        if (prevChar is EscapeChar)
                        {
                            _prevTokenKind = _prevTokenKind is NamedRegexTokenKind.OpenedCurlyBrace
                                ? NamedRegexTokenKind.InvalidCurlyBrace
                                : NamedRegexTokenKind.RegexCharacter;
                        }
                        else
                        {
                            _prevTokenKind = _prevTokenKind is NamedRegexTokenKind.ClosedCurlyBrace
                                ? NamedRegexTokenKind.InvalidCurlyBrace
                                : NamedRegexTokenKind.ClosedCurlyBrace;
                        }

                        return CreateToken(_prevTokenKind);
                    }
                    case EscapeChar:
                    {
                        break;
                    }
                    case EndOfString:
                    {
                        return CreateToken(NamedRegexTokenKind.EndOfPattern);
                    }
                    default:
                    {
                        if (_prevTokenKind is NamedRegexTokenKind.OpenedCurlyBrace)
                        {
                            return char.IsLetterOrDigit(ch)
                                ? CreateToken(NamedRegexTokenKind.IdentifierCharacter)
                                : CreateToken(NamedRegexTokenKind.InvalidIdentifierCharacter);
                        }
                        else
                        {
                            return CreateToken(NamedRegexTokenKind.RegexCharacter);
                        }
                    }
                }
            }

            NamedRegexToken CreateToken(NamedRegexTokenKind tokenKind)
            {
                var range = startIndex.._currentIndex;
                var value = tokenKind is not NamedRegexTokenKind.EndOfPattern
                                ? _pattern[range]
                                : ReadOnlyMemory<char>.Empty;

                return new NamedRegexToken(range, tokenKind, value);
            }
        }
    }
}
