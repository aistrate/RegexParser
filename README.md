Regex Parser
============

### Implemented Regex Features ###

- Character escapes:
    - any character except for one of **<code>.&#36;^{&#91;(|)&#42;+?&#92;</code>** matches itself
    - **`\n`**: new line
    - **`\r`**: carriage return
    - **`\t`**: tab
    - **`\b`**: backspace (only inside a character class)
    - <code><strong>&#92;</strong>_nnn_</code>: ASCII character, where _`nnn`_ is a two- or three-digit octal character code
    - <code><strong>\x</strong>_nn_</code>: ASCII character, where _`nn`_ is a two-digit hexadecimal character code
    - <code><strong>\u</strong>_nnnn_</code>: UTF-16 code unit whose value is _`nnnn`_ hexadecimal
    - **`\`** followed by a character not recognized as escaped matches that character
- Character classes:
    - <code>**.**</code> matches any character except <code>**\n**</code>
    - positive character groups (e.g., <code>**&#91;aeiou&#93;**</code>, <code>**&#91;a-zA-Z&#93;**</code>, <code>**&#91;abcA-H\d\n&#93;**</code>)
    - negative character groups (e.g., <code>**&#91;^a-zA-Z&#93;**</code>)
    - named character classes:
        - **`\w`**: a word character; same as <code>**&#91;0-9A-Z&#95;a-z&#93;**</code>
        - **`\W`**: a non-word character; same as <code>**&#91;^0-9A-Z&#95;a-z&#93;**</code>
        - **`\s`**: a whitespace character; same as <code>**&#91; \n\r\t&#93;**</code>
        - **`\S`**: a non-whitespace character; same as <code>**&#91;^ \n\r\t&#93;**</code>
        - **`\d`**: a digit character; same as <code>**&#91;0-9&#93;**</code>
        - **`\D`**: a non-digit character; same as <code>**&#91;^0-9&#93;**</code>
    - character class subtraction (e.g., <code>**&#91;0-9-&#91;246&#93;&#93;**</code> matches any digit except for 2, 4 and 6)
- Grouping (without capturing): <code>**(**_subexpr_**)**</code>
- Quantifiers:
    - Greedy: <code>**&#42;**</code>, <code>**+**</code>, <code>**?**</code>, <code>**{**_n_**}**</code>, <code>**{**_n_**,}**</code>, <code>**{**_n_**,**_m_**}**</code>
    - Lazy: <code>**&#42;?**</code>, <code>**+?**</code>, <code>**??**</code>, <code>**{**_n_**}?**</code>, <code>**{**_n_**,}?**</code>, <code>**{**_n_**,**_m_**}?**</code>
    <blockquote>The difference between _greedy_ and _lazy_ quantifiers is in how they control backtracking. _Greedy_ quantifiers will first try to match _as many_ characters as possible. Then, if the rest of the regex does not match, they will backtrack to matching one character _less_, then try again on the rest of the regex–and so on, one character _less_ every time. _Lazy_ quantifiers, on the other hand, will first try to match _as few_ characters as possible, then backtrack to matching one character _more_ every time.</blockquote>
- Alternation: **`|`**
- Anchors:
    - <code>**^**</code>: start of string or line (depending on the `Multiline` option)
    - <code>**&#36;**</code>: end of string or line (depending on the `Multiline` option)
    - **`\A`**: start of string only
    - **`\Z`**: end of string or before ending newline
    - **`\z`**: end of string only
    - **`\G`**: contiguous match (must start where previous match ended)
    - **`\b`**: word boundary
    - **`\B`**: non-word boundary
- Regex options (global):
    - `IgnoreCase`
    - `Multiline`: <code>**^**</code> and <code>**&#36;**</code> will match the beginning and end of each line (instead of the beginning and end of the input string)
    - `Singleline`: the period (<code>**.**</code>) will match every character (instead of every character except **`\n`**)

See also: [Missing Regex Features](#missing-regex-features).


### Architecture ###

_RegexParser_ has three layers, corresponding to three parsing phases:

1. Parsing the regex pattern, resulting in an [Abstract Syntax Tree][1] (AST)
2. Transforming the AST
3. Parsing the target string using the AST

Phases 1 and 2 happen only once for a given regex. Phase 3 may happen multiple times, for different target strings.

  [1]: http://en.wikipedia.org/wiki/Abstract_Syntax_Tree


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

> **NOTE**: The idea of _parser combinators_ came from these articles:

> - [Monadic Parsing in Haskell][2] (Hutton, Meijer) (1998)
> - [Parsec, a fast combinator parser][3] (Leijen) (2001)

> In the articles, the type is defined similarly to:

> `newtype Parser token tree = Parser ([token] -> [(tree, [token])])`

> This allows the parser to be ambiguous (able to parse a string in multiple ways). The parser will return either a list of one or more "success" alternatives, or an empty list to indicate failure.

> As the regex syntax is non-ambigious, the `Maybe` definition was preferred.

  [2]: https://github.com/aistrate/RegexParser/raw/master/Haskell/Monadic%20Parsing%20in%20Haskell%20(Hutton%2C%20Meijer%3B%201998).pdf
  [3]: https://github.com/aistrate/RegexParser/raw/master/Haskell/Parsec%2C%20a%20fast%20combinator%20parser%20(Leijen%3B%202001).pdf


### Parser Combinators in C# ###

The following parser combinators have been defined (see [source][4] for descriptions):

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

Beside combinators, there are also a number of "primitive" character parsers (see [source][5] for descriptions):

- `AnyChar`
- `Satisfy`
- `Char`
- `Digit`
- `OctDigit`
- `HexDigit`
- `OneOf`
- `NoneOf`

Each of these will match exactly _one_ character.

  [4]: https://github.com/aistrate/RegexParser/blob/master/ParserCombinators/Parsers.cs
  [5]: https://github.com/aistrate/RegexParser/blob/master/ParserCombinators/CharParsers.cs


### The Parser Monad in C# ###

[LINQ][6], the data querying subset of _C#_, offers a form of _syntactic sugar_ that allows writing code similar to the _Haskell_ `do` notation. This greatly simplifies the writing of more complex parsers.

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

Because this is a common pattern, we can define a helper [extension method][7], `Select()`:

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

Notice the similarity to the `do` notation in _Haskell_:

```Haskell
naturalNum = do ds <- many1 digit
                return (readInt ds)
