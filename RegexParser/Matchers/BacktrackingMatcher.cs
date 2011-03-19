using System;
using System.Linq;
using ParserCombinators;
using RegexParser.Matchers.Backtracking;
using RegexParser.Patterns;
using RegexParser.Transforms;
using Utility.BaseTypes;
using Utility.ConsLists;

namespace RegexParser.Matchers
{
    public class BacktrackingMatcher : BaseMatcher
    {
        public BacktrackingMatcher(string patternText, RegexOptionsEx options)
            : base(patternText, options)
        {
        }

        protected override BasePattern TransformAST(BasePattern pattern)
        {
            pattern = base.TransformAST(pattern);

            return new QuantifierASTTransform().Transform(pattern);
        }

        protected override Result<char, string> Parse(ArrayConsList<char> consList, int afterLastMatchIndex)
        {
            BacktrackPoint lastBacktrackPoint = null;

            var callStack = new StackFrame(null, Pattern);
            var partialResult = new Result<char, int>(0, consList);

            while (callStack != null)
            {
                BasePattern currentPattern = callStack.RemainingChildren.Head;

                if (callStack is QuantifierStackFrame)
                {
                    QuantifierStackFrame quantStackFrame = (QuantifierStackFrame)callStack;

                    if (partialResult.Value > quantStackFrame.LastPosition)
                    {
                        StackFrame nonEmptyBranch = new StackFrame(quantStackFrame.MoveToNextChild(partialResult.Value), currentPattern),
                                   emptyBranch = quantStackFrame.Parent;

                        lastBacktrackPoint = new BacktrackPoint(lastBacktrackPoint,
                                                                quantStackFrame.IsGreedy ? emptyBranch : nonEmptyBranch,
                                                                partialResult);

                        callStack = quantStackFrame.IsGreedy ? nonEmptyBranch : emptyBranch;
                    }
                    else
                        callStack = quantStackFrame.Parent;
                }
                else
                {
                    if (currentPattern.MinCharLength > partialResult.Rest.Length)
                        partialResult = null;
                    else
                    {
                        callStack = callStack.MoveToNextChild();

                        switch (currentPattern.Type)
                        {
                            case PatternType.Group:
                                callStack = new StackFrame(callStack, ((GroupPattern)currentPattern).Patterns);
                                break;


                            case PatternType.Quantifier:
                                var quant = (QuantifierPattern)currentPattern;

                                quant.AssertCanonicalForm();

                                if (quant.MinOccurrences == quant.MaxOccurrences)
                                    callStack = new StackFrame(callStack,
                                                               new RepeaterConsList<BasePattern>(quant.ChildPattern,
                                                                                                 quant.MinOccurrences));

                                else
                                    callStack = new QuantifierStackFrame(callStack, quant);
                                break;


                            case PatternType.Alternation:
                                var alternatives = ((AlternationPattern)currentPattern).Alternatives;

                                foreach (var alt in alternatives.Skip(1).Reverse())
                                    lastBacktrackPoint = new BacktrackPoint(lastBacktrackPoint,
                                                                            new StackFrame(callStack, alt),
                                                                            partialResult);

                                callStack = new StackFrame(callStack, alternatives.First());
                                break;


                            case PatternType.Anchor:
                                if (!doesAnchorMatch(((AnchorPattern)currentPattern).AnchorType,
                                                     (ArrayConsList<char>)partialResult.Rest,
                                                     afterLastMatchIndex))
                                    partialResult = null;
                                break;


                            case PatternType.Char:
                                partialResult = parseChar(partialResult, ((CharPattern)currentPattern).IsMatch);
                                break;


                            default:
                                throw new ApplicationException(
                                    string.Format("BacktrackingMatcher: unrecognized pattern type ({0}).",
                                                  currentPattern.GetType().Name));
                        }
                    }

                    if (partialResult == null)
                    {
                        if (lastBacktrackPoint != null)
                        {
                            callStack = lastBacktrackPoint.CallStack;
                            partialResult = lastBacktrackPoint.PartialResult;

                            lastBacktrackPoint = lastBacktrackPoint.Previous;
                        }
                        else
                            return null;
                    }
                }

                callStack = unwindEmptyFrames(callStack);
            }

            return new Result<char, string>(consList.AsEnumerable().Take(partialResult.Value).AsString(),
                                            partialResult.Rest);
        }

        private StackFrame unwindEmptyFrames(StackFrame callStack)
        {
            while (callStack != null && callStack.RemainingChildren.IsEmpty)
                callStack = callStack.Parent;

            return callStack;
        }

        private bool doesAnchorMatch(AnchorType anchorType, ArrayConsList<char> currentPos, int afterLastMatchIndex)
        {
            switch (anchorType)
            {
                case AnchorType.StartOfString:
                    return currentPos.IsStartOfArray;

                case AnchorType.StartOfLine:
                    return currentPos.IsStartOfArray || currentPos.Prev == '\n';


                case AnchorType.EndOfString:
                    return currentPos.IsEmpty;

                case AnchorType.EndOfLine:
                    return currentPos.IsEmpty || currentPos.Head == '\n';

                case AnchorType.EndOfStringOrBeforeEndingNewline:
                    return currentPos.DropWhile(c => c == '\n').IsEmpty;


                case AnchorType.ContiguousMatch:
                    return currentPos.ArrayIndex == afterLastMatchIndex;

                case AnchorType.WordBoundary:
                    return isWordBoundary(currentPos);

                case AnchorType.NonWordBoundary:
                    return !isWordBoundary(currentPos);


                default:
                    throw new ApplicationException(
                        string.Format("BacktrackingMatcher: illegal anchor type ({0}).",
                                      anchorType.ToString()));
            }
        }

        private bool isWordBoundary(ArrayConsList<char> currentPos)
        {
            return (!currentPos.IsStartOfArray && CharGroupPattern.WordChar.IsMatch(currentPos.Prev)) ^
                   (!currentPos.IsEmpty && CharGroupPattern.WordChar.IsMatch(currentPos.Head));
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
}
