using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Patterns;
using RegexParser.Tests.Helpers;

namespace RegexParser.Tests.Transforms
{
    //[TestFixture(AlgorithmType.ExplicitDFA)]
    [TestFixture(AlgorithmType.Backtracking)]
    public class StringAstTransformTests : AlgorithmTests
    {
        public StringAstTransformTests(AlgorithmType algorithmType)
            : base(algorithmType) { }

        [Test]
        public void OneChar()
        {
            BasePattern actual = getTransformedPattern("x");
            BasePattern expected = new GroupPattern(new BasePattern[]
                                   {
                                       new CharEscapePattern('x'),
                                   });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TwoChars()
        {
            BasePattern actual = getTransformedPattern("cd");
            BasePattern expected = new GroupPattern(new BasePattern[]
                                   {
                                       new StringPattern("cd"),
                                   });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ManyChars()
        {
            BasePattern actual = getTransformedPattern(@"A longer string\.");
            BasePattern expected = new GroupPattern(new BasePattern[]
                                   {
                                       new StringPattern("A longer string."),
                                   });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ManyCharsWithClasses()
        {
            BasePattern actual = getTransformedPattern(@"ab[cd][efg]hijk\dm");
            BasePattern expected = new GroupPattern(new BasePattern[]
                                   {
                                       new StringPattern("ab"),
                                       new CharGroupPattern(true, "cd"),
                                       new CharGroupPattern(true, "efg"),
                                       new StringPattern("hijk"),
                                       CharGroupPattern.DigitChar,
                                       new CharEscapePattern('m'),
                                   });
            Assert.AreEqual(expected, actual);
        }

        private BasePattern getTransformedPattern(string patternText)
        {
            RegexAssert.DisplayAstTransform(patternText, AlgorithmType);

            return BaseMatcher.CreateMatcher(AlgorithmType, patternText).Pattern;
        }
    }
}
