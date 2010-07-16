using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RegexParser.Util;
using Msoft = System.Text.RegularExpressions;
using RegexParser.Matchers;

namespace RegexParser.Tests.Helpers
{
    public static class RegexAssert
    {
        // TODO: add new method: ThrowsSameExceptionAsMsoft

        public static void AreMatchesSameAsMsoft(string input, string pattern, AlgorithmType algorithmType)
        {
            AreMatchesSameAsMsoft(input, pattern, algorithmType, null);
        }

        public static void AreMatchesSameAsMsoft(string input, string pattern, AlgorithmType algorithmType, string message)
        {
            Match2[] actual = new Regex2(pattern, algorithmType).Matches(input).ToArray();
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

            message += string.Format("Comparing with .NET Regex: Input=\"{0}\", Pattern=\"{1}\"\n", input, pattern) +
                       ex.Message;
            message = indent + message.Replace("\n", "\n" + indent);

            return message;
        }
    }
}
