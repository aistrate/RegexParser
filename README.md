RegexParser
============

### Contents ###

<toc>

- [Implemented Regex Features](https://github.com/aistrate/RegexParser/blob/master/Doc/ImplementedRegexFeatures.md#implemented-regex-features)
    - [Missing Features](https://github.com/aistrate/RegexParser/blob/master/Doc/ImplementedRegexFeatures.md#missing-features)
- [How _RegexParser_ Works](https://github.com/aistrate/RegexParser/blob/master/Doc/HowRegexParserWorks.md#how-regexparser-works)
    - [Phase 1: Parsing the Regex Language](https://github.com/aistrate/RegexParser/blob/master/Doc/HowRegexParserWorks.md#phase-1-parsing-the-regex-language)
        - [The Parser Type](https://github.com/aistrate/RegexParser/blob/master/Doc/HowRegexParserWorks.md#the-parser-type)
        - [Parser Combinators in C#](https://github.com/aistrate/RegexParser/blob/master/Doc/HowRegexParserWorks.md#parser-combinators-in-c)
        - [The Parser Monad in C#](https://github.com/aistrate/RegexParser/blob/master/Doc/HowRegexParserWorks.md#the-parser-monad-in-c)
        - [Parsing the Regex Language](https://github.com/aistrate/RegexParser/blob/master/Doc/HowRegexParserWorks.md#parsing-the-regex-language)
    - [Phase 2: Transforming the _Abstract Syntax Tree_ (_AST_)](https://github.com/aistrate/RegexParser/blob/master/Doc/HowRegexParserWorks.md#phase-2-transforming-the-abstract-syntax-tree-ast)
    - [Phase 3: Parsing the Target String](https://github.com/aistrate/RegexParser/blob/master/Doc/HowRegexParserWorks.md#phase-3-parsing-the-target-string)
        - [Matching without Backtracking](https://github.com/aistrate/RegexParser/blob/master/Doc/HowRegexParserWorks.md#matching-without-backtracking)
        - [The Need for Backtracking](https://github.com/aistrate/RegexParser/blob/master/Doc/HowRegexParserWorks.md#the-need-for-backtracking)

</toc>
