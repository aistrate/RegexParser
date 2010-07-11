using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexParser.ParserCombinators;

namespace RegexParser.Tests.ParserCombinators.MiniML
{
    public class MiniMLParsers : CharParsers
    {
        public MiniMLParsers()
        {
            Whitespace = Many(Choice(new[] { Char(' '), Char('\t'), Char('\n'), Char('\r') }));

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

            Term = Choice(new[] {
                            from u1 in WsChr('\\')
                            from x in Ident
                            from u2 in WsChr('.')
                            from t in Term
                            select (Term)new LambdaTerm(x, t),

                            from letid in LetId
                            from x in Ident
                            from u1 in WsChr('=')
                            from t in Term
                            from inid in InId
                            from c in Term
                            select (Term)new LetTerm(x, t, c),

                            from t in Term1
                            from ts in Many(Term1)
                            select (Term)new AppTerm(t, ts.ToArray()) });

            All = from t in Term
                  from u in WsChr(';')
                  select t;
        }

        public Parser<char, IEnumerable<char>> Whitespace;
        public Func<char, Parser<char, char>> WsChr;
        public Parser<char, string> Id;
        public Parser<char, string> Ident;
        public Parser<char, string> LetId;
        public Parser<char, string> InId;
        public Parser<char, Term> Term;
        public Parser<char, Term> Term1;
        public Parser<char, Term> All;
    }
}
