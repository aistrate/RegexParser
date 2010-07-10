using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexParser.ParserCombinators;

namespace RegexParser.Tests.ParserCombinators.MiniML
{
    public class MiniMLParsers<TInput> : CharParsers<TInput>
    {
        public MiniMLParsers(Parser<TInput, char> parseOneChar)
            : base(parseOneChar)
        {
            //Whitespace = Many(Char(' ').OR(Char('\t').OR(Char('\n')).OR(Char('\r'))));

            // TODO: use method Choice()
            Whitespace = Many(EitherOf(Char(' '),
                              EitherOf(Char('\t'),
                              EitherOf(Char('\n'),
                                       Char('\r')))));

            //WsChr = chr => Whitespace.AND(Char(chr));
            WsChr = chr => from w in Whitespace
                           from c in Char(chr)
                           select c;

            Id = from w in Whitespace
                 from c in Satisfy(char.IsLetter)
                 from cs in Many(Satisfy(char.IsLetterOrDigit))
                 select cs.Aggregate(c.ToString(), (acc, ch) => acc + ch);

            Ident = from s in Id
                    where s != "let" && s != "in"
                    select s;

            LetId = from s in Id
                    where s == "let"
                    select s;

            InId = from s in Id
                   where s == "in"
                   select s;

            Term1 = EitherOf(from x in Ident
                             select (Term)new VarTerm(x),
                             
                             from u1 in WsChr('(')
                             from t in Term
                             from u2 in WsChr(')')
                             select t);

            // TODO: use method Choice()
            Term = EitherOf(from u1 in WsChr('\\')
                            from x in Ident
                            from u2 in WsChr('.')
                            from t in Term
                            select (Term)new LambdaTerm(x, t),

                   EitherOf(from letid in LetId
                            from x in Ident
                            from u1 in WsChr('=')
                            from t in Term
                            from inid in InId
                            from c in Term
                            select (Term)new LetTerm(x, t, c),

                            from t in Term1
                            from ts in Many(Term1)
                            select (Term)new AppTerm(t, ts.ToArray())));

            All = from t in Term
                  from u in WsChr(';')
                  select t;
        }

        public Parser<TInput, IEnumerable<char>> Whitespace;
        public Func<char, Parser<TInput, char>> WsChr;
        public Parser<TInput, string> Id;
        public Parser<TInput, string> Ident;
        public Parser<TInput, string> LetId;
        public Parser<TInput, string> InId;
        public Parser<TInput, Term> Term;
        public Parser<TInput, Term> Term1;
        public Parser<TInput, Term> All;
    }
}
