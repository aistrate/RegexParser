Regex Parser
============

[TOC]

### Implemented Regex Constructs ###

- Character escapes
- Character classes
- Grouping (no capturing): __`(`__*subexpr*__`)`__
- Quantifiers:
    - greedy: __`*`__, __`+`__, __`?`__, __`{`__*n*__`}`__, __`{`__*n*__`,}`__, __`{`__*n*__`,`__*m*__`}`__
    - non-greedy: __`*?`__, __`+?`__, __`??`__, __`{`__*n*__`}?`__, __`{`__*n*__`,}?`__, __`{`__*n*__`,`__*m*__`}?`__
- Alternation: __`|`__
- Anchors:
    - start of string or line (depending on `Multiline` option): __`^`__
    - end of string or line (depending on `Multiline` option): __`$`__
    - start of string only: __`\A`__
    - end of string or before ending newline: __`\Z`__
    - end of string only: __`\z`__
    - contiguous match (must start where previous match ended): __`\G`__
    - word boundary: __`\b`__
    - non-word boundary: __`\B`__
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
    - substitution patterns: __`$$`__, __`$1`__, etc,
- Look-ahead
- Look-behind
- Regex options:
    - `IgnorePatternWhitespace`
    - `ExplicitCapture`
    - inline options (as opposed to global)
- Comments in patterns: __`(?#`__*comment*__`)`__
