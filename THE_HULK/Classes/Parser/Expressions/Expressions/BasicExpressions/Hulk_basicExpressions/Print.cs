namespace THE_HULK;

public class Print : BasicExpression
{
    public override ExpressionKind Kind { get; set; }

    public override object? value { get; set; }

    List<Expression> parameters;

    public Print(List<Expression> _parameters, Environment _environment) : base(_environment)
    {
        this.parameters = _parameters;
        Kind = ExpressionKind.Temp;
        if (_parameters.Count != 1)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"! SEMANTIC ERROR: function \"cos\" needs 1 parameter(s), but {_parameters.Count} were given.");
            Console.ResetColor();
            throw new Exception();
        }
    }

    public override void Evaluate(Environment _environment)
    {
        parameters.First().Evaluate(_environment);
        value = parameters.First().value;

        System.Console.WriteLine(value);
    }

    public override object? GetValue() => value;
}