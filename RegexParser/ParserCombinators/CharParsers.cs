using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser.ParserCombinators
{
    public class CharParsers : Parsers<char>
    {
        public Parser<char, char> Satisfy(Func<char, bool> predicate)
        {
            return from c in Token
                   where predicate(c)
                   select c;
        }

        public Parser<char, char> AnyChar
        {
            get { return Satisfy(c => true); }
        }

        public Parser<char, char> Char(char ch)
        {
            return Satisfy(c => c == ch);
        }

        public Parser<char, char> OneOf(IEnumerable<char> chars)
        {
            return Satisfy(c => chars.Contains(c));
        }

        public Parser<char, char> NoneOf(IEnumerable<char> chars)
        {
            return Satisfy(c => !chars.Contains(c));
        }
    }
}
