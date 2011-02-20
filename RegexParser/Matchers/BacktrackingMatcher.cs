using System;
using System.Collections.Generic;
using System.Linq;
using ParserCombinators;
using ParserCombinators.ConsLists;
using ParserCombinators.Util;
using RegexParser.Patterns;
using RegexParser.Transforms;

namespace RegexParser.Matchers
{
    public class BacktrackingMatcher : BaseMatcher
    {
        public BacktrackingMatcher(string patternText)
            : base(patternText)
        {
        }

        protected override BasePattern TransformAST(BasePattern pattern)
        {
            return new QuantifierASTTransform().Transform(pattern);
        }

        protected override Result<char, string> Parse(IConsList<char> consList)
        {
            BacktrackPoint lastBacktrackPoint = null;

            var callStack = new StackFrame(null, Pattern);
            var partialResult = new Result<char, int>(0, consList);

            while (callStack != null)
            {
                if (callStack.RemainingChildren.IsEmpty)
                    callStack = callStack.Parent;
                else
                {
                    BasePattern currentPattern = callStack.RemainingChildren.Head;
                    callStack = new StackFrame(callStack.Parent, callStack.RemainingChildren.Tail);

                    while (callStack != null && callStack.RemainingChildren.IsEmpty)
                        callStack = callStack.Parent;

                    if (currentPattern is GroupPattern)
                        callStack = new StackFrame(callStack, ((GroupPattern)currentPattern).Patterns);

                    else if (currentPattern is QuantifierPattern)
                    {
                        var quant = (QuantifierPattern)currentPattern;

                        if (quant.MinOccurrences == quant.MaxOccurrences)
                            callStack = new StackFrame(callStack,
                                                       new RepeaterConsList<BasePattern>(quant.ChildPattern, quant.MinOccurrences));

                        else if (quant.MinOccurrences == 0)
                        {
                            BasePattern split = splitQuantifier(quant);

                            BasePattern firstAlt = quant.IsGreedy ? split : null,
                                        secondAlt = quant.IsGreedy ? null : split;

                            lastBacktrackPoint = new BacktrackPoint(lastBacktrackPoint, callStack, partialResult, secondAlt);

                            callStack = new StackFrame(callStack, firstAlt);
                        }

                        else
                            throw new ApplicationException(string.Format("Quantifier pattern with bad parameters: {{{0},{1}}}.",
                                                                         quant.MinOccurrences,
                                                                         quant.MaxOccurrences));
                    }

                    else if (currentPattern is AlternationPattern)
                    {
                        var alternatives = ((AlternationPattern)currentPattern).Alternatives;

                        foreach (var alternative in alternatives.Skip(1).Reverse())
                            lastBacktrackPoint = new BacktrackPoint(lastBacktrackPoint, callStack, partialResult, alternative);

                        callStack = new StackFrame(callStack, alternatives.First());
                    }

                    else if (currentPattern is CharPattern)
                    {
                        partialResult = parseChar(partialResult, ((CharPattern)currentPattern).IsMatch);

                        if (partialResult == null)
                        {
                            if (lastBacktrackPoint != null)
                            {
                                callStack = lastBacktrackPoint.CallStack;
                                partialResult = lastBacktrackPoint.PartialResult;

                                if (lastBacktrackPoint.Condition != null)
                                    callStack = new StackFrame(callStack, lastBacktrackPoint.Condition);

                                lastBacktrackPoint = lastBacktrackPoint.Previous;
                            }
                            else
                                return null;
                        }
                    }

                    else
                        throw new ApplicationException(string.Format("Unknown pattern type ({0}).", currentPattern.GetType().Name));
                }
            }

            return new Result<char, string>(consList.AsEnumerable().Take(partialResult.Value).AsString(),
                                            partialResult.Rest);
        }

        private BasePattern splitQuantifier(QuantifierPattern quant)
        {
            if (quant.MaxOccurrences == 1)
                return quant.ChildPattern;
            else
            {
                QuantifierPattern rest =
                    quant.MaxOccurrences == null ?
                        quant :
                        new QuantifierPattern(quant.ChildPattern,
                                              0,
                                              quant.MaxOccurrences - 1,
                                              quant.IsGreedy);

                return new GroupPattern(quant.ChildPattern, rest);
            }
        }

        private Result<char, int> parseChar(Result<char, int> partialResult, Func<char, bool> isMatch)
        {
            if (!partialResult.Rest.IsEmpty && isMatch(partialResult.Rest.Head))
                return new Result<char, int>(partialResult.Value + 1,
                                             partialResult.Rest.Tail);
            else
                return null;
        }
    }

    /// <summary>
    /// Immutable class.
    /// </summary>
    public class StackFrame
    {

        public StackFrame(StackFrame parent, params BasePattern[] remainingChildren)
            : this(parent, new ArrayConsList<BasePattern>(remainingChildren))
        {
        }

        public StackFrame(StackFrame parent, IConsList<BasePattern> remainingChildren)
        {
            Parent = parent;
            RemainingChildren = remainingChildren;
        }

        public StackFrame Parent { get; private set; }
        public IConsList<BasePattern> RemainingChildren { get; private set; }
    }

    /// <summary>
    /// Immutable class.
    /// </summary>
    public class BacktrackPoint
    {
        public BacktrackPoint(BacktrackPoint previous,
                              StackFrame callStack,
                              Result<char, int> partialResult,
                              BasePattern condition)
        {
            Previous = previous;
            CallStack = callStack;
            PartialResult = partialResult;
            Condition = condition;
        }

        public BacktrackPoint Previous { get; private set; }

        public StackFrame CallStack { get; private set; }
        public Result<char, int> PartialResult { get; private set; }

        public BasePattern Condition { get; private set; }
    }
}
