namespace THE_HULK;
public class Environment
{
    public Dictionary<string, Expression> variables { get; set; }
    public Dictionary<string /*functionName*/, FunctionStructure> functions { get; set; }

    public Environment? father { get; set; }

    public Environment()
    {
        variables = new Dictionary<string,Expression>();
        functions = new Dictionary<string, FunctionStructure>();
    }

    public Environment MakeChild()
    {
        Environment child = new Environment();
        child.father = this;
        return child;
    }
}