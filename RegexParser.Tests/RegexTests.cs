using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RegexParser.Util;
using RegexParser.Tests.Util;

namespace RegexParser.Tests
{
    [TestFixture]
    public class RegexTests
    {
        [Test]
        public void NoMatch()
        {
            Regex2 regex = new Regex2("xyz");
            Match2 match = regex.Match("Something or other");

            Assert.AreEqual(Match2.Empty, match);
        }

        [Test]
        public void CharPatternMatch()
        {
            Regex2 regex = new Regex2("thing");
            Match2 match = regex.Match("Something or other");

            Assert.AreEqual(Factory.CreateMatch(4, 5, "thing", null), match);
        }

        [Test]
        public void CharPatternMatches()
        {
            Regex2 regex = new Regex2("thing");
            Match2[] matches = regex.Matches("A thing or another thing.").ToArray();

            Match2[] expected = new Match2[] {
                Factory.CreateMatch(2, 5, "thing", null),
                Factory.CreateMatch(19, 5, "thing", null)
            };

            CollectionAssert.AreEqual(expected, matches, "MatchCollection");

            Assert.AreEqual(matches[1], matches[0].NextMatch(), "NextMatch.");
        }

        [Test]
        public void CharPatternMatches_Overlap()
        {
            Match2[] matches = Regex2.Matches("Some thinthing or another", "thing").ToArray();
            Match2[] expected = new Match2[] {
                Factory.CreateMatch(9, 5, "thing", null)
            };

            CollectionAssert.AreEqual(expected, matches, "False overlap.");

            matches = Regex2.Matches("This is alfalfa", "alfa").ToArray();
            expected = new Match2[] {
                Factory.CreateMatch(8, 4, "alfa", null)
            };

            CollectionAssert.AreEqual(expected, matches, "Real overlap.");

            matches = Regex2.Matches("This is alfalfalfa", "alfa").ToArray();
            expected = new Match2[] {
                Factory.CreateMatch(8, 4, "alfa", null),
                Factory.CreateMatch(14, 4, "alfa", null)
            };

            CollectionAssert.AreEqual(expected, matches, "Double overlap.");
        }
    }
}
