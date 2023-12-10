namespace THE_HULK;

/*
    Its Parse Priority is 2.
    This is the LessOrEqualThan class.
    This class is responsible for handling the "<=" operator.
*/

public class LessOrEqualThan : BinaryExpression
{
    public LessOrEqualThan(TokenKind _operator, Expression nodeLeft, Expression nodeRight) :
    base(_operator, nodeLeft, nodeRight)
    {
        Kind = ExpressionKind.Bool;
    }


    public override ExpressionKind Kind { get; set; }
    public override object? value { get; set; }

    public override void Evaluate(Environment _environment)
    {
        nodeLeft!.Evaluate(_environment);
        nodeRight!.Evaluate(_environment);

        if (nodeLeft.Kind != ExpressionKind.Number || nodeRight.Kind != ExpressionKind.Number)
        {
            Console.WriteLine($"! SEMANTIC ERROR: \"{Operator}\" cannot be used between \"{nodeLeft.Kind}\" and \"{nodeRight.Kind}\".");
            throw new Exception();
        }

        value = (double)nodeLeft.GetValue()! <= (double)nodeRight.GetValue()!;
    }

    public override object? GetValue() => value;
}