using System.Runtime.CompilerServices;

namespace THE_HULK;

/*
    This is the base class for all binary expressions.
*/

public abstract class BinaryExpression : Expression
{
    public Expression nodeLeft;
    public Expression nodeRight;
    public TokenKind Operator;
    public override Environment? environment { get; set; }

    public BinaryExpression(TokenKind _operator, Expression _nodeLeft, Expression _nodeRight) : base(null!)
    {
        nodeLeft = _nodeLeft;
        nodeRight = _nodeRight;
        Operator = _operator;
    }

    public virtual void SemantiCheck(Expression nodeLeft, TokenKind operator_, Expression nodeRight)
    {
        if ((nodeLeft!.Kind != ExpressionKind.Number && nodeLeft.Kind != ExpressionKind.Temp) || (nodeRight!.Kind != ExpressionKind.Number && nodeRight.Kind != ExpressionKind.Temp))
        {
            Console.WriteLine($"! SEMANTIC ERROR: \"{operator_}\" cannot be used between \"{nodeLeft.Kind}\" and \"{nodeRight!.Kind}\".");
            throw new Exception();
        }
    }
}