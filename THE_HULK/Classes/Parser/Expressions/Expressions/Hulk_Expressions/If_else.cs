namespace THE_HULK;
/*
    This is the if else expression.
*/
public class If_Else : Expression
{
    public Expression condition;
    public Expression nodeLeft;
    public Expression nodeRight;

    public If_Else(Expression _condition, Expression _nodeLeft, Expression _nodeRight) : base(null!)
    {
        condition = _condition;
        nodeLeft = _nodeLeft;
        nodeRight = _nodeRight;
        Kind = ExpressionKind.Temp;
    }

    public override ExpressionKind Kind { get; set; }
    public override object? value { get; set; }
    public override Environment? environment { get; set; }

    public override void Evaluate(Environment _environment)
    {
        condition!.Evaluate(_environment);

        if (condition.value is true)
        {
            nodeLeft.Evaluate(_environment);
            value = nodeLeft.value;
            Kind = nodeLeft.Kind;
        }
        else
        {
            nodeRight.Evaluate(_environment);
            value = nodeRight!.GetValue();
            Kind = nodeRight.Kind;
        }
    }

    public override object? GetValue() => value;
}