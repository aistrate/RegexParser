using NUnit.Framework;
using RegexParser.Patterns;
using RegexParser.Tests.Asserts;
using RegexParser.Transforms;

namespace RegexParser.Tests.Transforms
{
    [TestFixture]
    public class RegexOptionsASTTransformTests
    {
        [Test]
        public void Singleline_False()
        {
            string patternText = ".+";

            BasePattern expected = new GroupPattern(
                                        true,
                                        new QuantifierPattern(
                                            new AnyCharPattern(false),
                                            1, null, true));

            RegexAssert.IsASTTransformCorrect(expected, patternText,
                                              new RegexOptionsASTTransform(RegexOptions.None));
        }

        [Test]
        public void Singleline_True()
        {
            string patternText = ".+";

            BasePattern expected = new GroupPattern(
                                        true,
                                        new QuantifierPattern(
                                            new AnyCharPattern(true),
                                            1, null, true));

            RegexAssert.IsASTTransformCorrect(expected, patternText,
                                              new RegexOptionsASTTransform(RegexOptions.Singleline));
        }
    }
}
