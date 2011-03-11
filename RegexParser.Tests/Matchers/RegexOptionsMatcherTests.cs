using NUnit.Framework;
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

        [Test]
        public void Singleline_Newlines()
        {
            string input = "AB\nCD\r\nEF\n\rGH\rIJ\n\nKL\r\rMN\n\r\nOP\r\n\rQR\n";

            string[] patterns = new[] {
                @".",
                @".+",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType, RegexOptions.Singleline);
        }

        [Test]
        public void IgnoreCase()
        {
            string input = "First line\nSecond line.\nAnd a third\nThe last";

            string[] patterns = new[] {
                @"first",
                @"SECOND",
                @"[A-Z]+",
                @"[a-z]+",
                @"s",
                @"s\w*",
                @"S.",
                @"E.",
                @"D.",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType, RegexOptions.IgnoreCase);
        }
    }

#endif
}
