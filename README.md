Regex Parser
============

### Implemented Regex Constructs ###

- Character escapes
- Character classes
- Grouping (no capture): <code>__(__*subexpr*__)__</code>
- Quantifiers:
    - greedy: __`*`__, __`+`__, __`?`__, <code>__{__*n*__}__</code>, <code>__{__*n*__,}__</code>, <code>__{__*n*__,__*m*__}__</code>
    - non-greedy: __`*?`__, __`+?`__, __`??`__, <code>__{__*n*__}?__</code>, <code>__{__*n*__,}?__</code>, <code>__{__*n*__,__*m*__}?__</code>
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
- Comments in patterns: <code>__(?#__*comment*__)__</code>
