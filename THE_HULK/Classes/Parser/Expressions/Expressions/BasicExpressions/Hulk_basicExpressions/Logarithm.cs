namespace THE_HULK;

public class Logarithm : BasicExpression
{
    public override ExpressionKind Kind { get; set; }

    public override object? value { get; set; }

    List<Expression> parameters;

    public Logarithm(List<Expression> _parameters, Environment _environment) : base(_environment)
    {
        parameters = _parameters;
        Kind = ExpressionKind.Number;
        if (parameters.Count != 2)
        {
            System.Console.WriteLine($"! SEMANTIC ERROR: function \"cos\" needs 1 parameter(s), but {_parameters.Count} were given.");
            throw new Exception();
        }
    }

    public override void Evaluate(Environment _environment)
    {
        parameters.First().Evaluate(_environment);
        parameters[1].Evaluate(_environment);
        value = Math.Log((double)parameters.First().value!, (double)parameters[1].value!);
    }

    public override object? GetValue() => value;
}