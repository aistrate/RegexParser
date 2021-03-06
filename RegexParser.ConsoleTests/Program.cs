﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using ParserCombinators;
using ParserCombinators.Tests;
using ParserCombinators.Tests.Performance;
using RegexParser.Matchers;
using RegexParser.Patterns;
using RegexParser.Tests;
using RegexParser.Tests.Asserts;
using RegexParser.Tests.Matchers;
using RegexParser.Tests.Patterns;
using RegexParser.Tests.Performance;
using RegexParser.Tests.Transforms;
using RegexParser.Util;
using Utility.BaseTypes;
using Utility.ConsLists;
using Utility.Tests.BaseTypes;
using Msoft = System.Text.RegularExpressions;

namespace RegexParser.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MatcherPerformanceTests.VeryLongMatches();
                //BacktrackingPerformanceTests.BacktrackingTest();
                //RegexAssert.AreMatchesSameAsMsoft("", @"", AlgorithmType.Backtracking);

                //new AnchorMatcherTests(AlgorithmType.Backtracking).ContiguousMatch_2();
                //RegexAssert.AreMatchesSameAsMsoft("?Que?", @"\b.+?\b", AlgorithmType.Backtracking);

                //Console.WriteLine(formatMsoftMatches(Msoft.Regex.Matches("abc", @"^+abc")));
                //Console.WriteLine(formatMsoftMatches(Msoft.Regex.Matches("abc", @"(^\w)+")));
                //Console.WriteLine(formatMsoftMatches(Msoft.Regex.Matches("abc", @"(^)+")));
                //RegexAssert.AreMatchesSameAsMsoft("abc", @"(^\w)+", AlgorithmType.Backtracking);
                //RegexAssert.AreMatchesSameAsMsoft("abc", @"(^)+", AlgorithmType.Backtracking);

                //new CharEscapeMatcherTests(AlgorithmType.Backtracking).TwoMatches();

                //RegexAssert.DisplayASTTransform(@"(a|ab)bbbc", AlgorithmType.ExplicitDFA, RegexOptions.None);
                //RegexAssert.DisplayASTTransform(@"(a|ab[a-mx-z])+bbc", AlgorithmType.ExplicitDFA, RegexOptions.None);
                //RegexAssert.DisplayASTTransform(@"(ab[a-mx-z])*efg", AlgorithmType.ExplicitDFA, RegexOptions.None);
                //RegexAssert.DisplayASTTransform(@"(ab[a-z-[m-p]])*efg", AlgorithmType.ExplicitDFA, RegexOptions.None);
                //RegexAssert.DisplayASTTransform(@"(ab[cdemx-z])*efg", AlgorithmType.ExplicitDFA, RegexOptions.None);
                //RegexAssert.DisplayASTTransform(@"(a|a[a-z])+\wb*c(ab|cd)", AlgorithmType.ExplicitDFA, RegexOptions.None);

                //RegexAssert.AreMatchesSameAsMsoft("aaac", @"a*ac", AlgorithmType.Backtracking);
                //RegexAssert.AreMatchesSameAsMsoft("aaax", @"[ab]*\wx", AlgorithmType.Backtracking);
                //RegexAssert.AreMatchesSameAsMsoft("aaax", @"\S*\wx", AlgorithmType.Backtracking);

                //new CharClassMatcherTests(AlgorithmType.Backtracking).Subtract();

                //GeneralPerformanceTests.MatchCollections();
                //testBacktracking2();

                //RegexAssert.DisplayPattern(@"a|x[abc0-9]|c*");
                //RegexAssert.DisplayPattern(@"(a|b)x|c");
                //RegexAssert.DisplayPattern(@"abcd|defgh|012");
                //RegexAssert.DisplayPattern(@"a|x\w|c*");

                //RegexAssert.DisplayPattern(@"(ab[a-z-[m-p]])*efg");
                //RegexAssert.DisplayPattern(@".x[^abc0-9A-M]");
                //RegexAssert.DisplayPattern(@"^.x[abc0-9A-M]");
                //RegexAssert.DisplayPattern(@"abc++");

                //displayMatches("abbbc", @"(a|ab)bbbc", AlgorithmType.Backtracking, RegexOptions.Singleline | RegexOptions.Multiline);
                //RegexAssert.ThrowsSameExceptionAsMsoft("abab", @"(ab)++", AlgorithmType.Backtracking,
                //                                       RegexOptions.Singleline | RegexOptions.Multiline);

                //Console.WriteLine(formatMsoftMatches(Msoft.Regex.Matches("abbbc", @"(abb?b?b?c)|(bc)")));

                //Console.WriteLine(".$^{[(|)*+!?\\  - \b\n\b []09azAZ}".Distinct().OrderBy(c => c).AsString().Show());
                // "\b\n !$()*+-.09?AZ[\\]^az{|}"
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

        private static Result<char, TTree> runParser<TTree>(Parser<char, TTree> parser, string input)
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

        private static void displayResult<TTree>(Result<char, TTree> result, Func<TTree, string> toString)
        {
            if (result != null)
            {
                Console.WriteLine("Result: {0}", toString(result.Tree));
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
            displayMatches(input, pattern, algorithmType, RegexOptions.None);
        }

        private static void displayMatches(string input, string pattern, AlgorithmType algorithmType, RegexOptions options)
        {
            //RegexAssert.DisplayPattern(pattern);

            var matches = new Regex2(pattern, algorithmType, options).Matches(input);
            RegexAssert.DisplayMatches(input, pattern, algorithmType, options, matches);
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
