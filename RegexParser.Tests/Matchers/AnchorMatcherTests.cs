using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Tests.Asserts;

namespace RegexParser.Tests.Matchers
{
#if TEST_BACKTRACKING
    [TestFixture(AlgorithmType.Backtracking)]
#endif

#if TEST_BACKTRACKING

    public class AnchorMatcherTests : AlgorithmTests
    {
        public AnchorMatcherTests(AlgorithmType algorithmType)
            : base(algorithmType) { }

        [Test]
        public void StartAndEndOfStringOrLine()
        {
            string input = "One thing or another\nAnd then some";

            string[] patterns = new[] {
                @"^\w+",
                @"\w+$",

                @"^\w+|\w+$",
                @"\w+$|^\w+",

                @"\w*^\w*",
                @"\w*$\w*",
                @"\w+$\w*",

                @"\A\w+",
                @"\w+\Z",
                @"\w+\z",

                @"\A\w+|\w+\Z",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType, RegexOptions.Multiline);
        }

        [Test]
        public void StartAndEndOfStringOrLine_Newlines()
        {
            string input = "AB\nCD\r\nEF\n\rGH\rIJ\n\nKL\r\rMN\n\r\nOP\r\n\rQR\n";

            string[] patterns = new[] {
                @"^\w+",
                @"^.+",

                @"\w+$",
                @"\w+\r?$",
                @".+$",

                @"\A\w+",
                @"\w+\Z",
                @"\w+\z",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType, RegexOptions.Multiline);
        }

        [Test]
        public void EscapedChars()
        {
            string input = @"abc^d ef gh i$ jk    \A \z\Z \b\B \G   \g \a\n";

            string[] patterns = new[] {
                @"\^",
                @"\$",

                @"\\A",
                @"\\a",
                @"\\Z",
                @"\\z",
                @"\\B",
                @"\\b",
                @"\\G",
                @"\\g",
                @"\\N",
                @"\\n",
                @"\n",

                @"\\",
                @"\\\w",
                @"\\.",
                @"\\[a-z]",
                @"\\[A-Z]",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }
    }

#endif
}
