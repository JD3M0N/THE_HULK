namespace THE_HULK;

/*
    These are the keywords.
*/
public class TokenKeyword : Token
{
    public TokenKeyword(TokenKind kind) : base(kind)
    {
        Kind = kind;
    }

    public override string GetTokenName() => Kind.ToString();

    public override object GetTokenValue() => throw new NotImplementedException();

}