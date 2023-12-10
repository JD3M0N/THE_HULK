namespace THE_HULK;

public class Print : BasicExpression
{
    public override ExpressionKind Kind { get; set; }

    public override object? value { get; set; }

    List<Expression> Arguments;

    public Print(List<Expression> Arguments, Environment scope) : base(scope)
    {
        this.Arguments = Arguments;
        Kind = ExpressionKind.Temp;
        if (Arguments.Count != 1)
        {
            Console.WriteLine($"!semantic error: function \"print\" recieves 1 argument(s), but {Arguments.Count} were given.");
            throw new Exception();
        }
    }

    public override void Evaluate(Environment scope)
    {
        Arguments[0].Evaluate(scope);
        value = Arguments[0].value;
        if (value is string) Kind = ExpressionKind.String;
        if (value is double) Kind = ExpressionKind.Number;
        if (value is bool) Kind = ExpressionKind.Bool;
    }

    public override object? GetValue() => value;
}