using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RegexParser.Patterns;

namespace RegexParser.Tests.Patterns
{
    [TestFixture]
    public class PatternTests
    {
        [Test]
        public void CharPattern_Short()
        {
            BasePattern actual = BasePattern.CreatePattern("abc");

            BasePattern expected = new GroupPattern(new BasePattern[] {
                new CharPattern('a'), 
                new CharPattern('b'),
                new CharPattern('c')
            });

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CharPattern_Empty()
        {
            BasePattern actual = BasePattern.CreatePattern("");

            BasePattern expected = new GroupPattern(new BasePattern[] { });

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GroupPattern()
        {
            string pattern = "t(h(in))g";

            BasePattern actual = BasePattern.CreatePattern(pattern);

            BasePattern expected = new GroupPattern(new BasePattern[] {
                new CharPattern('t'),
                new GroupPattern(new BasePattern[] {
                    new CharPattern('h'),
                    new GroupPattern(new BasePattern[] {
                        new CharPattern('i'),
                        new CharPattern('n')
                    }),
                }),
                new CharPattern('g')
            });

            Assert.AreEqual(expected, actual, pattern);
        }

        [Test]
        public void CharGroupPattern()
        {
            BasePattern actual = BasePattern.CreatePattern("[A-Z][a-z][0-9][A-Za-z][A-Za-z0-9][A-Z.,;:?!a-z][^x][^a-z]");

            BasePattern expected = new GroupPattern(new BasePattern[] {
                new CharGroupPattern(true, upperCase),
                new CharGroupPattern(true, lowerCase),
                new CharGroupPattern(true, digits),
                new CharGroupPattern(true, lowerCase + upperCase),
                new CharGroupPattern(true, upperCase + lowerCase + digits),
                new CharGroupPattern(true, upperCase + lowerCase + ".,;:?!"),

                new CharGroupPattern(false, "x"),
                new CharGroupPattern(false, lowerCase)
            });

            Assert.AreEqual(expected, actual);
        }

        private string upperCase = range('A', 'Z'),
                       lowerCase = range('a', 'z'),
                       digits = range('0', '9');

        private static string range(char from, char to)
        {
            StringBuilder sb = new StringBuilder();

            for (char c = from; c <= to; c++)
                sb.Append(c);

            return sb.ToString();
        }
    }
}
