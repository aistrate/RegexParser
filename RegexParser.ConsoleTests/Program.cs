using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using ParserCombinators;
using ParserCombinators.ConsLists;
using ParserCombinators.Tests;
using ParserCombinators.Tests.Performance;
using ParserCombinators.Tests.Util;
using ParserCombinators.Util;
using RegexParser.Matchers;
using RegexParser.Patterns;
using RegexParser.Tests;
using RegexParser.Tests.Helpers;
using RegexParser.Tests.Matchers;
using RegexParser.Tests.Patterns;
using RegexParser.Tests.Performance;
using Msoft = System.Text.RegularExpressions;

namespace RegexParser.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //new CharClassMatcherTests(AlgorithmType.ImplicitDFA).Subtract();

                //PatternPerformanceTests.CharClassPatternTest();

                //RegexAssert.DisplayPattern(@"a|x[abc0-9]|c*");
                //RegexAssert.DisplayPattern(@"(a|b)x|c");
                //RegexAssert.DisplayPattern(@"abcd|defgh|012");

                //displayMatches("Therefore they took CS101 and EE201.", @"\w\w\S\S\S", AlgorithmType.ImplicitDFA);
                displayMatches("abbbc", @"(a|ab)bbbc", AlgorithmType.ImplicitDFA);

                //Console.WriteLine(formatMsoftMatches(Msoft.Regex.Matches("abbbc", @"(abb?b?b?c)|(bc)")));

                //Console.WriteLine(".$^{[(|)*+!?\\  - \b\n\b []09azAZ}".Distinct().OrderBy(c => c).AsString().Show());
                // "\b\n !$()*+-.09?AZ[\\]^az{|}"

                //testBacktracking2();
            }
            catch (Exception ex)
            {
                Console.WriteLine("TYPE:\n");
                for (Exception e = ex; e != null; e = e.InnerException)
                    Console.WriteLine(e.GetType().FullName);
                Console.WriteLine();

                Console.WriteLine("MESSAGE:\n");
                Console.WriteLine(ex.Message);

                if (!(ex is AssertionException))
                    Console.WriteLine();

                Console.WriteLine("STACK TRACE:\n");
                Console.WriteLine(formatStackTrace(ex.StackTrace));
            }

            Console.WriteLine();
        }

        private static void testBacktracking()
        {
            var letExpr = stringParser("let");
            var identifier = CharParsers.Many1(CharParsers.Satisfy(c => char.IsLetter(c)));

            var expr = CharParsers.Choice(letExpr, identifier);

            displayResult(runParser(expr, "lexical"));
        }

        private static void testBacktracking2()
        {
            var alternatives = CharParsers.Choice(
                                    stringParser("a"),
                                    stringParser("ab")
                               );

            var pattern = from ss in CharParsers.Sequence(new[]
                                     {
                                         alternatives,
                                         stringParser("bbb"),
                                         stringParser("c")
                                     })
                          select ss.SelectMany(s => s);

            displayResult(runParser(pattern, "abbbc"));
            displayResult(runParser(pattern, "abbbbc"));
        }

        private static Parser<char, IEnumerable<char>> stringParser(string s)
        {
            return CharParsers.Sequence(s.Select(c => CharParsers.Char(c)));
        }

        private static Result<char, TValue> runParser<TValue>(Parser<char, TValue> parser, string input)
        {
            Console.WriteLine("Input:  {0}", input.Show());
            return parser(new ArrayConsList<char>(input));
        }

        private static void displayResult(Result<char, string> result)
        {
            displayResult(result, v => v.Show());
        }

        private static void displayResult(Result<char, IEnumerable<char>> result)
        {
            displayResult(result, v => v.AsString().Show());
        }

        private static void displayResult<TValue>(Result<char, TValue> result, Func<TValue, string> toString)
        {
            if (result != null)
            {
                Console.WriteLine("Result: {0}", toString(result.Value));
                Console.WriteLine("Rest:   {0}\n", result.Rest.AsEnumerable().AsString().Show());
            }
            else
                Console.WriteLine("Result: null\n");
        }

        private static string formatStackTrace(string stackTrace)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(Program));
            string assemblyName = assembly.FullName.Substring(0, assembly.FullName.IndexOf(", "));
            int assemblyNameIndex = assembly.Location.IndexOf(assemblyName);

            if (assemblyNameIndex >= 0)
            {
                string parentFolder = assembly.Location.Substring(0, assemblyNameIndex);
                stackTrace = stackTrace.Replace(parentFolder, "");
            }

            return stackTrace.Replace("\n", "\n\n")
                             .Replace(" in ", "\n   in ")
                             .Replace(".cs:line ", ".cs\n   ln ");
        }

        private static void displayMatches(string input, string pattern, AlgorithmType algorithmType)
        {
            //RegexAssert.DisplayPattern(pattern);

            var matches = new Regex2(pattern, algorithmType).Matches(input);
            RegexAssert.DisplayMatches(input, pattern, algorithmType, matches);
        }

        private static string formatMsoftMatches(Msoft.MatchCollection matches)
        {
            Msoft.Match[] matchArr = matches.Cast<Msoft.Match>().ToArray();

            return string.Format("{0} matches: [{1}]",
                                 matchArr.Length,
                                 matchArr.Select(m => formatMsoftMatch(m))
                                         .JoinStrings());
        }

        private static string formatMsoftMatch(Msoft.Match match)
        {
            return string.Format("({0}, {1}, {2})", match.Index, match.Length, match.Value.Show());
        }
    }
}
