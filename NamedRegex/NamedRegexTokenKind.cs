﻿namespace NamedRegex
{
    public enum NamedRegexTokenKind
    {
        None,

        OpenedCurlyBrace,
        ClosedCurlyBrace,

        NamedPattern,
        RegexPattern,
        EndOfPattern,
        EmptyPattern,

        Error
    }
}
