Regex Parser
============

### Implemented Regex Constructs ###

- Character escapes
- Character classes
- Grouping (no capture): <code>**(**_subexpr_**)**</code>
- Quantifiers:
    - greedy: <code>__*__</code>, <code>**+**</code>, <code>**?**</code>, <code>**{**_n_**}**</code>, <code>**{**_n_**,}**</code>, <code>**{**_n_**,**_m_**}**</code>
    - non-greedy: <code>__*?__</code>, <code>**+?**</code>, <code>**??**</code>, <code>**{**_n_**}?**</code>, <code>**{**_n_**,}?**</code>, <code>**{**_n_**,**_m_**}?**</code>
- Alternation: **`|`**
- Anchors:
    - start of string or line (depending on `Multiline` option): **`^`**
    - end of string or line (depending on `Multiline` option): **`$`**
    - start of string only: **`\A`**
    - end of string or before ending newline: **`\Z`**
    - end of string only: **`\z`**
    - contiguous match (must start where previous match ended): **`\G`**
    - word boundary: **`\b`**
    - non-word boundary: **`\B`**
- Regex options:
    - `IgnoreCase`
    - `Multiline`
    - `Singleline`

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
