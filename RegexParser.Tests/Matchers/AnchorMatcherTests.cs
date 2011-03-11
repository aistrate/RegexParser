using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Tests.Asserts;

namespace RegexParser.Tests.Matchers
{
#if TEST_BACKTRACKING
    //[TestFixture(AlgorithmType.Backtracking)]
#endif

#if TEST_BACKTRACKING

    public class AnchorMatcherTests : AlgorithmTests
    {
        public AnchorMatcherTests(AlgorithmType algorithmType)
            : base(algorithmType) { }

        //[Test]
        public void StartAndEndOfString()
        {
            string input = "One thing or another\nAnd then some";

            string[] patterns = new[] {
                @"^\w+",
                @"\w+$",
                @"^\w+|\w+$",
                @"(^\w+)|(\w+$)",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType, RegexOptions.Multiline);
        }
    }

#endif
}
