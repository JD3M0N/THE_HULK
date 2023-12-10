namespace THE_HULK;

/*
    The Parse Priority is 1.
    This is the EqualsTo class.
    This class is responsible for comparing two expressions.
*/
public class EqualTo : BinaryExpression
{
    public EqualTo(TokenKind _operator, Expression nodeLeft, Expression nodeRight) :
    base(_operator, nodeLeft, nodeRight)
    {
        Kind = ExpressionKind.Bool;
    }

    public override ExpressionKind Kind { get; set; }
    public override object? value { get; set; }

    public override void CheckNodesSemantic(Expression nodeLeft, TokenKind _operator, Expression nodeRight)
    {
        if (base.nodeLeft!.Kind == ExpressionKind.Temp || base.nodeRight!.Kind == ExpressionKind.Temp) { return; }
        if (base.nodeLeft.Kind != base.nodeRight.Kind)
        {
            Console.WriteLine($"! SEMANTIC ERROR: operator \"{_operator}\" cannot be used with different data types.");
            throw new Exception();
        }
    }

    public override void Evaluate(Environment _environment)
    {
        nodeLeft!.Evaluate(_environment);
        nodeRight!.Evaluate(_environment);

        if (nodeLeft.Kind != nodeRight.Kind)
        {
            Console.WriteLine($"! SEMANTIC ERROR: \"{Operator}\" cannot be used between \"{nodeLeft.Kind}\" and \"{nodeRight.Kind}\".");
            throw new Exception();
        }

        value = nodeLeft.GetValue()!.ToString() == nodeRight.GetValue()!.ToString();
    }

    public override object? GetValue() => value;
}