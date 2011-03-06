using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RegexParser.Patterns;

namespace RegexParser.Tests.Patterns
{
    [TestFixture]
    public class QuantifierPatternTests
    {
        [Test]
        public void Quantifiers()
        {
            BasePattern actual = BasePattern.CreatePattern(@"\d*\s?\W+x*?\n??y+?");

            BasePattern expected = new GroupPattern(
                                        true,
                                        new BasePattern[]
                                        {
                                            new QuantifierPattern(CharGroupPattern.DigitChar, 0, null, true),
                                            new QuantifierPattern(CharGroupPattern.WhitespaceChar, 0, 1, true),
                                            new QuantifierPattern(CharGroupPattern.WordChar.Negated, 1, null, true),

                                            new QuantifierPattern(new CharEscapePattern('x'), 0, null, false),
                                            new QuantifierPattern(new CharEscapePattern('\n'), 0, 1, false),
                                            new QuantifierPattern(new CharEscapePattern('y'), 1, null, false)
                                        });

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Doubled()
        {
            BasePattern actual = BasePattern.CreatePattern(@"((ab)+)+");

            BasePattern expected = new GroupPattern(true,
                                       new QuantifierPattern(
                                           new GroupPattern(true,
                                               new QuantifierPattern(
                                                   new GroupPattern(true,
                                                       new CharEscapePattern('a'),
                                                       new CharEscapePattern('b')),
                                                   1, null, true)),
                                           1, null, true));

            Assert.AreEqual(expected, actual, "With parentheses");
        }
    }
}
