RegexParser
============

### Contents ###

<toc>

- [Implemented Regex Features](Doc/ImplementedRegexFeatures.md#implemented-regex-features)
    - [Missing Features](Doc/ImplementedRegexFeatures.md#missing-features)
- [How _RegexParser_ Works](Doc/HowRegexParserWorks.md#how-regexparser-works)
    - [Phase 1: Parsing the Regex Language](Doc/HowRegexParserWorks.md#phase-1-parsing-the-regex-language)
        - [The Parser Type](Doc/HowRegexParserWorks.md#the-parser-type)
        - [Parser Combinators in C#](Doc/HowRegexParserWorks.md#parser-combinators-in-c)
        - [The Parser Monad in C#](Doc/HowRegexParserWorks.md#the-parser-monad-in-c)
        - [Parsing the Regex Language](Doc/HowRegexParserWorks.md#parsing-the-regex-language)
    - [Phase 2: Transforming the _Abstract Syntax Tree_ (_AST_)](Doc/HowRegexParserWorks.md#phase-2-transforming-the-abstract-syntax-tree-ast)
    - [Phase 3: Parsing the Target String](Doc/HowRegexParserWorks.md#phase-3-parsing-the-target-string)
        - [Matching without Backtracking](Doc/HowRegexParserWorks.md#matching-without-backtracking)
        - [The Need for Backtracking](Doc/HowRegexParserWorks.md#the-need-for-backtracking)

</toc>
