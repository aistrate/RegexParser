using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ParserCombinators.Util;
using RegexParser.Matchers;
using RegexParser.Util;
using Msoft = System.Text.RegularExpressions;

namespace RegexParser.Tests.Helpers
{
    public static class RegexAssert
    {
        public static void AreMatchesSameAsMsoft(string input, string pattern, AlgorithmType algorithmType)
        {
            AreMatchesSameAsMsoft(input, pattern, algorithmType, null);
        }

        public static void AreMatchesSameAsMsoft(string input, string pattern, AlgorithmType algorithmType, string message)
        {
            Match2[] actual = new Regex2(pattern, algorithmType).Matches(input).ToArray();

            DisplayMatches(input, pattern, algorithmType, actual);

            Match2[] expected = Msoft.Regex.Matches(input, pattern)
                                           .Cast<Msoft.Match>()
                                           .Select(m => createMatch(m))
                                           .ToArray();

            try
            {
                CollectionAssert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
                throw new AssertionException(formatException(message, input, pattern, ex));
            }
        }

        public static void AreMatchesSameAsMsoft(string input, string[] patterns, AlgorithmType algorithmType)
        {
            foreach (string pattern in patterns)
                AreMatchesSameAsMsoft(input, pattern, algorithmType);
        }

        public static void DisplayMatches(string input, string pattern, AlgorithmType algorithmType, IEnumerable<Match2> matches)
        {
            int count = matches.Count();

            Console.WriteLine("Input: {0}", input.Show());
            Console.WriteLine("Pattern: {0}", pattern.ShowVerbatim());

            Console.WriteLine("{0} match{1} ({2}){3}",
                              count,
                              count == 1 ? "" : "es",
                              algorithmType.ToString(),
                              count > 0 ? ":" : ".\n");

            if (count > 0)
                Console.WriteLine(string.Join("\n", matches.Select(m => string.Format("{0,4:#0},{1,3:#0},  {2}",
                                                                                      m.Index, m.Length, m.Value.Show()))
                                                           .ToArray()) + "\n");
        }

        private static Match2 createMatch(Msoft.Match msoftMatch)
        {
            if (msoftMatch.Success)
                return Factory.CreateMatch(msoftMatch.Index, msoftMatch.Length, msoftMatch.Value);
            else
                return Match2.Empty;
        }

        private static string formatException(string message, string input, string pattern, Exception ex)
        {
            const string indent = "  ";

            if (string.IsNullOrEmpty(message))
                message = "";
            else
                message += "\n";

            message += string.Format("Comparing with .NET Regex: Input={0}, Pattern={1}\n", input.Show(), pattern.ShowVerbatim()) +
                       ex.Message;
            message = indent + message.Replace("\n", "\n" + indent);

            return message;
        }
    }
}
