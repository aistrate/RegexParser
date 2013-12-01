import Text.ParserCombinators.ReadP
import Data.Char (isDigit)

digit = satisfy isDigit

readInt :: String -> Int
readInt = read


naturalNum = do ds <- many1 digit
                return (readInt ds)

integerNum = do sign <- option '+' (char '-')
                ds <- many1 digit
                let s = if sign == '-' then -1 else 1
                return (s * (readInt ds))


main = do let pos = "123ab"
              neg = "-5678ab"
              bad = "x123ab"
          let rn1 = (readP_to_S naturalNum) pos
              rn2 = (readP_to_S naturalNum) bad
          print $ last rn1
          print rn2
          let ri1 = (readP_to_S integerNum) pos
              ri2 = (readP_to_S integerNum) neg
              ri3 = (readP_to_S integerNum) bad
          print $ last ri1
          print $ last ri2
          print ri3
