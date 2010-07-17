using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserCombinators.Util
{
    public static class Numeric
    {
        public static int ReadOct(IEnumerable<char> digits)
        {
            return ReadInt(digits, 8);
        }

        public static int ReadDec(IEnumerable<char> digits)
        {
            return ReadInt(digits, 10);
        }

        public static int ReadHex(IEnumerable<char> digits)
        {
            return ReadInt(digits, 16);
        }

        public static int ReadInt(IEnumerable<char> digits, int numBase)
        {
            var intDigits = digits.Select(c => valDigit(c));

            if (intDigits.Any(d => d >= numBase))
                throw new ApplicationException(string.Format("Could not read '{0}' as a number in base {1}.",
                                                             new string(digits.ToArray()), numBase));

            int result = 0;
            foreach (int d in intDigits)
                result = numBase * result + d;

            return result;
        }

        private static int valDigit(char d)
        {
            if ('0' <= d && d <= '9')
                return (int)d - (int)'0';
            else if ('a' <= d && d <= 'f')
                return (int)d - (int)'a' + 10;
            else if ('A' <= d && d <= 'F')
                return (int)d - (int)'A' + 10;
            else
                throw new ApplicationException(string.Format("Error reading number: unknown digit '{0}'.", d));
        }
    }
}
