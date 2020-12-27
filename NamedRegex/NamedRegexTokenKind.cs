namespace NamedRegex
{
    public enum NamedRegexTokenKind
    {
        None,

        EmptyPattern,
        EndOfPattern,

        OpenedCurlyBrace,
        ClosedCurlyBrace,

        RegexCharacter,
        IdentifierCharacter,

        InvalidCurlyBrace,
        InvalidIdentifierCharacter
    }
}
