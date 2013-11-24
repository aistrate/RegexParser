using System;
using System.Text;
using Utility.BaseTypes;

namespace ParserCombinators
{
    public class CharParsers : Parsers<char>
    {
        /// <summary>
        /// Succeed for any character that satisfies 'predicate'. Return the parsed character.
        /// </summary>
        public static Parser<char, char> Satisfy(Func<char, bool> predicate)
        {
            return from c in AnyToken
                   where predicate(c)
                   select c;
        }

        /// <summary>
        /// Succeed for any character. Return the parsed character.
        /// </summary>
        public static Parser<char, char> AnyChar
        {
            get { return Satisfy(c => true); }
        }

        /// <summary>
        /// Succeed for a character that is equal to 'ch'. Return the parsed character.
        /// </summary>
        public static Parser<char, char> Char(char ch)
        {
            return Satisfy(c => c == ch);
        }

        /// <summary>
        /// Succeed for a character that is a digit. Return the parsed character.
        /// </summary>
        public static Parser<char, char> Digit
        {
            get { return Satisfy(c => c.IsDigit()); }
        }

        /// <summary>
        /// Succeed for a character that is an octal digit. Return the parsed character.
        /// </summary>
        public static Parser<char, char> OctDigit
        {
            get { return Satisfy(c => c.IsOctDigit()); }
        }

        /// <summary>
        /// Succeed for a character that is a hexadecimal digit. Return the parsed character.
        /// </summary>
        public static Parser<char, char> HexDigit
        {
            get { return Satisfy(c => c.IsHexDigit()); }
        }

        /// <summary>
        /// Succeed for a character that is equal to any in the string 'chars'.
        /// Return the parsed character.
        /// </summary>
        public static Parser<char, char> OneOf(string chars)
        {
            return Satisfy(c => chars.IndexOf(c) >= 0);
        }

        /// <summary>
        /// Succeed for a character that is NOT equal to any in the string 'chars'.
        /// Return the parsed character.
        /// </summary>
        public static Parser<char, char> NoneOf(string chars)
        {
            return Satisfy(c => chars.IndexOf(c) < 0);
        }

        /// <summary>
        /// Parse a sequence of characters given by 's'. Return the parsed string.
        /// </summary>
        public static Parser<char, string> String(string s)
        {
            // TODO: implement IgnoreCase flag

            return consList =>
            {
                StringBuilder sb = new StringBuilder();

                foreach (char c in s.ToCharArray())
                {
                    if (consList.IsEmpty || consList.Head != c)
                        return null;

                    sb.Append(consList.Head);
                    consList = consList.Tail;
                }

                return new Result<char, string>(sb.ToString(), consList);
            };
        }
    }
}
