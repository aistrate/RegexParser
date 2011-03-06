using System.Collections.Generic;
using System.Text;
using RegexParser.Patterns;

namespace RegexParser.Transforms
{
    /// <summary>
    /// An Abstract Syntax Tree (AST) transform in which every sequence of CharEscapePatterns becomes a StringPattern.
    /// </summary>
    public class StringASTTransform : BaseASTTransform
    {
        public override BasePattern Transform(BasePattern pattern)
        {
            if (pattern.Type == PatternType.Group)
            {
                // TODO: use groupBy (Haskell-style)

                GroupPattern group = (GroupPattern)pattern;
                List<BasePattern> newChildPatterns = new List<BasePattern>();
                StringBuilder currentString = new StringBuilder();

                foreach (BasePattern oldChildPattern in group.Patterns)
                    if (oldChildPattern is CharEscapePattern)
                        currentString.Append(((CharEscapePattern)oldChildPattern).Value);
                    else
                    {
                        if (currentString.Length > 0)
                        {
                            addStringPattern(newChildPatterns, currentString.ToString());
                            currentString = new StringBuilder();
                        }

                        newChildPatterns.Add(Transform(oldChildPattern));
                    }

                if (currentString.Length > 0)
                    addStringPattern(newChildPatterns, currentString.ToString());

                return CreateGroupOrSingleton(group.IsCapturing, newChildPatterns.ToArray());
            }
            else
                return base.Transform(pattern);
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
