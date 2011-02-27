﻿using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Tests.Asserts;

namespace RegexParser.Tests.Matchers
{
#if TEST_EXPLICITDFA
    [TestFixture(AlgorithmType.ExplicitDFA)]
#endif
#if TEST_BACKTRACKING
    [TestFixture(AlgorithmType.Backtracking)]
#endif

#if TEST_EXPLICITDFA || TEST_BACKTRACKING

    public class RegexOptionsMatcherTests : AlgorithmTests
    {
        public RegexOptionsMatcherTests(AlgorithmType algorithmType)
            : base(algorithmType) { }

        [Test]
        public void Singleline()
        {
            string input = "First line\nSecond line.\nAnd a third\nThe last";

            string[] patterns = new[] {
                @".",
                @"\.",
                @".+",
                @"line.",
                @"line\.",
                //@"\w+.",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType, RegexOptions.Singleline);
        }
    }

#endif
}
