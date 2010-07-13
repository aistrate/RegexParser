using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators;
using RegexParser.Patterns;

namespace RegexParser.Matchers
{
    public class MatchParsers : CharParsers
    {
        public Parser<char, string> CreateParser(BasePattern pattern)
        {
            if (pattern.GetType() == typeof(GroupPattern))
                return consList =>
                {
                    StringBuilder groupResult = new StringBuilder();

                    foreach (Parser<char, string> parser in ((GroupPattern)pattern).Patterns.Select(p => CreateParser(p)))
                    {
                        Result<char, string> result = parser(consList);

                        if (result != null)
                        {
                            groupResult.Append(result.Value);
                            consList = result.Rest;
                        }
                        else
                            return null;
                    }

                    return new Result<char, string>(groupResult.ToString(), consList);
                };
            
            else if (pattern.GetType() == typeof(CharPattern))
                return from c in Char(((CharPattern)pattern).Value)
                       select new string(c, 1);
            
            else
                return null;
        }
    }
}
