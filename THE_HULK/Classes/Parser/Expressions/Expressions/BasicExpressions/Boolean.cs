namespace THE_HULK;

/*
    This is the boolean expression.
    It only contains a value true or false.
*/
public class Boolean : BasicExpression
{
    public Boolean(bool value) : base(null!)
    {
        this.value = value;
        Kind = ExpressionKind.Bool;
    }

    public override ExpressionKind Kind { get => ExpressionKind.Bool; set { } }
    public override object? value { get; set; }

    public override void Evaluate(Environment _environment) { return; }

    public override object? GetValue() => value;
}