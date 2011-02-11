using RegexParser.Matchers;

namespace RegexParser.Tests
{
    public abstract class AlgorithmTests
    {
        public AlgorithmTests(AlgorithmType algorithmType)
        {
            AlgorithmType = algorithmType;
        }

        protected AlgorithmType AlgorithmType { get; private set; }
    }
}
