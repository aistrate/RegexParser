﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RegexParser.Tests.Helpers;
using RegexParser.Util;
using RegexParser.Matchers;

namespace RegexParser.Tests.Matchers
{
    [TestFixture(AlgorithmType.ImplicitDFA)]
    [TestFixture(AlgorithmType.Backtracking)]
    public class CharPatternMatcherTests : MatcherTests
    {
        public CharPatternMatcherTests(AlgorithmType algorithmType)
            : base(algorithmType) { }

        [Test]
        public void NoMatch()
        {
            Regex2 regex = new Regex2("xyz", AlgorithmType);
            Match2 match = regex.Match("Something or other");

            Assert.AreEqual(Match2.Empty, match);
        }

        [Test]
        public void OneMatch()    
        {
            Regex2 regex = new Regex2("thing", AlgorithmType);
            Match2 match = regex.Match("Something or other");

            Assert.AreEqual(Factory.CreateMatch(4, 5, "thing"), match);
        }

        [Test]
        public void TwoMatches()
        {
            Regex2 regex = new Regex2("thing", AlgorithmType);
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
            Match2[] matches = new Regex2("thing", AlgorithmType).Matches("Some thinthing or another").ToArray();
            Match2[] expected = new Match2[] {
                Factory.CreateMatch(9, 5, "thing")
            };

            CollectionAssert.AreEqual(expected, matches, "False overlap.");

            matches = new Regex2("alfa", AlgorithmType).Matches("This is alfalfa").ToArray();
            expected = new Match2[] {
                Factory.CreateMatch(8, 4, "alfa")
            };

            CollectionAssert.AreEqual(expected, matches, "Real overlap.");

            matches = new Regex2("alfa", AlgorithmType).Matches("This is alfalfalfa").ToArray();
            expected = new Match2[] {
                Factory.CreateMatch(8, 4, "alfa"),
                Factory.CreateMatch(14, 4, "alfa")
            };

            CollectionAssert.AreEqual(expected, matches, "Double overlap.");
        }

        [Test]
        public void CompareToMsoft()
        {
            RegexAssert.AreMatchesSameAsMsoft("Something or other", "xyz", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("Something or other", "thing", AlgorithmType);

            RegexAssert.IsFirstMatchSameAsMsoft("A thing or another thing", "thing", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("A thing or another thing", "thing", AlgorithmType);

            RegexAssert.AreMatchesSameAsMsoft("Some thinthing or another", "thing", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("This is alfalfa", "alfa", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("This is alfalfalfa", "alfa", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("This is alfalfalf", "alfa", AlgorithmType);
        }

        [Test]
        public void RepeatedChar()
        {
            RegexAssert.AreMatchesSameAsMsoft("Something or other", "o", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("Matter", "t", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("Mississippi", "s", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("CCCP", "CC", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("CCCCP", "CC", AlgorithmType);
        }

        [Test]
        public void FalseStart()
        {
            RegexAssert.AreMatchesSameAsMsoft("Something or other", "thingy", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("Something or other", "Somme", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("Something or other", "Some", AlgorithmType);
        }
    }
}