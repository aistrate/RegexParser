Regex Parser
============

[TOC]

### Implemented Regex Constructs ###

- Character escapes
- Character classes
- Grouping (no capturing): **`(`***subexpr***`)`**
- Quantifiers:
    - greedy: **`*`**, **`+`**, **`?`**, **`{`***n***`}`**, **`{`***n***`,}`**, **`{`***n***`,`***m***`}`**
    - non-greedy: **`*?`**, **`+?`**, **`??`**, **`{`***n***`}?`**, **`{`***n***`,}?`**, **`{`***n***`,`***m***`}?`**
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


<a id="missing-features"></a>
### Missing Features ###

- Groups:
    - capturing groups
    - backreferences
    - named groups
    - atomic groups (non-backtracking)
- Substitution:
    - `Regex.Replace()` method
    - substitution patterns: **`$$`**, **`$1`**, etc,
- Look-ahead
- Look-behind
- Regex options:
    - `IgnorePatternWhitespace`
    - `ExplicitCapture`
    - inline options (as opposed to global)
- Comments in patterns: **`(?#`***comment***`)`**
