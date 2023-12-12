namespace THE_HULK;


/*
    Its Parse Priority is 1.
    This is the And class.
    This class is responsible for handling the "&&" operator.
*/
public class And : BinaryExpression
{
    public And(TokenKind _operator, Expression nodeLeft, Expression nodeRight) :
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

        if (nodeLeft.Kind != ExpressionKind.Bool || nodeRight.Kind != ExpressionKind.Bool)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"! SEMANTIC ERROR: \"{Operator}\" cannot be used between \"{nodeLeft.Kind}\" and \"{nodeRight.Kind}\".");
            Console.ResetColor();
            throw new Exception();
        }

        value = (bool)nodeLeft.GetValue()! && (bool)nodeRight.GetValue()!;
    }

    public override void SemantiCheck(Expression nodeLeft, TokenKind _operator, Expression nodeRight)
    {
        if ((base.nodeLeft.Kind != ExpressionKind.Bool && base.nodeLeft.Kind != ExpressionKind.Temp) || (base.nodeRight.Kind != ExpressionKind.Bool && base.nodeRight.Kind != ExpressionKind.Temp))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"! SEMANTIC ERROR: operator \"{_operator}\" cannot be used between \"{base.nodeLeft.Kind}\" and \"{base.nodeRight!.Kind}\" data types.");
            Console.ResetColor();
            throw new Exception();
        }
    }

    public override object? GetValue() => value;
}