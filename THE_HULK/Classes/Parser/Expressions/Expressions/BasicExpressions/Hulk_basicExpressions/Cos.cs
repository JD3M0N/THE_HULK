namespace THE_HULK;
public class CosNode : BasicExpression
{
    public override ExpressionKind Kind { get; set; }

    public override object? value { get; set; }

    List<Expression> Arguments;

    public CosNode(List<Expression> Arguments, Environment scope) : base(scope)
    {
        this.Arguments = Arguments;
        Kind = ExpressionKind.Number;
        if (Arguments.Count != 1)
        {
            Console.WriteLine($"!semantic error: function \"cos\" recieves 1 argument(s), but {Arguments.Count} were given.");
            throw new Exception();
        }
    }

    public override void Evaluate(Environment scope)
    {
        Arguments[0].Evaluate(scope);
        value = Math.Cos((double)Arguments[0].value!);
    }

    public override object? GetValue() => value;
}