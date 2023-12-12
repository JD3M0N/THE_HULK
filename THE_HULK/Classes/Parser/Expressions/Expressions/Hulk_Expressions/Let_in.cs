using System.Globalization;

namespace THE_HULK;

public class Let_In : Expression
{
    public override Environment? environment { get; set; }
    public Expression parameter;
    public override ExpressionKind Kind { get; set; }
    public Dictionary<string, Expression> Variables { get; set; }
    public override object? value { get; set; }

    public Let_In(Expression _parameter, Environment _environment) : base(_environment)
    {
        Variables = new Dictionary<string, Expression>();
        Kind = ExpressionKind.Temp;
        parameter = _parameter;
    }

    public override void Evaluate(Environment _environment)
    {
        parameter.Evaluate(environment!);
        value = parameter.GetValue();

        //make sure the type of element u are using
            //! SEMANTIC ERROR: "Sum" cannot be used between "Number" and "Temp".
        if (value is bool) { Kind = ExpressionKind.Bool; } 
        if (value is string) { Kind = ExpressionKind.String; }
        if (value is double) { Kind = ExpressionKind.Number; }
    }

    public override object? GetValue() => value;
}