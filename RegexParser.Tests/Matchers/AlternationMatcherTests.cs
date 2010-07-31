using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Tests.Helpers;

namespace RegexParser.Tests.Matchers
{
    //[TestFixture(AlgorithmType.ImplicitDFA)]
    [TestFixture(AlgorithmType.Backtracking)]
    public class AlternationMatcherTests : MatcherTests
    {
        public AlternationMatcherTests(AlgorithmType algorithmType)
            : base(algorithmType) { }

        [Test]
        public void Simple()
        {
            RegexAssert.AreMatchesSameAsMsoft("abbbc", @"(a|ab)bbbc", AlgorithmType);
            //RegexAssert.AreMatchesSameAsMsoft("abbbbc", @"(a|ab)bbbc", AlgorithmType);

            //RegexAssert.AreMatchesSameAsMsoft("abbbc", @"(ab|a)bbbc", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbbbc", @"(ab|a)bbbc", AlgorithmType);
        }
    }
}
