namespace THE_HULK;
/*
    Its Parse Priority is 3.
    This is the Sum expression.
    This class is responsible for handling the "@" operator.
*/
public class Concatenation : BinaryExpression
{
    public Concatenation(TokenKind _operator, Expression nodeLeft, Expression nodeRight) :
     base(_operator, nodeLeft, nodeRight)
    {
        Kind = ExpressionKind.String;
    }

    public override ExpressionKind Kind { get; set; }
    public override object? value { get; set; }

    public override void Evaluate(Environment _environment)
    {
        nodeLeft!.Evaluate(_environment);
        nodeRight!.Evaluate(_environment);
        value = nodeLeft.GetValue()!.ToString()! + nodeRight.GetValue()!.ToString()!;
    }

    public override object? GetValue() => value;
}