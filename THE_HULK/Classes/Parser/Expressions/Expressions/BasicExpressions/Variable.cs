namespace THE_HULK;

/*
    This is the variable expression.
*/
public class Variable : BasicExpression
{
    public override Environment? environment { get; set; }
    public string name;

    public override object? value { get; set; }
    public override ExpressionKind Kind { get; set; }

    public Variable(string _name) : base(null!)
    {
        name = _name;
        Kind = ExpressionKind.Temp;
    }

    // this is the semantic check for the variable it makes sure that the variable exists
    // if not it throws an exception
    public void CheckSemantic(Environment privateEnvironment)
    {
        switch (name)
        {
            case "E": // Euler's number
                return;
            case "PI": // PI
                return;
            case "Tau": // Tau
                return;
        }

        if (privateEnvironment is null)
        {
            Console.WriteLine($"! SEMANTIC ERROR: the variable \"{name}\" doesn't exists.");
            throw new Exception();
        }
        if (privateEnvironment.variables.ContainsKey(name))
        {
            return;
        }
        else
        {
            CheckSemantic(privateEnvironment.father!);
        }
    }


    public override void Evaluate(Environment internalEnvironment)
    {
        // If the environment that we are in contains the variable name as key
        // we evaluate the value of that key and get the value of the variable
        switch (name)
        {
            case "E": // Euler's number
                Kind = ExpressionKind.Number;
                value = Math.E;
                return;
            case "PI": // PI 
                Kind = ExpressionKind.Number;
                value = Math.PI;
                return;
            case "Tau": // Tau
                Kind = ExpressionKind.Number;
                value = Math.Tau;
                return;
        }

        if (internalEnvironment!.variables.ContainsKey(name))
        {

            if (internalEnvironment.variables[name].value is not null)
                value = internalEnvironment.variables[name].value;
            else
            {
                internalEnvironment.variables[name].Evaluate(internalEnvironment.variables[name].environment!);
                value = internalEnvironment.variables[name].value;
            }
        }
        else
        {
            Evaluate(internalEnvironment!.father!);
        }

        switch (value.GetType().ToString())
        {
            case "System.String": // String
                Kind = ExpressionKind.String;
                break;
            case "System.Double": // Number
                Kind = ExpressionKind.Number;
                break;
            case "System.Bool": // Boolean
                Kind = ExpressionKind.Bool;
                break;
        }

    }

    public override object? GetValue() => value;
}