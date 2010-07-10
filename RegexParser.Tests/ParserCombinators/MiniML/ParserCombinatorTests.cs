using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RegexParser.ParserCombinators;

namespace RegexParser.Tests.ParserCombinators.MiniML
{
    [TestFixture]
    public class ParserCombinatorTests
    {
        [Test]
        public void MiniML()
        {
            string sourceCode = @"let true = \x.\y.x in 
                                  let false = \x.\y.y in 
                                  let if = \b.\l.\r.(b l) r in
                                  if true false true;";

            MiniMLParsers<string> miniMLParsers = new MiniMLParsers<string>(ParserFactory.CharParserFromString);
            Result<string, Term> result = miniMLParsers.All(sourceCode);

            string expected = @"
let true = \x. \y. (x ) in
let false = \x. \y. (y ) in
let if = \b. \l. \r. ((b l) r) in
(if true false true)"
                .TrimStart();

            Assert.AreEqual("", result.Rest, "Rest.");
            Assert.AreEqual(expected, result.Value.ToString(), "Value.");
        }
    }
}
