using System.Collections.Generic;
using NUnit.Framework;
using Utility.BaseTypes;
using Utility.ConsLists;

namespace ParserCombinators.Tests
{
    [TestFixture]
    public class ParserMonadTests : CharParsers
    {
        private static IConsList<char> posInput = new ArrayConsList<char>("234ab");
        private static IConsList<char> negInput = new ArrayConsList<char>("-5678ab");

        private static IConsList<char> badInput = new ArrayConsList<char>("x234ab");

        [Test]
        public void Explicit()
        {
            Parser<char, int> naturalNum =
                consList =>
                {
                    var result = Many1(Digit)(consList);

                    if (result != null)
                        return new Result<char, int>(readInt(result.Tree), result.Rest);
                    else
                        return null;
                };

            testNaturalNum(naturalNum);
        }

        [Test]
        public void Select()
        {
            Parser<char, int> naturalNum = Many1(Digit).Select(ds => readInt(ds));

            testNaturalNum(naturalNum);
        }

        [Test]
        public void SyntacticSugar()
        {
            Parser<char, int> naturalNum = from ds in Many1(Digit)
                                           select readInt(ds);

            testNaturalNum(naturalNum);
        }

        [Test]
        public void WithSign()
        {
            Parser<char, int> integerNum = from sign in Option('+', Char('-'))
                                           from ds in Many1(Digit)
                                           let s = (sign == '-' ? -1 : 1)
                                           select s * readInt(ds);

            Result<char, int> result = integerNum(negInput);

            Assert.AreEqual(-5678, result.Tree);
            Assert.AreEqual("ab", result.Rest.AsEnumerable().AsString());

            testNaturalNum(integerNum);
        }

        private void testNaturalNum(Parser<char, int> parser)
        {
            Result<char, int> result = parser(posInput);

            Assert.AreEqual(234, result.Tree);
            Assert.AreEqual("ab", result.Rest.AsEnumerable().AsString());


            result = parser(badInput);

            Assert.IsNull(result);
        }

        private int readInt(IEnumerable<char> digits)
        {
            return Numeric.ReadDec(digits);
        }
    }
}
