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

        public static void IsFirstMatchSameAsMsoft(string input, string patternText, AlgorithmType algorithmType)
        {
            IsFirstMatchSameAsMsoft(input, patternText, algorithmType, null);
        }

        public static void IsFirstMatchSameAsMsoft(string input, string patternText, AlgorithmType algorithmType, string message)
        {
            Match2 actual = new Regex2(patternText, algorithmType).Match(input);
            Match2 expected = createMatch(Msoft.Regex.Match(input, patternText));

            try
            {
                Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
                throw new AssertionException(formatException(message, input, patternText, ex));
            }
        }

        public static void AreMatchesSameAsMsoft(string input, string patternText, AlgorithmType algorithmType)
        {
            AreMatchesSameAsMsoft(input, patternText, algorithmType, null);
        }

        public static void AreMatchesSameAsMsoft(string input, string patternText, AlgorithmType algorithmType, string message)
        {
            Match2[] actual = new Regex2(patternText, algorithmType).Matches(input).ToArray();
            Match2[] expected = Msoft.Regex.Matches(input, patternText)
                                           .Cast<Msoft.Match>()
                                           .Select(m => createMatch(m))
                                           .ToArray();

            try
            {
                CollectionAssert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
                throw new AssertionException(formatException(message, input, patternText, ex));
            }
        }

        private static Match2 createMatch(Msoft.Match msoftMatch)
        {
            if (msoftMatch.Success)
                return Factory.CreateMatch(msoftMatch.Index, msoftMatch.Length, msoftMatch.Value);
            else
                return Match2.Empty;
        }

        private static string formatException(string message, string regexInputText, string regexPatternText, Exception ex)
        {
            const string indent = "  ";

            if (string.IsNullOrEmpty(message))
                message = "";
            else
                message += "\n";

            message += string.Format("Compare with .NET Regex: Input=\"{0}\", Pattern=\"{1}\"\n", regexInputText, regexPatternText) +
                       ex.Message;
            message = indent + message.Replace("\n", "\n" + indent);

            return message;
        }
    }
}
