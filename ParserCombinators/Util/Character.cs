using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserCombinators.Util
{
    public static class Character
    {
        public static bool IsDigit(this char c)
        {
            return char.IsDigit(c);
        }

        public static bool IsOctDigit(this char c)
        {
            return '0' <= c && c <= '7';
        }

        public static bool IsHexDigit(this char c)
        {
            return char.IsDigit(c) || ('A' <= c && c <= 'F') || ('a' <= c && c <= 'f');
        }
    }
}
