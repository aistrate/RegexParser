RegexParser
============

_RegexParser_ is a regular expression engine that follows functional programming principles: parser combinators, purely functional data structures, and side-effect free code.


<toc>

- [Implemented Regex Features](/Doc/ImplementedRegexFeatures.md#implemented-regex-features)
    - [Missing Features](/Doc/ImplementedRegexFeatures.md#missing-features)
- [How _RegexParser_ Works](/Doc/HowRegexParserWorks.md#how-regexparser-works)
    - [Phase 1: Parsing the Regex Pattern](/Doc/HowRegexParserWorks.md#phase-1-parsing-the-regex-pattern)
        - [The Parser Type](/Doc/HowRegexParserWorks.md#the-parser-type)
        - [Parser Combinators in C#](/Doc/HowRegexParserWorks.md#parser-combinators-in-c)
        - [The Parser Monad in C#](/Doc/HowRegexParserWorks.md#the-parser-monad-in-c)
        - [Parsing the Regex Language](/Doc/HowRegexParserWorks.md#parsing-the-regex-language)
    - [Phase 2: Transforming the _Abstract Syntax Tree_](/Doc/HowRegexParserWorks.md#phase-2-transforming-the-abstract-syntax-tree)
    - [Phase 3: Pattern Matching on the Target String](/Doc/HowRegexParserWorks.md#phase-3-pattern-matching-on-the-target-string)
        - [Matching without Backtracking](/Doc/HowRegexParserWorks.md#matching-without-backtracking)
        - [The Need for Backtracking](/Doc/HowRegexParserWorks.md#the-need-for-backtracking)

</toc>
