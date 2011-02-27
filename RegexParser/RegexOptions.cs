using System;

namespace RegexParser
{
    [Flags]
    public enum RegexOptions
    {
        /// <summary>
        /// Specifies that no options are set.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies case-insensitive matching.
        /// </summary>
        IgnoreCase = 1,

        /// <summary>
        /// Multiline mode. Changes the meaning of ^ and $ so they match at the beginning
        /// and end, respectively, of any line, and not just the beginning and end of
        /// the entire string.
        /// </summary>
        Multiline = 2,

        /// <summary>
        /// Specifies that the only valid captures are explicitly named or numbered groups
        /// of the form (?<name>...). This allows unnamed parentheses to act as noncapturing
        /// groups without the syntactic clumsiness of the expression (?:...).
        /// </summary>
        ExplicitCapture = 4,

        /// <summary>
        /// Specifies single-line mode. Changes the meaning of the dot (.) so it matches
        /// every character (instead of every character except \n).
        /// </summary>
        Singleline = 16,

        /// <summary>
        /// Eliminates unescaped white space from the pattern.
        /// However, the System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace
        /// value does not affect or eliminate white space in character classes.
        /// </summary>
        IgnorePatternWhitespace = 32,
    }

    public class RegexOptionsEx
    {
        public RegexOptionsEx(RegexOptions options)
            : this((options & RegexOptions.IgnoreCase) == RegexOptions.IgnoreCase,
                   (options & RegexOptions.Multiline) == RegexOptions.Multiline,
                   (options & RegexOptions.ExplicitCapture) == RegexOptions.ExplicitCapture,
                   (options & RegexOptions.Singleline) == RegexOptions.Singleline,
                   (options & RegexOptions.IgnorePatternWhitespace) == RegexOptions.IgnorePatternWhitespace)
        {
        }

        public RegexOptionsEx(bool ignoreCase,
                              bool multiline,
                              bool explicitCapture,
                              bool singleline,
                              bool ignorePatternWhitespace)
        {
            IgnoreCase = ignoreCase;
            Multiline = multiline;
            ExplicitCapture = explicitCapture;
            Singleline = singleline;
            IgnorePatternWhitespace = ignorePatternWhitespace;
        }

        public bool IgnoreCase { get; private set; }
        public bool Multiline { get; private set; }
        public bool ExplicitCapture { get; private set; }
        public bool Singleline { get; private set; }
        public bool IgnorePatternWhitespace { get; private set; }
    }
}
