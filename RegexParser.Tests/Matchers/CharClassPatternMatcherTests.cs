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

            RegexAssert.AreMatchesSameAsMsoft(input, "[^A-Z]", AlgorithmType);
        }
    }
}
