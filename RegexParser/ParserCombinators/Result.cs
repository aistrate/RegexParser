using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser.ParserCombinators
{
    public class Result<TInput, TValue>
    {
        public Result(TValue value, TInput rest)
        {
            Value = value;
            Rest = rest;
        }

        public readonly TValue Value;
        public readonly TInput Rest;
    }
}
