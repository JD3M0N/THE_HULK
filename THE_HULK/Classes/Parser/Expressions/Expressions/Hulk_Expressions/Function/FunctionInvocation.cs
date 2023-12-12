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

    public void SemantiCheck(Environment privateEnvironment)
    {
        if (!privateEnvironment.functions.ContainsKey(name))
        {
            SemantiCheck(privateEnvironment.father!);
        }
        else
        {
            Count = privateEnvironment.functions[name].VariablesOfTheFunction!.Count;
            return;
        }
    }

    public void CheckCount(Environment _environment)
    {
        if (variablesOfTheFunction.Count != Count)
        {
            Console.WriteLine($"! SEMANTIC ERROR: function \"{name}\" needs {_environment.functions[name].VariablesOfTheFunction!.Count} argument(s), but {variablesOfTheFunction.Count} were given.");
            throw new Exception();
        }
    }

    public override void Evaluate(Environment _environment)
    {

        Environment child = _environment.CreateChild();

        void PublicEnvironmental(Environment _environment)
        {
            if(_environment.father is null)
            {
                child.functions = _environment.functions;
                return;
            }
            else
            {
                PublicEnvironmental(_environment.father);
            }
        }

        PublicEnvironmental(child);

        //child.functions = _environment.functions;

        for (int i = 0; i < variablesOfTheFunction.Count; i++)
            child.variables.Add(publicEnvironment.functions[name].VariablesOfTheFunction![i], variablesOfTheFunction[i]);

        foreach (var arg in child.variables)
            child.variables[arg.Key].Evaluate(_environment);

        AbstractSyntaxTree parser = new AbstractSyntaxTree(publicEnvironment.functions[name].tokens!, child);
        
        Expression ast = parser.Parse();
        ast.Evaluate(child);

        if (ast.value is string) this.Kind = ExpressionKind.String;
        if (ast.value is double) this.Kind = ExpressionKind.Number;
        if (ast.value is bool) this.Kind = ExpressionKind.Bool;

        value = ast.value;
    }

    public override object? GetValue() => value;
}