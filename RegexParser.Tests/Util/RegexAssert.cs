using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RegexParser.Util;
using Msoft = System.Text.RegularExpressions;

namespace RegexParser.Tests.Util
{
    public static class RegexAssert
    {
        public static void IsFirstMatchSameAsMsoft(string input, string patternText)
        {
            IsFirstMatchSameAsMsoft(input, patternText, null);
        }

        public static void IsFirstMatchSameAsMsoft(string input, string patternText, string message)
        {
            Match2 actual = Regex2.Match(input, patternText);
            Match2 expected = createMatch(Msoft.Regex.Match(input, patternText));

            Assert.AreEqual(expected, actual, message);
        }

        public static void AreMatchesSameAsMsoft(string input, string patternText)
        {
            AreMatchesSameAsMsoft(input, patternText, null);
        }

        public static void AreMatchesSameAsMsoft(string input, string patternText, string message)
        {
            Match2[] actual = Regex2.Matches(input, patternText).ToArray();
            Match2[] expected = Msoft.Regex.Matches(input, patternText)
                                           .Cast<Msoft.Match>()
                                           .Select(m => createMatch(m))
                                           .ToArray();

            CollectionAssert.AreEqual(expected, actual, message);
        }

        private static Match2 createMatch(Msoft.Match msoftMatch)
        {
            if (msoftMatch.Success)
                return Factory.CreateMatch(msoftMatch.Index, msoftMatch.Length, msoftMatch.Value, null);
            else
                return Match2.Empty;
        }
    }
}
