# How _RegexParser_ Works #


_RegexParser_ works in three phases:

1. Parsing the regex pattern, which results in an [Abstract Syntax Tree][1] (_AST_)
2. Transforming the _AST_
3. Pattern matching on the target string using the _AST_

Phases 1 and 2 happen only once for a given regex. Phase 3 may happen multiple times, for different target strings.


<toc>

- [Phase 1: Parsing the Regex Pattern](#phase-1-parsing-the-regex-pattern)
    - [The Parser Type](#the-parser-type)
    - [Parser Combinators in C#](#parser-combinators-in-c)
    - [The Parser _Monad_ in C#](#the-parser-monad-in-c)
    - [Parsing the Regex Language](#parsing-the-regex-language)
- [Phase 2: Transforming the _Abstract Syntax Tree_](#phase-2-transforming-the-abstract-syntax-tree)
- [Phase 3: Pattern Matching on the Target String](#phase-3-pattern-matching-on-the-target-string)
    - [Matching without Backtracking](#matching-without-backtracking)
    - [The Need for Backtracking](#the-need-for-backtracking)

</toc>



## Phase 1: Parsing the Regex Pattern ##


### The Parser Type ###

The `Parser` type is defined as:

```C#
public delegate Result<TToken, TTree> Parser<TToken, TTree>(IConsList<TToken> consList);

public class Result<TToken, TTree>
{
    public Result(TTree tree, IConsList<TToken> rest)
    {
        Tree = tree;
        Rest = rest;
    }
    public TTree Tree { get; private set; }
    public IConsList<TToken> Rest { get; private set; }
}

public interface IConsList<T>
{
    T Head { get; }
    IConsList<T> Tail { get; }

    bool IsEmpty { get; }
    int Length { get; }
}
```

A parser is simply a function that takes a list of tokens (e.g., characters), and returns a syntax tree and the list of unconsumed tokens. To indicate failure to match, it will return `null`.

The `Parser` type is equivalent to the following _Haskell_ type:

```Haskell
newtype Parser token tree = Parser ([token] -> Maybe (tree, [token]))
```

> **NOTE**: The idea (and syntax) of _parser combinators_ came from these articles:

> - [Monadic Parsing in Haskell][11] (Hutton, Meijer) (1998)
> - [Parsec, a fast combinator parser][12] (Leijen) (2001)

> In the articles, the type is defined similarly to:

> `newtype Parser token tree = Parser ([token] -> [(tree, [token])])`

> This allows the parser to be ambiguous (to parse a string in multiple ways). The parser will return either a list of one or more "success" alternatives, or an empty list to indicate failure.

> As the regex syntax is non-ambigious, the `Maybe` definition was preferred.


### Parser Combinators in C# ###

[Parser combinators][2] are higher-order functions that can be used to combine basic parsers to construct parsers for more complex rules.

The following parser combinators are defined (see [source](/ParserCombinators/Parsers.cs) for descriptions):

- `Choice`
- `Option`
- `Many`
- `Many1`
- `Count`
- `Sequence`
- `PrefixedBy`
- `Between`
- `SepBy`
- `SepBy1`
- `NotFollowedBy`
- `Eof`
- `AnyToken`
- `Succeed`
- `Fail`

For example, the `Choice` combinator is defined as:

```C#
// Try to apply the parsers in the 'choices' list in order, until one succeeds.
// Return the tree returned by the succeeding parser.
public static Parser<TToken, TTree> Choice<TTree>(params Parser<TToken, TTree>[] choices)
{
    return consList =>
    {
        foreach (var parser in choices)
        {
            var result = parser(consList);
            if (result != null)
                return result;
        }
        return null;
    };
}
```

Beside combinators, there are also a number of "primitive" character parsers (see [source](/ParserCombinators/CharParsers.cs) for descriptions):

- `AnyChar`
- `Satisfy`
- `Char`
- `Digit`
- `OctDigit`
- `HexDigit`
- `OneOf`
- `NoneOf`

Each of these will consume exactly _one_ character.


### The Parser _Monad_ in C# ###

[LINQ][3], the data querying subset of _C#_, offers a form of _syntactic sugar_ that allows writing code similar to _Haskell_ `do` notation. This greatly simplifies the writing of more complex parsers.

For example, let's say we want to write a parser called `naturalNum`, which reads a sequence of digits and returns an `int` as syntactic tree. Using parser combinators and primitives from the previous section (i.e., `Many1` and `Digit`), we can define it like this:

```C#
Parser<char, int> naturalNum =
    consList =>
    {
        var result = Many1(Digit)(consList);

        if (result != null)
            return new Result<char, int>(readInt(result.Tree), result.Rest);
        else
            return null;
    };
```

Because this is such a common pattern, we can define a helper [extension method][4], `Select()`:

```C#
public static Parser<TToken, TTree2> Select<TToken, TTree, TTree2>(
                                        this Parser<TToken, TTree> parser,
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
```

Now the parser can be written more simply as:

```C#
Parser<char, int> naturalNum = Many1(Digit).Select(ds => readInt(ds));
```

A `Select()` method with a signature similar to ours has special meaning for _LINQ_. Taking advantage of that, we can rewrite the parser in a _syntactic sugar_ form, which will be translated (_desugared_) by the C# preprocessor to exactly the same code as above:

```C#
Parser<char, int> naturalNum = from ds in Many1(Digit)
                               select readInt(ds);
```

Notice the similarity with `do` notation in _Haskell_:

```Haskell
naturalNum = do ds <- many1 digit
                return (readInt ds)
```

So far, we could only use the `from` keyword _once_ per expression. By also defining a _LINQ_-related method called `SelectMany()` (see [source](/ParserCombinators/ParserMonad.cs)), we become able to build parser expressions that use `from` more than once. For example, if we want to parse an integer (prefixed by an optional minus sign), we can write:

```C#
Parser<char, int> integerNum = from sign in Option('+', Char('-'))
                               from ds in Many1(Digit)
                               let s = (sign == '-' ? -1 : 1)
                               select s * readInt(ds);
```

This is equivalent to the following _Haskell_ parser:

```Haskell
integerNum = do sign <- option '+' (char '-')
                ds <- many1 digit
                let s = if sign == '-' then -1 else 1
                return (s * (readInt ds))
```


### Parsing the Regex Language ###

Using parser combinators and primitives, as well as the _syntactic sugar_ notation described, we can write a parser for the whole regex language (as supported by _RegexParser_) in less than **150 lines** of code (see [source](/RegexParser/Patterns/PatternParsers.cs)).

For example, the `Quantifier` parser, which accepts the forms: <code>**&#42;**</code>, <code>**+**</code>, <code>**?**</code>, <code>**{**_n_**}**</code>, <code>**{**_n_**,}**</code>, <code>**{**_n_**,**_m_**}**</code> (_greedy_ quantifiers), and <code>**&#42;?**</code>, <code>**+?**</code>, <code>**??**</code>, <code>**{**_n_**}?**</code>, <code>**{**_n_**,}?**</code>, <code>**{**_n_**,**_m_**}?**</code> (_lazy_ quantifiers), is defined like this:

```C#
var RangeQuantifierSuffix = Between(Char('{'),
                                    Char('}'),

                                    from min in NaturalNum
                                    from max in
                                        Option(min, PrefixedBy(Char(','),
                                                               Option(null, Nullable(NaturalNum))))
                                    select new { Min = min, Max = max });

Quantifier = from child in Atom
             from quant in
                 Choice(
                     from _q in Char('*') select new { Min = 0, Max = (int?)null },
                     from _q in Char('+') select new { Min = 1, Max = (int?)null },
                     from _q in Char('?') select new { Min = 0, Max = (int?)1 },
                     RangeQuantifierSuffix)
             from greedy in
                 Option(true, from _c in Char('?')
                              select false)
             select (BasePattern)new QuantifierPattern(child, quant.Min, quant.Max, greedy);
```

The more complex parsers are built from more simple ones. The topmost parser is called `Regex`. The result of parsing is a tree of _pattern_ objects (derived from class `BasePattern`). Here are the main pattern classes (see [sources](/RegexParser/Patterns)):

- `CharEscapePattern`
- `CharGroupPattern`, `CharRangePattern`, `CharClassSubtractPattern`, `AnyCharPattern`, `CaseInsensitiveCharPattern` (dealing with character classes)
- `GroupPattern`
- `QuantifierPattern`
- `AlternationPattern`
- `AnchorPattern`

All pattern classes are _immutable_.



## Phase 2: Transforming the _Abstract Syntax Tree_ ##

The following transforms are performed (see [sources](/RegexParser/Transforms)):

- `BaseASTTransform`: Remove empty groups; replace non-capturing groups that have a single child pattern with the pattern itself.

- `QuantifierASTTransform`: Split quantifiers into their deterministic and non-deterministic parts. For example, the `QuantifierPattern` representing <code>**a{2,5}**</code> will be split into two patterns, equivalent to <code>**a{2}a{0,3}**</code>. The second pattern is fully non-deterministic, which means that _backtracking_ can and will be used at every step.

    Also, clean up the corner cases: quantifiers with empty child patterns, etc.

- `RegexOptionsASTTransform`: Implement the global regex options `IgnoreCase`, `Multiline` and `Singleline` by transforming `CharPattern` and `AnchorPattern` objects.



## Phase 3: Pattern Matching on the Target String ##


### Matching without Backtracking ###

The simplest way to parse the target string is to build a parser from the _AST_ using the combinators we already have. This, however, would be a _non-backtracking_ parser, as our `Parser` type does not allow returning multiple "success" alternatives.

Here is a recursive definition (see [source](/RegexParser/Matchers/ExplicitDFAMatcher.cs)), based on the `Sequence`, `Count`, `Choice` and `Satisfy` combinators:

```C#
private Parser<char, string> createParser(BasePattern pattern)
{
    if (pattern == null)
        throw new ArgumentNullException("pattern.", "Pattern is null when creating match parser.");

    switch (pattern.Type)
    {
        case PatternType.Group:
            return from vs in
                       CharParsers.Sequence(((GroupPattern)pattern).Patterns
                                                                   .Select(p => createParser(p)))
                   select vs.JoinStrings();

        case PatternType.Quantifier:
            QuantifierPattern quant = (QuantifierPattern)pattern;
            return from vs in CharParsers.Count(quant.MinOccurrences,
                                                quant.MaxOccurrences,
                                                createParser(quant.ChildPattern))
                   select vs.JoinStrings();

        case PatternType.Alternation:
            return CharParsers.Choice(((AlternationPattern)pattern).Alternatives
                                                                   .Select(p => createParser(p))
                                                                   .ToArray());

        case PatternType.Char:
            return from c in CharParsers.Satisfy(((CharPattern)pattern).IsMatch)
                   select new string(c, 1);

        default:
            throw new ApplicationException(
                string.Format("ExplicitDFAMatcher: unrecognized pattern type ({0}).",
                              pattern.GetType().Name));
    }
}
```

As a further drawback, this parser does not (and cannot) deal with _anchor_ patterns.


### The Need for Backtracking ###

To understand the need for backtracking, let's consider a simple example: we want to find all the words that end with <code>**t**</code> within the target <code>**"a lot of important text"**</code>.

We start with the most logical pattern: <code>**\w+t**</code> (any word character, repeated one or more times, followed by <code>**t**</code>). This doesn't work as expected: <code>**\w+**</code> will match <code>**lot**</code> (including the final <code>**t**</code>), so the <code>**t**</code> in the pattern won't have anything left to match.

We then try the pattern <code>**&#91;\w-&#91;t&#93;&#93;+t**</code> (any word character except <code>**t**</code>, repeated one or more times, followed by <code>**t**</code>). This will match <code>**lot**</code> correctly, but then it will match <code>**import**</code>, <code>**ant**</code>, and <code>**ext**</code>. Not correct.

Next we try <code>**\w+?t**</code> (any word character, repeated one or more times _lazily_, followed by <code>**t**</code>). This produces the same result as above. (Not to mention the fact that lazy quantifiers actually need backtracking in order to work.)

What _would_ work is the following: suppose that <code>**\w+**</code> matches every word character _including_ the <code>**t**</code> (in <code>**lot**</code>); then, when the parser notices that the next part of the pattern (i.e., <code>**t**</code>) does not match, it tries to _backtrack_ one match of the quantifier's subpattern (one word character); so now it has matched only <code>**lo**</code>, and the <code>**t**</code> in the pattern _will_ match.

Possible complications:

- Backtracking might need to go back thousands of matches (all of which need to be kept track of in a stack, like a trail of breadcrumbs).

- The moment of "mismatch" may arrive long after the end of the non-deterministic pattern (instead of right after it, as in our example), thousands of characters away, and in a different part of the pattern tree. Jumping back will need to restore the whole context as of _just before_ the match, including location in the pattern tree, and location in the target string.


  [1]: http://en.wikipedia.org/wiki/Abstract_Syntax_Tree
  [2]: http://en.wikipedia.org/wiki/Parser_combinator
  [3]: http://en.wikipedia.org/wiki/Language_Integrated_Query
  [4]: http://en.wikipedia.org/wiki/Extension_method

  [11]: https://github.com/aistrate/RegexParser/raw/master/Haskell/Monadic%20Parsing%20in%20Haskell%20(Hutton%2C%20Meijer%3B%201998).pdf
  [12]: https://github.com/aistrate/RegexParser/raw/master/Haskell/Parsec%2C%20a%20fast%20combinator%20parser%20(Leijen%3B%202001).pdf
