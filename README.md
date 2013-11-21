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

The Regex Parser library has three layers, each defined by a parsing phase:

1. Parsing the regex pattern, which produces an [Abstract Syntax Tree][1] (AST)
2. Transforming  the AST
3. Parsing the target string using the AST

Phases 1 and 2 will happen only once for a given regex. Phase 3 can happen multiple times, on different target strings.

  [1]: http://en.wikipedia.org/wiki/Abstract_Syntax_Tree


### The Parser Type ###

The `Parser` type emulates the following _Haskell_ type:

```Haskell
newtype Parser s t = P ([s] -> (t, [s]))
```

In _C#_, it is defined like this:

```C#
public delegate Result<TToken, TValue> Parser<TToken, TValue>(IConsList<TToken> consList);

public class Result<TToken, TValue>
{
    public Result(TValue value, IConsList<TToken> rest)
    {
        Value = value;
        Rest = rest;
    }
    public TValue Value { get; private set; }
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
