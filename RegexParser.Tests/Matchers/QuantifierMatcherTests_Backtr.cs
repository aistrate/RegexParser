using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Tests.Asserts;

namespace RegexParser.Tests.Matchers
{
#if TEST_BACKTRACKING
    [TestFixture(AlgorithmType.Backtracking)]
#endif

#if TEST_BACKTRACKING

    public class QuantifierMatcherTests_Backtr : AlgorithmTests
    {
        public QuantifierMatcherTests_Backtr(AlgorithmType algorithmType)
            : base(algorithmType) { }

        [Test]
        public void MustBacktrack()
        {
            RegexAssert.AreMatchesSameAsMsoft("aaac", @"a*ac", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("aaax", @"[ab]*\wx", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("aaax", @"\S*\wx", AlgorithmType);
        }
    }

#endif
}
