namespace THE_HULK;

public class Exponential : BasicExpression
{
    public override ExpressionKind Kind { get; set; }

    public override object? value { get; set; }

    List<Expression> Arguments;

    public Exponential(List<Expression> Arguments, Environment scope) : base(scope)
    {
        this.Arguments = Arguments;
        Kind = ExpressionKind.Number;
        if (Arguments.Count != 1)
        {
            Console.WriteLine($"!semantic error: function \"exp\" recieves 1 argument(s), but {Arguments.Count} were given.");
            throw new Exception();
        }
    }

    public override void Evaluate(Environment scope)
    {
        Arguments[0].Evaluate(scope);
        value = Math.Pow(Math.E, (double)Arguments[0].value!);
    }

    public override object? GetValue() => value;

}