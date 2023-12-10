namespace THE_HULK;
/*
    This is the function call expression.
    In this class, we evaluate the function call.
*/
public class FunctionInvocation : Function
{
    public override ExpressionKind Kind { get; set; }
    public override object? value { get; set; }
    public string name;
    public List<Expression> variablesOfTheFunction;

    int Count;

    public Environment publicEnvironment;

    public FunctionInvocation(string _name, List<Expression> _variablesOfTheFunction, Environment _publicEnvironment)
    {
        name = _name;
        variablesOfTheFunction = _variablesOfTheFunction;
        publicEnvironment = _publicEnvironment;
    }

    public void CheckSemantic(Environment privateEnvironment)
    {
        if (!privateEnvironment.functions.ContainsKey(name))
        {
            CheckSemantic(privateEnvironment.father!);
        }
        else
        {
            Count = privateEnvironment.functions[name].VariablesOfTheFunction!.Count;
            return;
        }
    }

    public void CheckVariablesCount(Environment scope)
    {
        if (variablesOfTheFunction.Count != Count)
        {
            Console.WriteLine($"!SEMANTIC ERROR: function \"{name}\" needs {scope.functions[name].VariablesOfTheFunction!.Count} argument(s), but {variablesOfTheFunction.Count} were given.");
            throw new Exception();
        }
    }

    public override void Evaluate(Environment scope)
    {
        Environment child = scope.MakeChild();
        child.functions = scope.functions;

        for (int i = 0; i < variablesOfTheFunction.Count; i++)
            child.variables.Add(publicEnvironment.functions[name].VariablesOfTheFunction![i], variablesOfTheFunction[i]);

        foreach (var arg in child.variables)
            child.variables[arg.Key].Evaluate(scope);

        AbstractSyntaxTree builder = new AbstractSyntaxTree(publicEnvironment.functions[name].tokens!, child);
        Expression ast = builder.Parse();
        ast.Evaluate(child);

        if (ast.value is string) this.Kind = ExpressionKind.String;
        if (ast.value is double) this.Kind = ExpressionKind.Number;
        if (ast.value is bool) this.Kind = ExpressionKind.Bool;

        value = ast.value;
    }

    public override object? GetValue() => value;
}