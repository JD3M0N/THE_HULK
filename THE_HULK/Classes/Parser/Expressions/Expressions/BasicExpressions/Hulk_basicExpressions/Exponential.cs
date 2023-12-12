namespace THE_HULK;

public class Exponential : BasicExpression
{
    public override ExpressionKind Kind { get; set; }

    public override object? value { get; set; }

    List<Expression> parameter;

    public Exponential(List<Expression> _parameter, Environment _environment) : base(_environment)
    {
        parameter = _parameter;
        Kind = ExpressionKind.Number;
        if (_parameter.Count != 2)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"! SEMANTIC ERROR: function \"cos\" needs 1 parameter(s), but {_parameter.Count} were given.");
            Console.ResetColor();
            throw new Exception();
        }
    }

    public override void Evaluate(Environment _environment)
    {
        parameter.First().Evaluate(_environment);
        parameter[1].Evaluate(_environment);
        value = Math.Pow((double)parameter.First().value!, (double)parameter[1].value!);
    }

    public override object? GetValue() => value;

}