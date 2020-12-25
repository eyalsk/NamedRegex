namespace NamedRegex.Tests
{
    using System.Collections.Generic;

    using Shouldly;

    using Xunit;

    public class NamedRegexLexerTest
    {
        [Fact]
        public void Should_lex_when_pattern_is_empty()
        {
            NamedRegexLexer lexer = new NamedRegexLexer("");

            NamedRegexToken token = lexer.Lex();

            token.Kind.ShouldBe(NamedRegexTokenKind.EmptyPattern);
        }

        [Theory]
        [InlineData("x")]
        [InlineData(@"\{x\}")]
        [InlineData(@"\{x-")]
        //[InlineData(@"-x\}")]
        public void Should_lex_when_pattern_has_RegexPattern(string pattern)
        {
            var lexer = new NamedRegexLexer(pattern);
            var results = new List<NamedRegexToken>();

            while (true)
            {
                var token = lexer.Lex();

                results.Add(token);

                if (token.Kind == NamedRegexTokenKind.EndOfPattern) break;
            }

            results.ShouldContain(t => t.Kind == NamedRegexTokenKind.RegexPattern);
        }

        [Theory]
        [InlineData("{x}")]
        [InlineData("x{x}")]
        [InlineData("{x}x")]
        [InlineData("x{x}x")]
        public void Should_lex_when_pattern_has_NamedPattern(string pattern)
        {
            var lexer = new NamedRegexLexer(pattern);
            var results = new List<NamedRegexToken>();

            while (true)
            {
                var token = lexer.Lex();

                results.Add(token);

                if (token.Kind == NamedRegexTokenKind.EndOfPattern) break;
            }

            results.ShouldContain(t => t.Kind == NamedRegexTokenKind.NamedPattern);
        }

        [Theory]
        //[InlineData("x{")]
        [InlineData("x}")]
        //[InlineData(@"\{x}")]
        [InlineData(@"{x\}")]
        //[InlineData("{x{x}")]
        //[InlineData(@"{x\{x}")]
        public void Should_lex_when_pattern_has_Error(string pattern)
        {
            var lexer = new NamedRegexLexer(pattern);
            var results = new List<NamedRegexToken>();

            while (true)
            {
                var token = lexer.Lex();

                results.Add(token);

                if (token.Kind == NamedRegexTokenKind.EndOfPattern) break;
            }

            results.ShouldContain(t => t.Kind == NamedRegexTokenKind.Error);
        }
    }
}
