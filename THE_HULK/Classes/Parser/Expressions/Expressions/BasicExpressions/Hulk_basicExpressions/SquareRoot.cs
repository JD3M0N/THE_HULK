namespace THE_HULK;

public class SquareRoot : BasicExpression
{
    public override ExpressionKind Kind { get; set; }

    public override object? value { get; set; }

    List<Expression> parameter;

    public SquareRoot(List<Expression> _parameter, Environment _environment) : base(_environment)
    {
        this.parameter = _parameter;
        Kind = ExpressionKind.Number;
        if (_parameter.Count != 1)
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
        value = Math.Sqrt((double)parameter.First().value!);
    }

    public override object? GetValue() => value;

}