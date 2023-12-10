namespace THE_HULK;

/*
    This is the number expression.
    It only contains a value of type double.
*/
public class Number : BasicExpression
{
    public Number(double value) : base(null!)
    {
        this.value = value;
        Kind = ExpressionKind.Number;
    }

    public override ExpressionKind Kind { get; set; }

    public override object? value { get; set; }

    public override void Evaluate(Environment _environment) { return; }

    public override object? GetValue() => value;

    public override string ToString() => $"{Kind.GetType} : {value}";
}