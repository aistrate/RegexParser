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
    - backslash (**`\`**) followed by a character not recognized as escaped (including any of **<code>.&#36;^{&#91;(|)&#42;+?&#92;</code>**) matches that character
- Character classes:
    - <code>**.**</code> matches any character except <code>**\n**</code> (or, if the `Singleline` option is on, any character _including_ <code>**\n**</code>)
    - positive character groups (e.g., <code>**&#91;aeiou&#93;**</code>, <code>**&#91;a-zA-Z&#93;**</code>, <code>**&#91;abcA-H\d\n&#93;**</code>)
    - negative character groups (e.g., <code>**&#91;^a-zA-Z&#93;**</code>)
    - named character classes:
        - **`\w`**: a word character; same as <code>**&#91;0-9A-Z&#95;a-z&#93;**</code>
        - **`\W`**: a non-word character; same as <code>**&#91;^0-9A-Z&#95;a-z&#93;**</code>
        - **`\s`**: a whitespace character; same as <code>**&#91; \n\r\t&#93;**</code>
        - **`\S`**: a non-whitespace character; same as <code>**&#91;^ \n\r\t&#93;**</code>
        - **`\d`**: a digit character; same as <code>**&#91;0-9&#93;**</code>
        - **`\D`**: a non-digit character; same as <code>**&#91;^0-9&#93;**</code>
    - character class subtraction (e.g., <code>**&#91;0-9-&#91;246&#93;&#93;**</code> matches any digit except for 2, 4, and 6)
- Grouping (without capturing): <code>**(**_subexpr_**)**</code>
- Quantifiers:
    - Greedy: <code>**&#42;**</code>, <code>**+**</code>, <code>**?**</code>, <code>**{**_n_**}**</code>, <code>**{**_n_**,}**</code>, <code>**{**_n_**,**_m_**}**</code>
    - Lazy: <code>**&#42;?**</code>, <code>**+?**</code>, <code>**??**</code>, <code>**{**_n_**}?**</code>, <code>**{**_n_**,}?**</code>, <code>**{**_n_**,**_m_**}?**</code>
    <blockquote>The difference between greedy and lazy quantifiers is in how they control backtracking. _Greedy quantifiers_ will first try to match as _many_ characters as possible. Then, if the rest of the regex does not match, they will backtrack to matching one character _less_, then try again on the rest of the regex--and so on, one character _less_ every time. _Lazy quantifiers_, on the other hand, will first try to match as _few_ characters as possible, then backtrack to matching one character _more_ every time.</blockquote>
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

The Regex Parser has three layers, corresponding to the three parsing phases:

1. Parsing the regex pattern, which produces an [Abstract Syntax Tree][1] (AST)
2. Transforming  the AST
3. Parsing the target string using the AST

Phases 1 and 2 happen only once for a given regex. Phase 3 may happen multiple times, for different target strings.

  [1]: http://en.wikipedia.org/wiki/Abstract_Syntax_Tree


### The Parser Type ###

The `Parser` type is defined like this:

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

A parser is a function that takes a list of tokens (e.g., characters), and returns a syntax tree and the list of unconsumed tokens. To indicate failure to match, it returns `null`.

The `Parser` type emulates the following _Haskell_ type:

```Haskell
newtype Parser token tree = Parser ([token] -> Maybe (tree, [token]))
```

> **NOTE**: The idea (and syntax) of parser combinators came from these articles (_Haskell_):

> - [Combinator Parsing: A Short Tutorial][2] (Swierstra) (2008)
> - [Monadic Parsing in Haskell][3] (Hutton, Meijer) (1998)

> In the articles, the type is defined:

> `newtype Parser token tree = Parser ([token] -> [(tree, [token])])`

> This allows the parser to be ambiguous (there may be multiple ways to parse a string). It returns either a list of one or more “successes”, or an empty list to indicate failure.

> The regex syntax is non-ambigious, so the first definition was preferred.

  [2]: https://github.com/aistrate/RegexParser/raw/master/Haskell/Combinator%20Parsing%20-%20A%20Short%20Tutorial%20(Swierstra%3B%202008).pdf
  [3]: https://github.com/aistrate/RegexParser/raw/master/Haskell/Monadic%20Parsing%20in%20Haskell%20(Hutton%2C%20Meijer%3B%201998).pdf


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
