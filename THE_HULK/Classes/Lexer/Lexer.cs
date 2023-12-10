namespace THE_HULK;

#region Lexical_Analyzer_Explanation

/*
    A Lexer, short for lexical analyzer, is a fundamental component of a compiler or interpreter. 
    Its main purpose is to break down the source code into smaller units called tokens. 
    Tokens are the building blocks of a programming language and represent meaningful elements 
    such as keywords, identifiers, operators, literals, and punctuation.

    The Lexer scans the source code character by character and groups them into tokens
    based on predefined rules and patterns.
    It identifies the lexical structure of the code, ignoring whitespace and comments, 
    and produces a stream of tokens that can be further processed by the parser.

    In the provided code excerpt, the Lexer class represents the implementation of a Lexer. 
    It takes the source code as input and provides a Tokenize method to tokenize the code. 
    The Tokenize method uses a loop to iterate over each character of the source code and identifies 
    the current character as a token.
*/

#endregion
public class Lexer
{

    public readonly string imput;
    private int currentPos;

    // ===>>> Constructor <<<===
    public Lexer(string input)
    {
        this.imput = input;
        currentPos = 0;
    }

    // ===>>> Tokenizer <<<===
    /*

    The Tokenize method uses a loop to iterate over each character of the source code and identifies
    the current character as a token.

    */
    public List<Token> Tokenizer()
    {
        // List of tokens
        List<Token> tokens = new();

        while (currentPos < imput.Length)
        {
            // Current character
            char currentChar = imput[currentPos];

            // Ignore whitespace
            if (char.IsWhiteSpace(currentChar))
            {
                currentPos++;
                continue;
            }

            // Add identifier or keyword
            else if (IsLetter(currentChar))
            {
                tokens.Add(UnkwnonType());
            }

            // Add string
            else if (currentChar == '"')
            {
                tokens.Add(String());
            }

            // Add number
            else if (char.IsDigit(currentChar))
            {
                tokens.Add(Number());
            }

            // Add operator
            else if (IsOperator(currentChar))
            {
                tokens.Add(Operator());
            }

            // Add punctuator mark
            else if (IsMark(currentChar))
            {
                tokens.Add(Puntuation_Marks());
            }

            // Add unknown token
            else
            {
                tokens.Add(new TokenCommon(TokenKind.Unknown, currentChar.ToString()));
                Console.WriteLine($"!lexical error: \"{tokens.Last()}\" is not a valid token.");
                currentPos++;
            }
        }

        // Add EOF token
        if (tokens.Last().GetTokenName() != ";")
        {
            Console.WriteLine("!syntax error: expression must end with \";\".");
            throw new Exception();
        }

        return tokens;
    }

    // ===>>> Tokenizer Helpers <<<===

    /*
        The Number() function is used to identify a number token.
        And return a TokenData object with the token kind and the value of the number.
    */
    private Token Number()
    {

        string number = "";

        while ((currentPos < imput.Length) && (char.IsDigit(imput[currentPos]) || imput[currentPos] == '.'))
        {
            number += imput[currentPos];

            if (IsLetter(LookAhead(1)))
            {
                Console.WriteLine($"Lexical Error!: \"{number + LookAhead(1)}\" isn't a valid token.");
                throw new Exception();
            }

            currentPos++;
        }

        return new TokenData(TokenKind.Number, Double.Parse(number));
    }

    /*
        The UnkwnonType() function is used to identify an identifier or keyword token.
        And return a Token object with the token kind and the value of the identifier or keyword.
    */
    private Token UnkwnonType()
    {

        string unkwnon = "";

        while (currentPos < imput.Length && (IsLetterOrDigit(imput[currentPos]) || imput[currentPos] == '_'))
        {
            unkwnon += imput[currentPos];
            currentPos++;
        }

        if (IsKeyWord(unkwnon))
        {
            return KeyWord(unkwnon);
        }

        else
        {
            return new TokenCommon(TokenKind.Identifier, unkwnon);
        }
    }

    /*
        The String() function is used to identify a string token.
        And return a TokenData object with the token kind and the value of the string.
    */
    private Token String()
    {
        currentPos++;
        string temporal = "";

        while (currentPos < imput.Length && imput[currentPos] != '"')
        {
            temporal += imput[currentPos];
            currentPos++;
        }

        Next();
        return new TokenData(TokenKind.String, temporal);
    }

