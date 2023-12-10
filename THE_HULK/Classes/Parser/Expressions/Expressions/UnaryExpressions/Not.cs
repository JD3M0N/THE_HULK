using System.Linq.Expressions;
namespace THE_HULK;

public class Not : UnaryExpression
{
    public override Expression node { get; set; }
    public override ExpressionKind Kind { get; set; }
    public override object? value { get; set; }
    public override Environment? environment { get; set; }

    public Not(Expression _node) : base(_node)
    {
        Kind = ExpressionKind.Bool;
        this.node = _node;
    }

    public override void CheckNodeSemantic(Expression node)
    {
        if (this.node.Kind != ExpressionKind.Bool && this.node.Kind != ExpressionKind.Temp)
        {
            Console.WriteLine($"!SEMANTIC ERROR: operator \"{TokenKind.Not}\" cannot be applied to \"{this.node.Kind}\".");
            throw new Exception();
        }
    }

    public override void Evaluate(Environment scope)
    {
        node.Evaluate(environment!);
        value = !(bool)node.GetValue()!;
    }

    public override object? GetValue() => value;
}