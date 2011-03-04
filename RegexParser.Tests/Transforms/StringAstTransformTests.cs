using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Patterns;
using RegexParser.Tests.Asserts;
using RegexParser.Transforms;

namespace RegexParser.Tests.Transforms
{
    [TestFixture]
    public class StringASTTransformTests
    {
        private BaseASTTransform transform = new StringASTTransform();

        [Test]
        public void OneChar()
        {
            string patternText = "x";

            BasePattern expected = new GroupPattern(new BasePattern[]
                                   {
                                       new CharEscapePattern('x'),
                                   });

            RegexAssert.IsASTTransformCorrect(expected, patternText, transform);
        }

        [Test]
        public void TwoChars()
        {
            string patternText = "cd";

            BasePattern expected = new GroupPattern(new BasePattern[]
                                   {
                                       new StringPattern("cd"),
                                   });

            RegexAssert.IsASTTransformCorrect(expected, patternText, transform);
        }

        [Test]
        public void ManyChars()
        {
            string patternText = @"A longer string\.";

            BasePattern expected = new GroupPattern(new BasePattern[]
                                   {
                                       new StringPattern("A longer string."),
                                   });

            RegexAssert.IsASTTransformCorrect(expected, patternText, transform);
        }

        [Test]
        public void ManyCharsWithClasses()
        {
            string patternText = @"ab[cd][efg]hijk\dm";

            BasePattern expected = new GroupPattern(new BasePattern[]
                                   {
                                       new StringPattern("ab"),
                                       new CharGroupPattern(true, "cd"),
                                       new CharGroupPattern(true, "efg"),
                                       new StringPattern("hijk"),
                                       CharGroupPattern.DigitChar,
                                       new CharEscapePattern('m'),
                                   });

            RegexAssert.IsASTTransformCorrect(expected, patternText, transform);
        }
    }
}
