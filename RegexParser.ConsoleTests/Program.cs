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

                //RegexAssert.DisplayPattern(@"[a-z-[m-x]-]");
                //displayMatches("Therefore they took CS101 and EE201.", @"\w\w\S\S\S", AlgorithmType.ImplicitDFA);

                //Console.WriteLine(formatMsoftMatches(Msoft.Regex.Matches("abcd", @"\w")));

                //Console.WriteLine(new string(".$^{[(|)*+!?\\  - \b\n\b []09azAZ}".Distinct().OrderBy(c => c).ToArray()).Show());
                // "\b\n !$()*+-.09?AZ[\\]^az{|}"

                testBacktracking();
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
            Func<string, Parser<char, IEnumerable<char>>> stringParser = s => CharParsers.Sequence(s.Select(c => CharParsers.Char(c)));

            var letExpr = stringParser("let");
            var identifier = CharParsers.Many1(CharParsers.Satisfy(c => char.IsLetter(c)));

            var expr = CharParsers.Either(letExpr, identifier);

            var result = expr(new ArrayConsList<char>("lexical"));

            displayResult(result);
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
            Console.WriteLine("Value: {0}", toString(result.Value));
            Console.WriteLine("Rest:  {0}", result.Rest.AsEnumerable().AsString().Show());
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

            return string.Format("{0} matches: [", matchArr.Length) +
                   string.Join(", ", matchArr.Select(m => formatMsoftMatch(m)).ToArray()) +
                   "]";
        }

        private static string formatMsoftMatch(Msoft.Match match)
        {
            return string.Format("({0}, {1}, {2})", match.Index, match.Length, match.Value.Show());
        }
    }
}
