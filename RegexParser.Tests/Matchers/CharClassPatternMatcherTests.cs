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
        }
    }
}
