using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserCombinators
{
    public class CharParsers : Parsers<char>
    {
        public static Parser<char, char> Satisfy(Func<char, bool> predicate)
        {
            return from c in Token
                   where predicate(c)
                   select c;
        }

        public static Parser<char, char> AnyChar
        {
            get { return Satisfy(c => true); }
        }

        public static Parser<char, char> Char(char ch)
        {
            return Satisfy(c => c == ch);
        }

        public static Parser<char, char> OneOf(string chars)
        {
            return Satisfy(c => chars.IndexOf(c) >= 0);
        }

        public static Parser<char, char> NoneOf(string chars)
        {
            return Satisfy(c => chars.IndexOf(c) < 0);
        }
    }
}
