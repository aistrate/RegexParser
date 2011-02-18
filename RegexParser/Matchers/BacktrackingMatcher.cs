﻿using System;
using System.Linq;
using ParserCombinators;
using ParserCombinators.ConsLists;
using ParserCombinators.Util;
using RegexParser.Patterns;
using System.Collections.Generic;

namespace RegexParser.Matchers
{
    public class BacktrackingMatcher : BaseMatcher
    {
        public BacktrackingMatcher(string patternText)
            : base(patternText)
        {
        }

        protected override Result<char, string> Parse(IConsList<char> consList)
        {
            BacktrackPoint lastBacktrackPoint = null;

            var callStack = new StackFrame(null, Pattern);
            var partialResult = new Result<char, SimpleConsList<char>>(SimpleConsList<char>.Empty, consList);

            while (callStack != null)
            {
                if (callStack.RemainingChildren.IsEmpty)
                    callStack = callStack.Parent;
                else
                {
                    BasePattern currentPattern = callStack.RemainingChildren.Head;
                    callStack = new StackFrame(callStack.Parent, callStack.RemainingChildren.Tail);

                    if (currentPattern is GroupPattern)
                        callStack = new StackFrame(callStack, ((GroupPattern)currentPattern).Patterns);

                    else if (currentPattern is QuantifierPattern)
                        callStack = new StackFrame(callStack, transformQuantifier((QuantifierPattern)currentPattern));

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

            return new Result<char, string>(partialResult.Value.AsEnumerable().Reverse().AsString(),
                                            partialResult.Rest);
        }

        private IConsList<BasePattern> transformQuantifier(QuantifierPattern quantifier)
        {
            if (quantifier.MinOccurrences == quantifier.MaxOccurrences)
                return new RepeaterConsList<BasePattern>(quantifier.ChildPattern, quantifier.MinOccurrences);

            BasePattern[] transformed;

            if (quantifier.MinOccurrences > 0)
                transformed = new BasePattern[]
                {
                    new QuantifierPattern(quantifier.ChildPattern,
                                          quantifier.MinOccurrences,
                                          quantifier.MinOccurrences,
                                          quantifier.IsGreedy),

                    new QuantifierPattern(quantifier.ChildPattern,
                                          0,
                                          quantifier.MaxOccurrences != null ?
                                                        quantifier.MaxOccurrences - quantifier.MinOccurrences :
                                                        null,
                                          quantifier.IsGreedy),
                };

            else
            {
                var groupPatterns = new List<BasePattern>();

                groupPatterns.Add(quantifier.ChildPattern);

                if (quantifier.MaxOccurrences == null)
                    groupPatterns.Add(quantifier);
                else if (quantifier.MaxOccurrences >= 2)
                    groupPatterns.Add(new QuantifierPattern(quantifier.ChildPattern,
                                                            0,
                                                            quantifier.MaxOccurrences - 1,
                                                            quantifier.IsGreedy));

                GroupPattern group = new GroupPattern(groupPatterns);

                AlternationPattern alternation =
                    quantifier.IsGreedy ? new AlternationPattern(group, GroupPattern.Empty) :
                                          new AlternationPattern(GroupPattern.Empty, group);

                transformed = new BasePattern[] { alternation };
            }

            return new ArrayConsList<BasePattern>(transformed);
        }

        private Result<char, SimpleConsList<char>> parseChar(Result<char, SimpleConsList<char>> partialResult, Func<char, bool> isMatch)
        {
            if (!partialResult.Rest.IsEmpty && isMatch(partialResult.Rest.Head))
                return new Result<char, SimpleConsList<char>>(partialResult.Value.Prepend(partialResult.Rest.Head),
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
                              Result<char, SimpleConsList<char>> partialResult,
                              BasePattern condition)
        {
            Previous = previous;
            CallStack = callStack;
            PartialResult = partialResult;
            Condition = condition;
        }

        public BacktrackPoint Previous { get; private set; }

        public StackFrame CallStack { get; private set; }
        public Result<char, SimpleConsList<char>> PartialResult { get; private set; }

        public BasePattern Condition { get; private set; }
    }
}
