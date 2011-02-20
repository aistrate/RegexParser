using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Tests.Helpers;

namespace RegexParser.Tests.Matchers
{
    [TestFixture(AlgorithmType.Backtracking)]
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
}
