namespace THE_HULK;

/*
    This are the operator and the symbols 
    which are not data types or keywords.
*/
public class TokenCommon : Token
{
    public string symbol { get; set; }

    public TokenCommon(TokenKind kind, string _symbol) : base(kind)
    {
        symbol = _symbol;
    }
    public override string GetTokenName() => symbol;

    public override object GetTokenValue() => throw new NotImplementedException();

    public override string ToString() => $"{base.Kind}: {symbol}";
}