    /*
        The Operator() function is used to identify an operator token.
        And return a Token object with the token kind and the value of the operator.
    */
    private Token Operator()
    {
        char _operator = imput[currentPos];
        switch (_operator)
        {
            case '+':
                Next();
                return new TokenCommon(TokenKind.Sum, _operator.ToString());
            case '-':
                Next();
                return new TokenCommon(TokenKind.Difference, _operator.ToString());
            case '*':
                Next();
                return new TokenCommon(TokenKind.Product, _operator.ToString());
            case '^':
                Next();
                return new TokenCommon(TokenKind.Power, _operator.ToString());
            case '/':
                Next();
                return new TokenCommon(TokenKind.Quotient, _operator.ToString());
            case '%':
                Next();
                return new TokenCommon(TokenKind.Modulo, _operator.ToString());
            case '@':
                Next();
                return new TokenCommon(TokenKind.Concatenation, _operator.ToString());
            case '!':
                if (LookAhead(1) == '=')
                {
                    Next(2);
                    return new TokenCommon(TokenKind.UnEqual, "!=");
                }
                else
                {
                    Next();
                    return new TokenCommon(TokenKind.Not, _operator.ToString());
                }
            case '<':
                if (LookAhead(1) == '=')
                {
                    Next(2);
                    return new TokenCommon(TokenKind.LessOrEqualThan, "<=");
                }
                else
                {
                    Next();
                    return new TokenCommon(TokenKind.LessThan, _operator.ToString());
                }
            case '>':
                if (LookAhead(1) == '=')
                {
                    Next(2);
                    return new TokenCommon(TokenKind.GreaterOrEqualThan, ">=");
                }
                else
                {
                    Next();
                    return new TokenCommon(TokenKind.GreaterThan, _operator.ToString());
                }
            case '=':
                if (LookAhead(1) == '=')
                {
                    Next(2);
                    return new TokenCommon(TokenKind.EqualTo, "==");
                }
                else if (LookAhead(1) == '>')
                {
                    Next(2);
                    return new TokenCommon(TokenKind.Arrow, "=>");
                }
                else
                {
                    Next();
                    return new TokenCommon(TokenKind.EqualEqual, _operator.ToString());
                }
            case '&':
                Next();
                return new TokenCommon(TokenKind.And, _operator.ToString());
            default:
                Next();
                return new TokenCommon(TokenKind.Or, _operator.ToString());
        }
    }

    /*
        The Puntuation_Marks() function is used to identify a punctuation mark token.
        And return a Token object with the token kind and the value of the punctuation mark.
    */
    private Token Puntuation_Marks()
    {
        char punctuator = imput[currentPos];
        switch (punctuator)
        {
            case '"':
                Next();
                return new TokenCommon(TokenKind.Quote, punctuator.ToString());
            case '(':
                Next();
                return new TokenCommon(TokenKind.LeftParenthesis, punctuator.ToString());
            case ')':
                Next();
                return new TokenCommon(TokenKind.RightParenthesis, punctuator.ToString());
            case ';':
                Next();
                return new TokenCommon(TokenKind.Semicolon, punctuator.ToString());
            case ',':
                Next();
                return new TokenCommon(TokenKind.Comma, punctuator.ToString());
            case ':':
                Next();
                return new TokenCommon(TokenKind.Colon, punctuator.ToString());
            default:
                Next();
                return new TokenCommon(TokenKind.End, punctuator.ToString());
        }
    }

    /*
        The KeyWord() function is used to identify a keyword token.
        And return a Token object with the token kind and the value of the keyword.
    */
    private Token KeyWord(string keywordToIdentify)
    {
        switch (keywordToIdentify)
        {
            case "true":
                return new TokenData(TokenKind.TrueKeyWord, true);
            case "false":
                return new TokenData(TokenKind.FalseKeyWord, false);
            case "function":
                return new TokenKeyword(TokenKind.FunctionKeyWord);
            case "let":
                return new TokenKeyword(TokenKind.LetKeyWord);
            case "in":
                return new TokenKeyword(TokenKind.InKeyWord);
            case "if":
                return new TokenKeyword(TokenKind.IfKeyWord);
            default:
                return new TokenKeyword(TokenKind.ElseKeyWord);
        }
    }


    // ===>>> Helpers <<<===

    /*
        The Next() function is used to move the current position to the next character.
    */
    private void Next()
    {
        currentPos++;
    }

    /*
        The Next() is and overload function of the previous one.
        This one is used to move the current position to the next input characters
    */
    private void Next(int positions)
    {
        currentPos += positions;
    }

    /*
        The LookAhead() function is used to look ahead the next character.
    */
    private char LookAhead(int positions)
    {
        if ((currentPos + positions) >= imput.Length)
        {
            return ' ';
        }

        return imput[currentPos + positions];
    }

    /*
        The IsLetter() function is used to identify if the current character is a letter.
    */
    private static bool IsLetter(char c) => char.IsLetter(c) || c == '_';

    /*
        The IsLetterOrDigit() function is used to identify if the current character is a letter or digit.
    */
    private static bool IsLetterOrDigit(char c) => char.IsLetterOrDigit(c) || c == '_';

    /*
        The IsOperator() function is used to identify if the current character is an operator.
    */
    private static bool IsOperator(char currentChar)
    {
        List<char> operators = new()
            {
                '+',
                '-',
                '=',
                '*',
                '<',
                '/',
                '^',
                '&',
                '%',
                '!',
                '@',
                '>',
                '|'
            };

        return operators.Contains(currentChar);
    }

    /*
        The IsMark() function is used to identify if the current character is a punctuation mark.
    */
    private static bool IsMark(char currentChar)
    {
        List<char> puntuation_marks = new()
            {
                '.',
                ';',
                '(',
                ')',
                ',',
                '"'
            };

        return puntuation_marks.Contains(currentChar);
    }

    /*
        The IsKeyWord() function is used to identify if the current character is a keyword.
    */
    private static bool IsKeyWord(string keyword)
    {
        List<string> keywords = new()
            {
                "if",
                "in" , 
                "let",
                "true",
                "else",
                "false",
                "function"
            };

        return keywords.Contains(keyword);
    }
}