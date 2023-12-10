namespace THE_HULK;

/*
    TokenKind is the type of token it is.
    Using the enum is the easiest way to identify the token.
*/
public enum TokenKind
{
    Identifier, // Variables

    // ===>>> KeyWords <<<===
    IfKeyWord, // if
    InKeyWord, // in
    LetKeyWord, // let
    ElseKeyWord, // else
    FunctionKeyWord, // function
    // ===>>> End of KeyWords <<<===

    // ===>>> Data Types <<<===
    String, // "string"
    Number, // 1.0
    TrueKeyWord, // true
    FalseKeyWord, // false
    // ===>>> End of Data Types <<<===

    // ===>>> Numeric Operators <<<===
    Sum, // +
    Power, // ^
    Modulo, // %
    Product, // *
    Quotient, // /
    Difference, // -
    // ===>>> End of Numeric Operators <<<===

    // ===>>> Logical Operators <<<===
    Or, // ||
    Not, // !
    And, // &&
    UnEqual, // !=
    EqualTo, // ==
    LessThan, // <
    EqualEqual, // ==
    GreaterThan, // >
    Concatenation, // @
    LessOrEqualThan, // <=
    GreaterOrEqualThan, // >=
    // ===>>> End of Logical Operators <<<===

    // ===>>> Symbols <<<===
    End, // ;
    Arrow, // >
    Quote, // "
    Comma, // ,
    Colon, // :
    Semicolon, // ;
    LeftParenthesis, // (
    RightParenthesis, // )
    // ===>>> End of Symbols <<<===

    // ===>>> Comments <<<===
    EndOfFile, // End of File \n
    Unknown, // Unknown
    // ===>>> End of Comments <<<===

}