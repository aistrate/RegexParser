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

            RegexAssert.AreMatchesSameAsMsoft(input, "[a-m]", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, "[a-ae-ei-io-ou-u]", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, "[A-Z] [a-z]", AlgorithmType);
        }

        [Test]
        public void CharSetsAndRanges()
        {
            string input = "A thing or another thing";

            RegexAssert.AreMatchesSameAsMsoft(input, "[aeiou]", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, "[a-fdmzA-D]", AlgorithmType);
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

            RegexAssert.AreMatchesSameAsMsoft(input, @"[A-Z][A-Z][0-9][0-9][0-9]", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, @"[A-Z][A-Z]\d\d\d", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, @"\w\w\d\d\d", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, @"\D\D\d\d\d", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, @"..\d\d\d", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, @"\s..\d\d\d", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, @"\w\w\d\d\d\s", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, @"\w\w\d\d\d\S", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, @"\w\w\d\d\d.", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, @"\d\d\d\d\d", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, @"..\d..", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, @".\d.", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, @"\d.", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, @"\w\w\S\S\S", AlgorithmType);

        }

        [Test]
        public void WhitespaceCharacters()
        {
            string input = "ID\tName\tAddress \t\t\tPhone number\n" +
                           "55\tJohn\tOaks Avenue 5\t065656565\n";

            RegexAssert.AreMatchesSameAsMsoft(input, @"\s", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, @"\s\s", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, @"\s\s\s", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, @"\s\s\s\s", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, @"\s\w", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, @"\w\s", AlgorithmType);
        }
    }
}
