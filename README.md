Regex Parser
============

### Implemented Regex Features ###

- Character escapes
- Character classes
- Grouping (without capturing): <code>**(**_subexpr_**)**</code>
- Quantifiers:
    - Greedy: <code>**\***</code>, <code>**+**</code>, <code>**?**</code>, <code>**{**_n_**}**</code>, <code>**{**_n_**,}**</code>, <code>**{**_n_**,**_m_**}**</code>
    - Lazy: <code>**\*?**</code>, <code>**+?**</code>, <code>**??**</code>, <code>**{**_n_**}?**</code>, <code>**{**_n_**,}?**</code>, <code>**{**_n_**,**_m_**}?**</code>
> The difference between greedy and lazy quantifiers is in the way they control backtracking. _Greedy_ quantifiers will first try to match as _many_ characters as possible; then, if the rest of the Regex does not match, will backtrack to one character _less_ and try again the rest; and so on, one character _less_ every time. _Lazy_ quantifiers, on the other hand, will first try to match as _few_ characters as possible, then backtrack to matching one character _more_ every time.
- Alternation: **`|`**
- Anchors:
    - **`^`**: start of string or line (depending on the `Multiline` option)
    - **`$`**: end of string or line (depending on the `Multiline` option)
    - **`\A`**: start of string only
    - **`\Z`**: end of string or before ending newline
    - **`\z`**: end of string only
    - **`\G`**: contiguous match (must start where previous match ended)
    - **`\b`**: word boundary
    - **`\B`**: non-word boundary
- Regex options (global):
    - `IgnoreCase`
    - `Multiline`: **`^`** and **`$`** match the beginning and end of each line (instead of the beginning and end of the input string)
    - `Singleline`: the period (**`.`**) matches every character (instead of every character except **`\n`**)

See also: [Missing Features](#missing-features).


### Missing Features ###

- Groups:
    - capturing groups
    - backreferences
    - named groups
    - atomic groups (non-backtracking)
- Substitution:
    - `Regex.Replace()` method
    - substitution patterns: <code>**$$**</code>, <code>**$1**</code>, etc,
- Look-ahead
- Look-behind
- Regex options:
    - `IgnorePatternWhitespace`
    - `ExplicitCapture`
    - inline options (as opposed to global)
- Comments in patterns: <code>**(?#**_comment_**)**</code>
