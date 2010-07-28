using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators.Util;

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

        public static Parser<char, char> Digit
        {
            get { return Satisfy(c => c.IsDigit()); }
        }

        public static Parser<char, char> OctDigit
        {
            get { return Satisfy(c => c.IsOctDigit()); }
        }

        public static Parser<char, char> HexDigit
        {
            get { return Satisfy(c => c.IsHexDigit()); }
        }

        public static Parser<char, char> OneOf(string chars)
        {
            return Satisfy(c => chars.IndexOf(c) >= 0);
        }

        public static Parser<char, char> NoneOf(string chars)
        {
            return Satisfy(c => chars.IndexOf(c) < 0);
        }

        public static Parser<char, string> String(string s)
        {
            return from cs in Sequence(s.Select(c => Char(c)))
                   select cs.AsString();
        }
    }
}
