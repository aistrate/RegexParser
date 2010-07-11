using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser.ParserCombinators.ConsLists
{
    public interface IConsList<T>
    {
        T Head { get; }

        IConsList<T> Tail { get; }

        bool IsEmpty { get; }
    }
}
