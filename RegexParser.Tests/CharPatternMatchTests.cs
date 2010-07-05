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
    public class CharPatternMatchTests
    {
        [Test]
        public void NoMatch()
        {
            Regex2 regex = new Regex2("xyz");
            Match2 match = regex.Match("Something or other");

            Assert.AreEqual(Match2.Empty, match);
        }

        [Test]
        public void OneMatch()
        {
            Regex2 regex = new Regex2("thing");
            Match2 match = regex.Match("Something or other");

            Assert.AreEqual(Factory.CreateMatch(4, 5, "thing"), match);
        }

        [Test]
        public void TwoMatches()
        {
            Regex2 regex = new Regex2("thing");
            Match2[] matches = regex.Matches("A thing or another thing").ToArray();

            Match2[] expected = new Match2[] {
                Factory.CreateMatch(2, 5, "thing"),
                Factory.CreateMatch(19, 5, "thing")
            };

            CollectionAssert.AreEqual(expected, matches, "MatchCollection");

            Assert.AreEqual(matches[1], matches[0].NextMatch(), "NextMatch/1.");
            Assert.AreEqual(Match2.Empty, matches[1].NextMatch(), "NextMatch/2.");
            Assert.AreEqual(Match2.Empty, matches[1].NextMatch().NextMatch(), "NextMatch/3.");
        }

        [Test]
        public void MatchOverlap()
        {
            Match2[] matches = Regex2.Matches("Some thinthing or another", "thing").ToArray();
            Match2[] expected = new Match2[] {
                Factory.CreateMatch(9, 5, "thing")
            };

            CollectionAssert.AreEqual(expected, matches, "False overlap.");

            matches = Regex2.Matches("This is alfalfa", "alfa").ToArray();
            expected = new Match2[] {
                Factory.CreateMatch(8, 4, "alfa")
            };

            CollectionAssert.AreEqual(expected, matches, "Real overlap.");

            matches = Regex2.Matches("This is alfalfalfa", "alfa").ToArray();
            expected = new Match2[] {
                Factory.CreateMatch(8, 4, "alfa"),
                Factory.CreateMatch(14, 4, "alfa")
            };

            CollectionAssert.AreEqual(expected, matches, "Double overlap.");
        }

        [Test]
        public void CompareToMsoft()
        {
            RegexAssert.AreMatchesSameAsMsoft("Something or other", "xyz");
            RegexAssert.AreMatchesSameAsMsoft("Something or other", "thing");

            RegexAssert.IsFirstMatchSameAsMsoft("A thing or another thing", "thing");
            RegexAssert.AreMatchesSameAsMsoft("A thing or another thing", "thing");

            RegexAssert.AreMatchesSameAsMsoft("Some thinthing or another", "thing");
            RegexAssert.AreMatchesSameAsMsoft("This is alfalfa", "alfa");
            RegexAssert.AreMatchesSameAsMsoft("This is alfalfalfa", "alfa");
            RegexAssert.AreMatchesSameAsMsoft("This is alfalfalf", "alfa");
        }
    }
}
