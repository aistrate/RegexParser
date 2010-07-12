using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexParser.Matchers;

namespace RegexParser.Tests.Matchers
{
    public abstract class MatcherTests
    {
        public MatcherTests(AlgorithmType algorithmType)
        {
            AlgorithmType = algorithmType;
        }

        protected AlgorithmType AlgorithmType { get; private set; }
    }
}
