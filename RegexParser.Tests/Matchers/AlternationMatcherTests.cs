using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Tests.Helpers;

namespace RegexParser.Tests.Matchers
{
    //[TestFixture(AlgorithmType.ExplicitDFA)]
    [TestFixture(AlgorithmType.Backtracking)]
    public class AlternationMatcherTests : AlgorithmTests
    {
        public AlternationMatcherTests(AlgorithmType algorithmType)
            : base(algorithmType) { }

        [Test]
        public void Simple()
        {
            RegexAssert.AreMatchesSameAsMsoft("abbbc", @"(a|ab)bbbc", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbbbc", @"(ab|a)bbbc", AlgorithmType);
        }

        [Test]
        public void MustBacktrack()
        {
            RegexAssert.AreMatchesSameAsMsoft("abbbbc", @"(a|ab)bbbc", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbbc", @"(ab|a)bbbc", AlgorithmType);
        }

        [Test]
        public void FirstDoesNotMatch()
        {
            RegexAssert.AreMatchesSameAsMsoft("abbbbc", @"(c|ba|ab)bbbc", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbbbc", @"(aa|ab)bbbc", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbbbc", @"(ba|aa|ab|a)bbbc", AlgorithmType);
        }

        [Test]
        public void NoParens()
        {
            RegexAssert.AreMatchesSameAsMsoft("abc", @"abc|def", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("def", @"abc|def", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abcdef", @"abc|def", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abc def", @"abc|def", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abcadef", @"abc|def", AlgorithmType);
        }
    }
}
