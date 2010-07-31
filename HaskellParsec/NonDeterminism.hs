import Text.ParserCombinators.Parsec


pattern1 = do x <- (string "a" <|> string "ab")
              y <- string "bbbc"
              return $ x ++ y

pattern2 = do x <- (string "ab" <|> string "a")
              y <- string "bbbc"
              return $ x ++ y


main = do parseTest pattern1 "abbbc"
          parseTest pattern2 "abbbbc"
          parseTest pattern1 "abbbbc"
          parseTest pattern2 "abbbc"
