using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser.ParserCombinators
{
    public class CharParsers<TInput> : Parsers<TInput>
    {
        public CharParsers(Parser<TInput, char> parseOneChar)
        {
            this.parseOneChar = parseOneChar;
        }

        private readonly Parser<TInput, char> parseOneChar;

        public Parser<TInput, char> AnyChar { get { return parseOneChar; } }

        public Parser<TInput, char> Satisfy(Predicate<char> predicate)
        {
            return from c in parseOneChar
                   where predicate(c)
                   select c;
        }

        public Parser<TInput, char> Char(char ch)
        {
            return Satisfy(c => c == ch);
        }
    }
}
