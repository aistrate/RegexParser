using NUnit.Framework;
using RegexParser.Patterns;
using RegexParser.Tests.Asserts;
using RegexParser.Transforms;

namespace RegexParser.Tests.Transforms
{
    [TestFixture]
    public class QuantifierASTTransformTests
    {
        private BaseASTTransform transform = new QuantifierASTTransform();

        [Test]
        public void Quantifier_N_M()
        {
            string patternText = "a(bc){2,7}";

            BasePattern innerGroup = new GroupPattern(
                                            true,
                                            new CharEscapePattern('b'),
                                            new CharEscapePattern('c'));

            BasePattern expected = new GroupPattern(
                                            true,
                                            new CharEscapePattern('a'),
                                            new GroupPattern(
                                                    false,
                                                    new QuantifierPattern(innerGroup, 2, 2, true),
                                                    new QuantifierPattern(innerGroup, 0, 5, true)));

            RegexAssert.IsASTTransformCorrect(expected, patternText, transform);
        }

        [Test]
        public void Quantifier_N_Infinity()
        {
            string patternText = "a(bc){3,}";

            BasePattern innerGroup = new GroupPattern(
                                            true,
                                            new CharEscapePattern('b'),
                                            new CharEscapePattern('c'));

            BasePattern expected = new GroupPattern(
                                            true,
                                            new CharEscapePattern('a'),
                                            new GroupPattern(
                                                    false,
                                                    new QuantifierPattern(innerGroup, 3, 3, true),
                                                    new QuantifierPattern(innerGroup, 0, null, true)));

            RegexAssert.IsASTTransformCorrect(expected, patternText, transform);
        }

        [Test]
        public void Quantifier_Plus()
        {
            string patternText = "a(bc)+";

            BasePattern innerGroup = new GroupPattern(
                                            true,
                                            new CharEscapePattern('b'),
                                            new CharEscapePattern('c'));

            BasePattern expected = new GroupPattern(
                                            true,
                                            new CharEscapePattern('a'),
                                            new GroupPattern(
                                                    false,
                                                    innerGroup,
                                                    new QuantifierPattern(innerGroup, 0, null, true)));

            RegexAssert.IsASTTransformCorrect(expected, patternText, transform);
        }

        [Test]
        public void Quantifier_0_Infinity()
        {
            string patternText = "a(bc)*";

            BasePattern innerGroup = new GroupPattern(
                                            true,
                                            new CharEscapePattern('b'),
                                            new CharEscapePattern('c'));

            BasePattern expected = new GroupPattern(
                                            true,
                                            new CharEscapePattern('a'),
                                            new QuantifierPattern(innerGroup, 0, null, true));

            RegexAssert.IsASTTransformCorrect(expected, patternText, transform);
        }

        [Test]
        public void Quantifier_N()
        {
            string patternText = "a(bc){4}";

            BasePattern innerGroup = new GroupPattern(
                                            true,
                                            new CharEscapePattern('b'),
                                            new CharEscapePattern('c'));

            BasePattern expected = new GroupPattern(
                                            true,
                                            new CharEscapePattern('a'),
                                            new QuantifierPattern(innerGroup, 4, 4, true));

            RegexAssert.IsASTTransformCorrect(expected, patternText, transform);
        }

        [Test]
        public void Quantifier_1()
        {
            string patternText = "a(bc){1}";

            BasePattern innerGroup = new GroupPattern(
                                            true,
                                            new CharEscapePattern('b'),
                                            new CharEscapePattern('c'));

            BasePattern expected = new GroupPattern(
                                            true,
                                            new CharEscapePattern('a'),
                                            innerGroup);

            RegexAssert.IsASTTransformCorrect(expected, patternText, transform);
        }

        [Test]
        public void Quantifier_Zero()
        {
            string patternText = "a(bc){0}";

            BasePattern expected = new GroupPattern(
                                            true,
                                            new CharEscapePattern('a'));

            RegexAssert.IsASTTransformCorrect(expected, patternText, transform);
        }
    }
}
