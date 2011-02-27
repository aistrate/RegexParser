using System.Collections.Generic;
using System.Linq;

namespace Utility.BaseTypes
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

        public static string AsString(this IEnumerable<char> chars)
        {
            return new string(chars.ToArray());
        }

        public static string JoinStrings(this IEnumerable<string> strings)
        {
            return strings.SelectMany(s => s).AsString();
        }

        public static string JoinStrings(this IEnumerable<string> strings, string separator)
        {
            return string.Join(separator, strings.ToArray());
        }

        /// <summary>
        /// Converts a character into C# character-literal format (ready to be pasted into a program), including the single quotes.
        /// </summary>
        public static string Show(this char c)
        {
            if (c == '\'')
                return @"'\''";
            else
                return string.Format("'{0}'", c.ShowLitChar());
        }

        /// <summary>
        /// Converts a string into C# string-literal format (ready to be pasted into a program), including the double quotes.
        /// </summary>
        public static string Show(this string s)
        {
            return string.Format("\"{0}\"", s.SelectMany(c => c.ShowLitChar())
                                             .AsString()
                                             .Replace("\"", "\\\""));
        }

        /// <summary>
        /// Converts a string into C# *verbatim* string-literal format (ready to be pasted into a program), including the double quotes.
        /// Verbatim string literals start with a '@' character (before the opening quote).
        /// </summary>
        public static string ShowVerbatim(this string s)
        {
            return string.Format("@\"{0}\"", s.Replace("\"", "\"\""));
        }

        public static string ShowLitChar(this char c)
        {
            if (c == '\\')
                return @"\\";
            else if (c >= ' ')
                return c.ToString();

            switch (c)
            {
                case '\a':
                    return @"\a";
                case '\b':
                    return @"\b";
                case '\f':
                    return @"\f";
                case '\n':
                    return @"\n";
                case '\r':
                    return @"\r";
                case '\t':
                    return @"\t";
                case '\v':
                    return @"\v";
                default:
                    return string.Format(@"\x{0:X2}", (int)c);
            }
        }
    }
}
