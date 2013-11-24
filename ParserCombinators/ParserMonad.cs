using System;

namespace ParserCombinators
{
    public static class ParserMonad
    {
        public static Parser<TToken, TTree2> Select<TToken, TTree, TTree2>(this Parser<TToken, TTree> parser,
                                                                           Func<TTree, TTree2> selector)
        {
            return consList =>
            {
                var result = parser(consList);
                
                if (result != null)
                    return new Result<TToken, TTree2>(selector(result.Tree), result.Rest);
                else
                    return null;
            };
        }

        public static Parser<TToken, TTree2> SelectMany<TToken, TTree, TIntermediate, TTree2>(this Parser<TToken, TTree> parser,
                                                                                              Func<TTree, Parser<TToken, TIntermediate>> selector,
                                                                                              Func<TTree, TIntermediate, TTree2> projector)
        {
            return consList =>
            {
                var result = parser(consList);

                if (result != null)
                {
                    var result2 = selector(result.Tree)(result.Rest);
                    
                    if (result2 != null)
                        return new Result<TToken, TTree2>(projector(result.Tree, result2.Tree), result2.Rest);
                }
                
                return null;
            };
        }

        public static Parser<TToken, TTree> Where<TToken, TTree>(this Parser<TToken, TTree> parser,
                                                                 Func<TTree, bool> predicate)
        {
            return consList =>
            {
                var result = parser(consList);

                if (result != null && predicate(result.Tree))
                    return result;
                else
                    return null;
            };
        }
    }
}
