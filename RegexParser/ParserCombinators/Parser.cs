using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser.ParserCombinators
{
    public delegate Result<TInput, TValue> Parser<TInput, TValue>(TInput input);
}
