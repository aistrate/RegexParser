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

        [Test]
        public void IgnoreCase()
        {
            string patternText = @"aA7 :.[a-m]\w\D\s.+";

            BasePattern expected = new GroupPattern(
                                        true,
                                        new CaseInsensitiveCharPattern(new CharEscapePattern('a')),
                                        new CaseInsensitiveCharPattern(new CharEscapePattern('A')),
                                        new CharEscapePattern('7'),
                                        new CharEscapePattern(' '),
                                        new CharEscapePattern(':'),
                                        new AnyCharPattern(false),
                                        new CaseInsensitiveCharPattern(
                                            new CharGroupPattern(true, new [] { new CharRangePattern('a', 'm') })),
                                        CharGroupPattern.WordChar,
                                        CharGroupPattern.DigitChar.Negated,
                                        CharGroupPattern.WhitespaceChar,
                                        new QuantifierPattern(
                                            new AnyCharPattern(false),
                                            1, null, true));

            RegexAssert.IsASTTransformCorrect(expected, patternText,
                                              new RegexOptionsASTTransform(RegexOptions.IgnoreCase));
        }

        [Test]
        public void Multiline_False()
        {
            string patternText = @"^\b\B\z\Z\A\G$";

            BasePattern expected = new GroupPattern(
                                        true,
                                        new AnchorPattern(AnchorType.StartOfString),
                                        new AnchorPattern(AnchorType.WordBoundary),
                                        new AnchorPattern(AnchorType.NonWordBoundary),
                                        new AnchorPattern(AnchorType.EndOfString),
                                        new AnchorPattern(AnchorType.EndOfStringOrBeforeEndingNewline),
                                        new AnchorPattern(AnchorType.StartOfString),
                                        new AnchorPattern(AnchorType.ContiguousMatch),
                                        new AnchorPattern(AnchorType.EndOfStringOrBeforeEndingNewline));

            RegexAssert.IsASTTransformCorrect(expected, patternText,
                                              new RegexOptionsASTTransform(RegexOptions.None));
        }

        [Test]
        public void Multiline_True()
        {
            string patternText = @"^\b\B\z\Z\A\G$";

            BasePattern expected = new GroupPattern(
                                        true,
                                        new AnchorPattern(AnchorType.StartOfLine),
                                        new AnchorPattern(AnchorType.WordBoundary),
                                        new AnchorPattern(AnchorType.NonWordBoundary),
                                        new AnchorPattern(AnchorType.EndOfString),
                                        new AnchorPattern(AnchorType.EndOfStringOrBeforeEndingNewline),
                                        new AnchorPattern(AnchorType.StartOfString),
                                        new AnchorPattern(AnchorType.ContiguousMatch),
                                        new AnchorPattern(AnchorType.EndOfLine));

            RegexAssert.IsASTTransformCorrect(expected, patternText,
                                              new RegexOptionsASTTransform(RegexOptions.Multiline));
        }
    }
}
