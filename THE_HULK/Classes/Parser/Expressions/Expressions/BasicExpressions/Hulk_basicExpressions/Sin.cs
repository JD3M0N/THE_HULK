namespace THE_HULK;

public class Sin : BasicExpression
{
    public override ExpressionKind Kind { get; set; }

    public override object? value { get; set; }

    List<Expression> parameter;

    public Sin(List<Expression> _parameters, Environment _environment) : base(_environment)
    {
        this.parameter = _parameters;
        Kind = ExpressionKind.Number;
        if (_parameters.Count != 1)
        {
            System.Console.WriteLine($"! SEMANTIC ERROR: function \"cos\" needs 1 parameter(s), but {_parameters.Count} were given.");
            throw new Exception();
        }
    }

    public override void Evaluate(Environment _environment)
    {
        parameter.First().Evaluate(_environment);
        value = Math.Sin((double)parameter.First().value!);
    }

    public override object? GetValue() => value;

}