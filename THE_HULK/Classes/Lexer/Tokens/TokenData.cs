namespace THE_HULK;

/*
    These are the numbers, string, boolean and variables.
*/

public class TokenData : Token
{
    public object value { get; set; }

    public TokenData(TokenKind kind, object _value) : base(kind)
    {
        this.value = _value;
    }

    public override string GetTokenName() => value.ToString()!;

    public override object GetTokenValue() => value;

    public override string ToString() => $"{base.Kind}: {value}";
}