namespace THE_HULK;

public class Logarithm : BasicExpression
{
    public override ExpressionKind Kind { get; set; }

    public override object? value { get; set; }

    List<Expression> Arguments;

    public Logarithm(List<Expression> arguments, Environment scope) : base(scope)
    {
        Arguments = arguments;
        Kind = ExpressionKind.Number;
        if (Arguments.Count != 2)
        {
            Console.WriteLine($"!semantic error: function \"log\" recieves 1 argument(s), but {Arguments.Count} were given.");
            throw new Exception();
        }
    }

    public override void Evaluate(Environment scope)
    {
        Arguments[0].Evaluate(scope);
        Arguments[1].Evaluate(scope);
        value = Math.Log((double)Arguments[1].value!, (double)Arguments[0].value!);
    }

    public override object? GetValue() => value;
}