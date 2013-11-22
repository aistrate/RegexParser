{-
    Parsec non-determinism tests
-}
import Text.ParserCombinators.Parsec


patternA1 = do x <- (string "a" <|> string "ab")
               y <- string "bbbc"
               return $ x ++ y

patternA2 = do x <- (string "ab" <|> string "a")
               y <- string "bbbc"
               return $ x ++ y

testAltern = do parseTest patternA1 "abbbc"
                parseTest patternA2 "abbbbc"
                parseTest patternA1 "abbbbc"
                parseTest patternA2 "abbbc"


patternB1 = string "abc" <|> string "xyz"

patternB2 = string "xbc" <|> string "xyz"

patternB3 = string "xyc" <|> string "xyz"

patternB4 = try (string "xyc") <|> string "xyz"

testPredictive = do parseTest patternB1 "xyz"
                    parseTest patternB2 "xyz"
                    parseTest patternB3 "xyz"
                    parseTest patternB4 "xyz"


patternC1 = do x <- ((try (string "a")) <|> string "ab")
               y <- string "bbbc"
               return $ x ++ y

patternC2 = do x <- ((try (string "ab")) <|> string "a")
               y <- string "bbbc"
               return $ x ++ y

testAltern2 = do parseTest patternC1 "abbbc"
                 parseTest patternC2 "abbbbc"
                 parseTest patternC1 "abbbbc"
                 parseTest patternC2 "abbbc"
