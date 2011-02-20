using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Tests.Helpers;

namespace RegexParser.Tests.Matchers
{
    [TestFixture(AlgorithmType.Backtracking)]
    public class AlternationMatcherTests_Backtr : AlgorithmTests
    {
        public AlternationMatcherTests_Backtr(AlgorithmType algorithmType)
            : base(algorithmType) { }

        [Test]
        public void MustBacktrack()
        {
            RegexAssert.AreMatchesSameAsMsoft("abbbbc", @"(a|ab)bbbc", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbbc", @"(ab|a)bbbc", AlgorithmType);

            RegexAssert.AreMatchesSameAsMsoft("abbbbc", @"(|ab)bbbc", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbbbc", @"(||ab)bbbc", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbbbc", @"(||)bbbc", AlgorithmType);

            RegexAssert.AreMatchesSameAsMsoft("abbbc", @"(|a)bbbc", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbbc", @"(b*|a)bbbc", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbbc", @"(ab+|a)bbbc", AlgorithmType);
        }
    }
}
