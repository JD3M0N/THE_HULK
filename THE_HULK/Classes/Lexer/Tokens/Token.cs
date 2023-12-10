namespace THE_HULK;
/*
    Token class is the base class for all tokens.
    It contains the token kind and the token value.
    The token kind is the type of token it is.
    The token value is the value of the token.
    The token value is an object because it can be a string, double, bool or variable.
*/
public abstract class Token
{
    public TokenKind Kind { get; set; }

    public Token(TokenKind kind)
    {
        Kind = kind;
    }

    public TokenKind GetTokenKind() => Kind;
    
    public abstract string GetTokenName();

    public abstract object GetTokenValue();

    public override string ToString() => $"{Kind}";
}



