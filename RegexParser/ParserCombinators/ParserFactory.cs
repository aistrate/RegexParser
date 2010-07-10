using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser.ParserCombinators
{
    public static class ParserFactory
    {
        public static Parser<string, char> CharParserFromString
        {
            get { return input => input.Length > 0 ? new Result<string, char>(input[0], input.Substring(1)) : null; }
        }
    }
}
