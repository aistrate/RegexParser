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

            BasePattern expected = new GroupPattern(new BasePattern[]
                                   {
                                       new QuantifierPattern(CharClassPattern.DigitChar, 0, null, true),
                                       new QuantifierPattern(CharClassPattern.WhitespaceChar, 0, 1, true),
                                       new QuantifierPattern(CharClassPattern.WordChar.Negated, 1, null, true),

                                       new QuantifierPattern(new CharPattern('x'), 0, null, false),
                                       new QuantifierPattern(new CharPattern('\n'), 0, 1, false),
                                       new QuantifierPattern(new CharPattern('y'), 1, null, false)
                                   });

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Doubled()
        {
            BasePattern actual = BasePattern.CreatePattern(@"((ab)+)+");

            BasePattern expected = new GroupPattern(
                                       new QuantifierPattern(
                                           new GroupPattern(
                                               new QuantifierPattern(
                                                   new GroupPattern(
                                                       new CharPattern('a'),
                                                       new CharPattern('b')),
                                                   1, null, true)),
                                           1, null, true));

            Assert.AreEqual(expected, actual, "With parentheses");
        }
    }
}
