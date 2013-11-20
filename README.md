Regex Parser
============

### Implemented Regex Features ###

- Character escapes:
    - any character except one of **<code>.&#36;^{&#91;(|)&#42;+?&#92;</code>**: matches itself
    - **`\n`**: new line
    - **`\r`**: carriage return
    - **`\t`**: tab
    - **`\b`**: backspace (only inside a character class)
    - <code><strong>&#92;</strong>_nnn_</code>: ASCII character, where _`nnn`_ is a two- or three-digit octal character code
    - <code><strong>\x</strong>_nn_</code>: ASCII character, where _`nn`_ is a two-digit hexadecimal character code
    - <code><strong>\u</strong>_nnnn_</code>: UTF-16 code unit whose value is _`nnnn`_ hexadecimal
    - **`\`** followed by character not recognized as escaped (including **<code>.&#36;^{&#91;(|)&#42;+?&#92;</code>**): matches the character
- Character classes
- Grouping (without capturing): <code>**(**_subexpr_**)**</code>
- Quantifiers:
    - Greedy: <code>**&#42;**</code>, <code>**+**</code>, <code>**?**</code>, <code>**{**_n_**}**</code>, <code>**{**_n_**,}**</code>, <code>**{**_n_**,**_m_**}**</code>
    - Lazy: <code>**&#42;?**</code>, <code>**+?**</code>, <code>**??**</code>, <code>**{**_n_**}?**</code>, <code>**{**_n_**,}?**</code>, <code>**{**_n_**,**_m_**}?**</code>
    <blockquote>The difference between greedy and lazy quantifiers is in how they control backtracking. _Greedy_ quantifiers will first try to match as _many_ characters as possible. Then, if the rest of the Regex does not match, they will backtrack to one character _less_, then try again the rest--and so on, one character _less_ every time. _Lazy_ quantifiers, on the other hand, will first try to match as _few_ characters as possible, then backtrack to matching one character _more_ every time.</blockquote>
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

See also: [Missing Features](#missing-features).


### Missing Features ###

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
