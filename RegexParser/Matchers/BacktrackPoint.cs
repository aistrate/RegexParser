using ParserCombinators;

namespace RegexParser.Matchers
{
    /// <summary>
    /// Immutable class.
    /// </summary>
    public class BacktrackPoint
    {
        public BacktrackPoint(BacktrackPoint previous,
                              StackFrame callStack,
                              Result<char, int> partialResult)
        {
            Previous = previous;
            CallStack = callStack;
            PartialResult = partialResult;
        }

        public BacktrackPoint Previous { get; private set; }

        public StackFrame CallStack { get; private set; }
        public Result<char, int> PartialResult { get; private set; }
    }
}
