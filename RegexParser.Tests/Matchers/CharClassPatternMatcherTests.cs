using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Tests.Helpers;

namespace RegexParser.Tests.Matchers
{
    [TestFixture(AlgorithmType.ImplicitDFA)]
    public class CharClassPatternMatcherTests : MatcherTests
    {
        public CharClassPatternMatcherTests(AlgorithmType algorithmType)
            : base(algorithmType) { }

        [Test]
        public void Ranges()
        {
            string input = "A thing or another thing";

            string[] patterns = new[] {
                "[a-m]",
                "[a-ae-ei-io-ou-u]",
                "[A-Z] [a-z]"
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void CharSetsAndRanges()
        {
            string input = "A thing or another thing";

            string[] patterns = new[] {
                "[aeiou]",
                "[a-fdmzA-D]"
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void NegativeRanges()
        {
            string input = "A thing or another thing";

            RegexAssert.AreMatchesSameAsMsoft(input, "[^A-Z ]", AlgorithmType);
        }

        [Test]
        public void BackslashClasses()
        {
            string input = "Therefore they took CS101 and EE201.";

            string[] patterns = new[] {
                @"[A-Z][A-Z][0-9][0-9][0-9]",
                @"[A-Z][A-Z]\d\d\d",
                @"\w\w\d\d\d",
                @"\D\D\d\d\d",
                @"..\d\d\d",
                @"\s..\d\d\d",
                @"\w\w\d\d\d\s",
                @"\w\w\d\d\d\S",
                @"\w\w\d\d\d.",
                @"\d\d\d\d\d",
                @"..\d..",
                @".\d.",
                @"\d.",
                @"\w\w\S\S\S"
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void WhitespaceCharacters()
        {
            string input = "ID\tName\tAddress \t\t\tPhone number\n" +
                           "55\tJohn\tOaks Avenue 5\t065656565\n";

            string[] patterns = new[] {
                @"\s",
                @"\s\s",
                @"\s\s\s",
                @"\s\s\s\s",
                @"\s\w",
                @"\w\s"
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }
    }
}
