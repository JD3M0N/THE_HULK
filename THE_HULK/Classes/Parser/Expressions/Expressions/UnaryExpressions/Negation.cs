using System.Linq.Expressions;
namespace THE_HULK;
public class Negation : UnaryExpression
{
    public override Expression node { get; set; }
    public override ExpressionKind Kind { get; set; }
    public override object? value { get; set; }
    public override Environment? environment { get; set; }

    public Negation(Expression _node) : base(_node)
    {
        Kind = ExpressionKind.Number;
        node = _node;
    }

    public override void CheckNodeSemantic(Expression node)
    {
        if(this.node.Kind != ExpressionKind.Number && this.node.Kind != ExpressionKind.Temp)
        {
            Console.WriteLine($"!SEMANTIC ERROR: operator \"{TokenKind.Difference}\" cannot be applied to \"{this.node.Kind}\".");
            throw new Exception();
        }
    }

    public override void Evaluate(Environment _environment)
    {
        node.Evaluate(environment!);
        value = -(double)node.GetValue()!;
    }

    public override object? GetValue() => value;
}