```

So far, we could only use the `from` keyword _once_ per expression. By also defining a _LINQ_-related method called `SelectMany()` (see [source][8]), we become able to build parser expressions that use `from` more than once. For example, if we want to parse an integer (prefixed by an optional minus sign), we can write:

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

  [6]: http://en.wikipedia.org/wiki/Language_Integrated_Query
  [7]: http://en.wikipedia.org/wiki/Extension_method
  [8]: https://github.com/aistrate/RegexParser/blob/master/ParserCombinators/ParserMonad.cs


### Parsing the Regex Language ###

Using parser combinators and primitives, as well as _syntactic sugar_ notation as described above, we can write a parser for the whole regex language (as supported by _RegexParser_) in less than **150 lines** of code (see [source][9]).

For example, the `Quantifier` parser, which parses any of the forms <code>**&#42;**</code>, <code>**+**</code>, <code>**?**</code>, <code>**{**_n_**}**</code>, <code>**{**_n_**,}**</code>, <code>**{**_n_**,**_m_**}**</code> (_greedy_ quantifiers), and <code>**&#42;?**</code>, <code>**+?**</code>, <code>**??**</code>, <code>**{**_n_**}?**</code>, <code>**{**_n_**,}?**</code>, <code>**{**_n_**,**_m_**}?**</code> (_lazy_ quantifiers), is defined like this:

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

The more complex parsers are built from more simple ones. The topmost parser is called `Regex`. The result of parsing is a tree of _pattern_ objects (derived from class `BasePattern`). Here are the main pattern classes (see [sources][10]):

- `CharEscapePattern`
- `CharGroupPattern`, `CharRangePattern`, `CharClassSubtractPattern`, `AnyCharPattern` (dealing with character classes)
- `GroupPattern`
- `QuantifierPattern`
- `AlternationPattern`
- `AnchorPattern`

  [9]: https://github.com/aistrate/RegexParser/blob/master/RegexParser/Patterns/PatternParsers.cs
  [10]: https://github.com/aistrate/RegexParser/tree/master/RegexParser/Patterns


### Missing Regex Features ###

Still on the TODO list:

- Groups:
    - capturing groups
    - backreferences
    - named groups
    - atomic groups (non-backtracking)
- Substitution:
    - `Regex.Replace()` method
    - substitution patterns: <code>**&#36;&#36;**</code>, <code>**&#36;1**</code>, etc.
- Look-ahead
- Look-behind
- Regex options:
    - `IgnorePatternWhitespace`
    - `ExplicitCapture`
    - inline options (as opposed to global)
- Comments in patterns: <code>**(?#**_comment_**)**</code>
