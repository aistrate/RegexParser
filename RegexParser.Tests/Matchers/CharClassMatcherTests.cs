using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Tests.Helpers;

namespace RegexParser.Tests.Matchers
{
    [TestFixture(AlgorithmType.ExplicitDFA)]
    //[TestFixture(AlgorithmType.Backtracking)]
    public class CharClassMatcherTests : AlgorithmTests
    {
        public CharClassMatcherTests(AlgorithmType algorithmType)
            : base(algorithmType) { }

        [Test]
        public void Ranges()
        {
            string input = "A thing or another thing";

            string[] patterns = new[] {
                "[a-m]",
                "[a-ae-ei-io-ou-u]",
                "[A-Z] [a-z]"
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void CharSetsAndRanges()
        {
            string input = "A thing or another thing";

            string[] patterns = new[] {
                "[aeiou]",
                "[a-fdmzA-D]"
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void NegativeRanges()
        {
            string input = "A thing or another thing";

            string[] patterns = new[] {
                @"[^A-Z ]",
                @"[^ a-z]",
                @"\W",
                @"\S"
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void NamedCharClasses()
        {
            string input = "Therefore they took CS101 and EE201.";

            string[] patterns = new[] {
                @"[A-Z][A-Z][0-9][0-9][0-9]",
                @"[A-Z][A-Z]\d\d\d",
                @"\w\w\d\d\d",
                @"\D\D\d\d\d",
                @"..\d\d\d",
                @"\s..\d\d\d",
                @"\w\w\d\d\d\s",
                @"\w\w\d\d\d\S",
                @"\w\w\d\d\d.",
                @"\d\d\d\d\d",
                @"..\d..",
                @".\d.",
                @"\d.",
                @"\w\w\S\S\S",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void WhitespaceChars()
        {
            string input = "ID\tName\tAddress \t\t\tPhone number\n" +
                           "55\tJohn\tOaks Avenue 5\t065656565\n";

            string[] patterns = new[] {
                @"\s",
                @"\s\s",
                @"\s\s\s",
                @"\s\s\s\s",
                @"\s\w",
                @"\w\s"
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void NonEscapedSpecialCharsInClass()
        {
            string input = @".$^{[(|)*+?\";

            string[] patterns = new[] {
                @"[.]",
                @"[$]",
                @"[{]",
                @"[[]",
                @"[(]",
                @"[|]",
                @"[)]",
                @"[*]",
                @"[+]",
                @"[?]",

                @"[x^]",
                @"[\^]",
                @"[^^]",

                @"[\\]",

                @"[$^{[(|)*+?]",
                @"[.$^{[(|)*+?\\]",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void SpecialCharsForClass()
        {
            string input = ".$^{[(|)*+?\\  - \b\a\n\b xyz []";

            string[] patterns = new[] {
                @"[-]",
                @"[x-]",
                @"[+-]",
                @"[\-]",

                // ASCII order is: "\b\n $()*+-.0?AZ[\\]^az{|}"
                @"[\b]",
                @"[\b-)]",
                @"[\b--]",
                @"[\b-0]",
                @"[\b-|]",
                @"[\b].",

                @"[\]]",
                @"[xyz\]ab]",
                @"[x\[\]\]\\]",

                @"[--\[]",
                @"[^--\[]",
                @"[$--]",

                @"[-[]",
                @"[-[x]",

                @"[--]",
                @"[---]",
                @"[----]",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void DashesInsideClass()
        {
            string input = "abc-def mnpqrstu [].";

            string[] patterns = new[] {
                @"[A-Za\-z]",
                @"[a-d-n]",
                @"[a-d-n-s]",
                @"[a-d-.]",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void ClosingSqrBracketOnFirstPos()
        {
            string input = ".$^{[(|)*+?\\  - \b\a\n\b []";

            string[] patterns = new[] {
                @"[]]",
                @"[]x]",
                @"[][]",
                @"[]-]",
                @"[]-}]",
                @"[x]-}]",
                @"[^]]",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void CharEscapesInClass()
        {
            string input = "ID\tName\tAddress \t\t\tPhone number\n" +
                           "55\tJohn\tOaks Avenue 5\t065656565\n\r" +
                           "\a\a\b\f\v\f\b\v";

            string[] patterns = new[] {
                @"[\a]",
                @"[\b]",
                @"[\f]",
                @"[\n]",
                @"[\r]",
                @"[\t]",
                @"[\v]",

                @"[\n\r]",
                @"[\n\r\t]",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void CharCodesInClass()
        {
            string input = "It cost € 3,200.\nExpensive.";

            string[] patterns = new[] {
                @"[\x20]",
                @"[\x30]",
                @"[\x0AE]",
                @"[\x0AF]",

                @"[\40]",
                @"[\040]",
                @"[\60]",
                @"[\060]",
                @"[\0600]",   // @"\060" + "0"
                @"[\12]",
                @"[\012]",

                @"[€]",
                @"[\u20ac]",
                @"[\u0030]"
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void EscapedSymbolInClass()
        {
            string input = @"abc € _ ' XY "" ` ! - ] 123";

            string[] patterns = new[] {
                @"[\€]",
                @"[\']",
                @"[\""]",
                @"[\`]",
                @"[\!]",

                @"[\`\'\!\=]",

                @"[!-\[]",
                @"[^!-\[]",
                @"[?-\[]",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void NamedCharClassInClass()
        {
            string input = "abcdLas \b (01234)-";

            string[] patterns = new[] {
                @"[\s]",
                @"[\w]",
                @"[\d]",

                @"[(\d)]",
                @"[\d-]",
                @"[(\d)-]",
                @"[(\d-)]",
                @"[-(\d-)-]",

                @"[\s\d]",
                @"[\s\w]",
                @"[\s()]",

                @"[\S]",
                @"[\D]",
                @"[\W]",
                @"[\D0]",

                @"[^\d]",
                @"[^\D]",
                @"[^\d\s]",
                @"[^\d\s()]",

                @"[a-dmx\d]",
                @"[A-Z\d]",
                @"[^a-d\d]",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void FalseNestedGroups()
        {
            RegexAssert.AreMatchesSameAsMsoft("abcdlas \b 01234 [] xyz", @"[[\s]\d]", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abcdlas \b 01234 [7] xyz", @"[[\s]\d]", AlgorithmType);

            RegexAssert.AreMatchesSameAsMsoft("abcdlas \b 01234 [] xyz", @"[\d[\s]]", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abcdlas \b 01234 [7] xyz", @"[\d[\s]]", AlgorithmType);
        }

        [Test]
        public void Subtract()
        {
            string input = "abcdlas \b (01234)- AB MNP [] xyz \x00";

            string[] patterns = new[] {
                @"[\w-[x]]",
                @"[\w-[0-9]]",
                @"[(\w-[0-9]]",
                @"[\w-[\d]]",
                @"[\b-[x]]",
                @"[b-[x]]",
                @"[xyz-[x]]",
                @"[xyz-[^x]]",
                @"[abcdefgh-[d-z]]",

                @"[a-m0-2-[d-z]]",
                @"[^a-m0-2-[d-z]]",
                @"[\x00-\xFF-[a-m0-2-[d-z]]]",

                @"[a-z-[d-z]]",
                @"[a-z-[d-z-[t-z]]]",
                @"[a-z-[d-z-[t-z-[x]]]]",

                @"[\W-[\s]]",
                @"[\w-[^a-z]]",
                @"[\S-[\w]]",
                @"[\S-[^\w]]",
                @"[\S-[\W]]",
                @"[\S-[^\W]]",

                @"[-a-z-[a-d]]",
                @"[[a-z-[a-d]]",
                @"[x[-[a-d]]",
                @"[x[a-z-[a-d]]",

                @"[--[a-d]]",
                @"[-a-[a-d]]",
                @"[-x-[a-d]]",
                @"[-[-[a-d]]",

                @"[[-[a-d]]",
                @"[[[-[a-d]]",

                @"[--[-[a-d]]",
                @"[--[-[\s]]",
                @"[--[\s]]",
                @"[--[[a-d]]",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void FalseSubtract()
        {
            string input = "abcdlas \b (01234)- [] xyz \x00";

            string[] patterns = new[] {
                @"[\d-a]",
                @"[\w-\d]",
                @"[\s-\d]",

                @"[-[a-d]]",
                @"[---[a-d]]",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }
    }
}
