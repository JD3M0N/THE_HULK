using System.Data;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace THE_HULK;
/*
    This is the base class for all functions.
*/
public abstract class Function : Expression
{
    public override ExpressionKind Kind { get; set; }
    public Function() : base(null!)
    {
        Kind = ExpressionKind.Temp;
    }
}

