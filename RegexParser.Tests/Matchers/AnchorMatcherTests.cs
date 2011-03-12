using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Tests.Asserts;

namespace RegexParser.Tests.Matchers
{
#if TEST_BACKTRACKING
    [TestFixture(AlgorithmType.Backtracking)]
#endif

#if TEST_BACKTRACKING

    public class AnchorMatcherTests : AlgorithmTests
    {
        public AnchorMatcherTests(AlgorithmType algorithmType)
            : base(algorithmType) { }

        [Test]
        public void StartAndEndOfStringOrLine()
        {
            string input = "One thing or another\nAnd then some";

            string[] patterns = new[] {
                @"^\w+",
                @"\w+$",

                @"^\w+|\w+$",
                @"\w+$|^\w+",

                @"\w*^\w*",
                @"\w*$\w*",
                @"\w+$\w*",

                @"\A\w+",
                @"\w+\Z",
                @"\w+\z",

                @"\A\w+|\w+\Z",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType, RegexOptions.Multiline);
        }

        [Test]
        public void StartAndEndOfStringOrLine_Newlines()
        {
            string input = "AB\nCD\r\nEF\n\rGH\rIJ\n\nKL\r\rMN\n\r\nOP\r\n\rQR\n";

            string[] patterns = new[] {
                @"^\w+",
                @"^.+",

                @"\w+$",
                @"\w+\r?$",
                @".+$",

                @"\A\w+",
                @"\w+\Z",
                @"\w+\z",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType, RegexOptions.Multiline);
        }

        [Test]
        public void StartAndEndOfStringOrLine_Empty()
        {
            string input = "";

            string[] patterns = new[] {
                @"^",
                @"$",
                @"^$",
                @"^\w*$",

                @"\A",
                @"\A\w*",
                @"\Z",
                @"\z",
                @"\A\Z",
                @"\A\z",
                @"\A\w*\z",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType, RegexOptions.Multiline);
        }

        [Test]
        public void WordBoundary()
        {
            string input = "One thing or another\nAnd then some";

            string[] patterns = new[] {
                @"\b\w+",
                @"\w+\b",
                @"\b\w+\b",

                @"\b.+",
                @".+\b",
                @"\b.+\b",
                @"\b.+?\b",
                @"\b.*?\b",

                @"\b[a-z]+",
                @"[a-z]+\b",
                @"\b[a-z]+\b",

                @"\bt\w*",
                @"\b[Aa]\w*",
                @"\w*e\b",
                @"\b[A-Z]\w*",

                @"\B\w+",
                @"\w+\B",
                @"\B\w+\B",

                @"(\B.)+",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void WordBoundary_Empty()
        {
            string input = "";

            string[] patterns = new[] {
                @"\b",
                @"\b\w*",
                @"\b\w*\b",
                @"\b\b",

                @"\B",
                @"\b\B",
                @"\B\b",
                @"\B\B",
                @"\B\w*\B",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void WordBoundary_Punctuation()
        {
            string[] inputs = new[] {
                "?Que?",
                "Woman: without her, man is nothing.",
                "(a_x-b_x)+ a_y - b_y",
                "123+567+3.14-a+a2",
            };

            string[] patterns = new[] {
                @"\b.+?\b",
                @"(\A|\b).+?(\b|\z)",
                @"\B",
            };

            foreach (string input in inputs)
                RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void ContiguousMatch_1()
        {
            string input = "capybara,squirrel,chipmunk,porcupine,gopher," +
                           "beaver,groundhog,hamster,guinea pig,gerbil," +
                           "chinchilla,prairie dog,mouse,rat";

            string[] patterns = new[] {
                @"\G(\w+\s?\w*),?",
                @"(\w+\s?\w*),?",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void ContiguousMatch_2()
        {
            string input = "test string";

            string[] patterns = new[] {
                @"\G\w",
                @"\G\w+",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void ContiguousMatch_3()
        {
            string input = "This is the first sentence. And this is the second one.";

            string[] patterns = new[] {
                @"\G\w+\s*",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void ContiguousMatch_4()
        {
            string input = " One thing or another.";

            string[] patterns = new[] {
                @"\G\w+\s*",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void ContiguousMatch_Empty()
        {
            string input = "";

            string[] patterns = new[] {
                @"\G",
                @"\G\G",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        // TODO: @"(^)+", @"(^\w*)+"

        [Test]
        public void EscapedChars()
        {
            string input = @"abc^d ef gh i$ jk    \A \z\Z \b\B \G   \g \a\n";

            string[] patterns = new[] {
                @"\^",
                @"\$",

                @"\\A",
                @"\\a",
                @"\\Z",
                @"\\z",
                @"\\B",
                @"\\b",
                @"\\G",
                @"\\g",
                @"\\N",
                @"\\n",
                @"\n",

                @"\\",
                @"\\\w",
                @"\\.",
                @"\\[a-z]",
                @"\\[A-Z]",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }
    }

#endif
}
