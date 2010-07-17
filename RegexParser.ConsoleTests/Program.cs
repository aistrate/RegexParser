using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using ParserCombinators.Tests;
using ParserCombinators.Tests.Performance;
using RegexParser.Matchers;
using RegexParser.Patterns;
using RegexParser.Tests;
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
                //displayMatches("Therefore they took CS101 and EE201.", @"\w\w\S\S\S");
                //Console.WriteLine(formatMsoftMatches(Msoft.Regex.Matches("x", "")));

                //EnumerablePerformanceTests.TestContainsCharBySize();
                //PatternPerformanceTests.CharClassPatternTest();
            }
            catch (Exception ex)
            {
                Console.WriteLine("MESSAGE:\n");
                Console.WriteLine(ex.Message);

                if (!(ex is AssertionException))
                    Console.WriteLine();

                Console.WriteLine("STACK TRACE:\n");
                Console.WriteLine(formatStackTrace(ex.StackTrace));
            }

            Console.WriteLine();
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

        private static void displayMatches(string input, string pattern)
        {
            MatchCollection2 matches = new Regex2(pattern, AlgorithmType.ImplicitDFA).Matches(input);

            Console.WriteLine("Match \"{0}\" against \"{1}\":\n", pattern, input);
            Console.WriteLine(string.Join("\n", matches.Select(m => m.ToString()).ToArray()));
            Console.WriteLine("\n");
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
            return string.Format("({0}, {1}, \"{2}\")", match.Index, match.Length, match.Value);
        }
    }
}
