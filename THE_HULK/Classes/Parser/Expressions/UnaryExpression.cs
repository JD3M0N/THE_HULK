namespace THE_HULK;
/*
    This is the unary expression.
    Its responsible for storing the node of the expression.
*/

public abstract class UnaryExpression : Expression
{
    public abstract Expression node { get; set; }
    public UnaryExpression(Expression _node) : base(null!)
    {
        node = _node;
    }

    public abstract void SemantiCheck(Expression _node);
}