namespace THE_HULK;
/*
    This is the basic expression.
    Its responsible for storing the value of the expression.
*/
public abstract class BasicExpression : Expression
{
    public override Environment? environment { get; set; }
    public BasicExpression(Environment _environment) : base(_environment) { }

    public override string ToString() => $"{value}";
}

