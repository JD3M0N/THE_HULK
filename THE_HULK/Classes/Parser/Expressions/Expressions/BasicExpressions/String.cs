namespace THE_HULK;

/*
    This is the string expression.
    It only contains a value of type string.
*/
public class String : BasicExpression
{
    public String(string value) : base(null!)
    {
        this.value = value;
        Kind = ExpressionKind.String;
    }

    public override ExpressionKind Kind { get => ExpressionKind.String; set { } }
    public override object? value { get; set; }

    public override void Evaluate(Environment _environment) { return; }

    public override object? GetValue() => value;
}