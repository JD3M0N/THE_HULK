using System.Globalization;

namespace THE_HULK;

public class Let_In : Expression
{
    public override Environment? environment { get; set; }
    public Expression argument;
    public override ExpressionKind Kind { get; set; }
    public Dictionary<string, Expression> Variables { get; set; }
    public override object? value { get; set; }

    public Let_In(Expression _argument, Environment _environment) : base(_environment)
    {
        Variables = new Dictionary<string, Expression>();
        Kind = ExpressionKind.Temp;
        argument = _argument;
    }

    public override void Evaluate(Environment _environment)
    {
        argument.Evaluate(environment!);
        value = argument.GetValue();

        if (value is bool) { Kind = ExpressionKind.Bool; } 
        if (value is string) { Kind = ExpressionKind.String; }
        if (value is double) { Kind = ExpressionKind.Number; }
    }

    public override object? GetValue() => value;
}