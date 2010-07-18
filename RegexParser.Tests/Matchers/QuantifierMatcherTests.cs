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
    public class QuantifierMatcherTests : MatcherTests
    {
        public QuantifierMatcherTests(AlgorithmType algorithmType)
            : base(algorithmType) { }

        [Test]
        public void Star()
        {
            string input = "A thing or another thing";

            string[] patterns = new[] {
                @"\w*",
                @"\s*"
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }
    }
}
