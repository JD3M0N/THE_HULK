namespace THE_HULK;

/*
    This is the expression class.
    Its responsible for storing the expressions.
*/
public abstract class Expression
{
    public abstract ExpressionKind Kind { get; set; }

    public abstract object? value { get; set; }

    public virtual Environment? environment { get; set; }

    public Expression(Environment _environment)
    {
        environment = _environment;
    }

    public virtual void Evaluate(Environment _environment) { return; }

    public override string ToString() => $"{value}";
    
    public abstract object? GetValue();
}