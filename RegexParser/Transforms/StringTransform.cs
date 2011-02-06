using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexParser.Patterns;

namespace RegexParser.Transforms
{
    /// <summary>
    /// A tree transform where every sequence of CharEscapePatterns becomes a StringPattern.
    /// </summary>
    public class StringTransform : BaseTransform
    {
        public override BasePattern RunTransform(BasePattern pattern)
        {
            if (pattern is GroupPattern)
            {
                BasePattern[] oldChildPatterns = ((GroupPattern)pattern).Patterns;
                List<BasePattern> newChildPatterns = new List<BasePattern>();
                StringBuilder currentString = new StringBuilder();

                foreach (BasePattern oldChildPattern in oldChildPatterns)
                    if (oldChildPattern is CharEscapePattern)
                        currentString.Append(((CharEscapePattern)oldChildPattern).Value);
                    else
                    {
                        if (currentString.Length > 0)
                        {
                            addStringPattern(newChildPatterns, currentString.ToString());
                            currentString = new StringBuilder();
                        }

                        newChildPatterns.Add(RunTransform(oldChildPattern));
                    }

                if (currentString.Length > 0)
                    addStringPattern(newChildPatterns, currentString.ToString());

                return new GroupPattern(newChildPatterns);
            }
            else
                return base.RunTransform(pattern);
        }

        private void addStringPattern(List<BasePattern> patterns, string s)
        {
            if (s.Length > 1)
                patterns.Add(new StringPattern(s));
            else
                patterns.Add(new CharEscapePattern(s[0]));
        }
    }
}
