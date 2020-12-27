namespace NamedRegex.Driver
{
    using System;
    using System.Diagnostics;

    internal static class Program
    {
        private static void Main()
        {
            const string pattern = "ab{c}d{ef}g}h";

            var lexer = new NamedRegexLexer(pattern.AsMemory());

            Trace.Listeners.Clear();
            Trace.Listeners.Add(new ConsoleTraceListener());

            while (true)
            {
                var token = lexer.Lex();

                if (token.Kind == NamedRegexTokenKind.EndOfPattern) break;

                Console.WriteLine("--------------------------------------------------------------------------------------------------------");
                Console.WriteLine($" {token}\n\t\t[{token.Range.Start}-{token.Range.End}]\t\t{token.Kind}");
                Console.WriteLine("--------------------------------------------------------------------------------------------------------");
            }
        }
    }
}
