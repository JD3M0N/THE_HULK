namespace THE_HULK;
/*
    This is the function structure.
    Its responsible for storing the variables of the function.
*/
public class FunctionStructure
{
    public virtual List<string>? VariablesOfTheFunction { get; set; }
    public List<Token>? tokens = new List<Token>();

    public FunctionStructure()
    {
        VariablesOfTheFunction = new List<string>();
    }
}