using System;
using System.Collections.Generic;
using System.Linq;
using RegexParser.Patterns;
using RegexParser.Util;

namespace RegexParser.Transforms
{
    /// <summary>
    /// Class representing an Abstract Syntax Tree (AST) transform.
    /// </summary>
    public abstract class BaseASTTransform
    {
        //public virtual BasePattern Transform(BasePattern pattern)
        //{
        //    // The Identity transform

        //    if (pattern is GroupPattern)
        //        return new GroupPattern(((GroupPattern)pattern).Patterns
        //                                                       .Select(a => Transform(a)));

        //    else if (pattern is QuantifierPattern)
        //    {
        //        QuantifierPattern quant = (QuantifierPattern)pattern;

        //        return new QuantifierPattern(Transform(quant.ChildPattern),
        //                                     quant.MinOccurrences, quant.MaxOccurrences, quant.IsGreedy);
        //    }

        //    else if (pattern is AlternationPattern)
        //        return new AlternationPattern(((AlternationPattern)pattern).Alternatives
        //                                                                   .Select(a => Transform(a)));

        //    else
        //        return pattern;
        //}

        public virtual BasePattern Transform(BasePattern pattern)
        {
            // The Identity transform

            if (pattern is GroupPattern)
                return transform<GroupPattern>((GroupPattern)pattern,
                                               g => g.Patterns,
                                               (g, ps) => new GroupPattern(ps));

            else if (pattern is QuantifierPattern)
                return transform<QuantifierPattern>((QuantifierPattern)pattern,
                                                    q => q.ChildPattern,
                                                    (q, p) => new QuantifierPattern(p,
                                                                                    q.MinOccurrences,
                                                                                    q.MaxOccurrences,
                                                                                    q.IsGreedy));

            else if (pattern is AlternationPattern)
                return transform<AlternationPattern>((AlternationPattern)pattern,
                                                     a => a.Alternatives,
                                                     (a, ps) => new AlternationPattern(ps));

            else
                return pattern;
        }

        private T transform<T>(T oldPattern,
                               Func<T, IEnumerable<BasePattern>> getChildren,
                               Func<T, IEnumerable<BasePattern>, T> createNewPattern)
            where T : BasePattern
        {
            return transformC<T, IEnumerable<BasePattern>>(
                        oldPattern,
                        getChildren,
                        createNewPattern,

                        ps => ps.Select(p => Transform(p)),
                        (nps, ops) => !nps.Zip(ops, (n, o) => !object.ReferenceEquals(n, o)).Any());
        }

        private T transform<T>(T oldPattern,
                               Func<T, BasePattern> getChild,
                               Func<T, BasePattern, T> createNewPattern)
            where T : BasePattern
        {
            return transformC<T, BasePattern>(
                        oldPattern,
                        getChild,
                        createNewPattern,

                        Transform,
                        (n, o) => ReferenceEquals(n, o));
        }

        private T transformC<T, Coll>(T oldPattern,
                                      Func<T, Coll> getChildren,
                                      Func<T, Coll, T> createNewPattern,
                                      Func<Coll, Coll> transformChildren,
                                      Func<Coll, Coll, bool> referenceEquals)
            where T : BasePattern
        {
            Coll oldChildren = getChildren(oldPattern),
                 newChildren = transformChildren(oldChildren);

            if (!referenceEquals(newChildren, oldChildren))
                return createNewPattern(oldPattern, newChildren);
            else
                return oldPattern;
        }
    }
}
