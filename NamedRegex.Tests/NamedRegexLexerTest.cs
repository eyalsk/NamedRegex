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
        [InlineData("x",
            new NamedRegexTokenKind[] {
                NamedRegexTokenKind.RegexCharacter,
                NamedRegexTokenKind.EndOfPattern })]
        [InlineData(@"\{x-",
            new NamedRegexTokenKind[] {
                NamedRegexTokenKind.RegexCharacter,
                NamedRegexTokenKind.RegexCharacter,
                NamedRegexTokenKind.RegexCharacter,
                NamedRegexTokenKind.EndOfPattern })]
        [InlineData(@"-x\}",
            new NamedRegexTokenKind[] {
                NamedRegexTokenKind.RegexCharacter,
                NamedRegexTokenKind.RegexCharacter,
                NamedRegexTokenKind.RegexCharacter,
                NamedRegexTokenKind.EndOfPattern })]
        [InlineData(@"\{x\}",
            new NamedRegexTokenKind[] {
                NamedRegexTokenKind.RegexCharacter,
                NamedRegexTokenKind.RegexCharacter,
                NamedRegexTokenKind.RegexCharacter,
                NamedRegexTokenKind.EndOfPattern })]
        public void Should_lex_regex_patterns(string pattern, NamedRegexTokenKind[] kinds)
        {
            var lexer = new NamedRegexLexer(pattern);
            var results = new List<NamedRegexTokenKind>();

            while (true)
            {
                var token = lexer.Lex();

                results.Add(token.Kind);

                if (token.Kind == NamedRegexTokenKind.EndOfPattern) break;
            }

            results.ShouldBe(kinds);
        }

        [Theory]
        [InlineData("{x}",
            new NamedRegexTokenKind[] {
                NamedRegexTokenKind.OpenedCurlyBrace,
                NamedRegexTokenKind.IdentifierCharacter,
                NamedRegexTokenKind.ClosedCurlyBrace,
                NamedRegexTokenKind.EndOfPattern })]
        [InlineData("x{x}",
            new NamedRegexTokenKind[] {
                NamedRegexTokenKind.RegexCharacter,
                NamedRegexTokenKind.OpenedCurlyBrace,
                NamedRegexTokenKind.IdentifierCharacter,
                NamedRegexTokenKind.ClosedCurlyBrace,
                NamedRegexTokenKind.EndOfPattern })]
        [InlineData("{x}x",
            new NamedRegexTokenKind[] {
                NamedRegexTokenKind.OpenedCurlyBrace,
                NamedRegexTokenKind.IdentifierCharacter,
                NamedRegexTokenKind.ClosedCurlyBrace,
                NamedRegexTokenKind.RegexCharacter,
                NamedRegexTokenKind.EndOfPattern })]
        [InlineData("x{x}x",
            new NamedRegexTokenKind[] {
                NamedRegexTokenKind.RegexCharacter,
                NamedRegexTokenKind.OpenedCurlyBrace,
                NamedRegexTokenKind.IdentifierCharacter,
                NamedRegexTokenKind.ClosedCurlyBrace,
                NamedRegexTokenKind.RegexCharacter,
                NamedRegexTokenKind.EndOfPattern })]
        public void Should_lex_named_regex_patterns(string pattern, NamedRegexTokenKind[] kinds)
        {
            var lexer = new NamedRegexLexer(pattern);
            var results = new List<NamedRegexTokenKind>();

            while (true)
            {
                var token = lexer.Lex();

                results.Add(token.Kind);

                if (token.Kind == NamedRegexTokenKind.EndOfPattern) break;
            }

            results.ShouldBe(kinds);
        }

        /*[Theory]
        //[InlineData("x{")]
        [InlineData("x}",
            new NamedRegexTokenKind[] {
                NamedRegexTokenKind.Error,
                NamedRegexTokenKind.EndOfPattern })]
        //[InlineData(@"\{x}")]
        [InlineData(@"{x\}",
            new NamedRegexTokenKind[] {
                NamedRegexTokenKind.Error,
                NamedRegexTokenKind.EndOfPattern })]
        //[InlineData("{x{x}")]
        //[InlineData(@"{x\{x}")]
        public void Should_lex_when_pattern_has_Error(string pattern, NamedRegexTokenKind[] kinds)
        {
            var lexer = new NamedRegexLexer(pattern);
            var results = new List<NamedRegexTokenKind>();

            while (true)
            {
                var token = lexer.Lex();

                results.Add(token.Kind);

                if (token.Kind == NamedRegexTokenKind.EndOfPattern) break;
            }

            results.ShouldBe(kinds);
        }*/
    }
}
