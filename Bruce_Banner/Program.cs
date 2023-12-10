using System.Diagnostics;
using System.Formats.Asn1;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using THE_HULK;


class Interpreter
{
    static void Main()
    {
        Run();
    }

    #region Explanation

    /*
    The Run method is a public static method defined within the Interpreter class. 
    Its purpose is to execute the main logic of the program. 
    Here's a breakdown of what the Run method does:

    1 => It creates a new instance of the THE_HULK.Environment class and assigns it to the GlobalEnvironment variable.

    2 => It clears the console screen using Console.Clear().

    3 => It prints a welcome message to the console using System.Console.WriteLine().

    4 => It enters a while loop that continues indefinitely until the program is terminated.

    5 => Inside the loop, it prompts the user for input by displaying a > symbol using Console.Write(">").

    6 => It reads the user's input from the console using Console.ReadLine() and assigns it to the input variable.

    7 => It checks if the user input is an empty string. If it is, it throws an exception with the message "Error 404: Not Text Found".

    8 => It creates a new instance of the Lexer class, passing the user input as a parameter.

    9 => It calls the Tokenize method on the Lexer instance, which returns a list of Token objects and assigns it to the tokens variable.


    The code you provided is an excerpt, so there may be additional code following this section that utilizes the tokens list for further processing or interpretation.
    */

    #endregion
    public static void Run()
    {
        THE_HULK.Environment GlobalEnvironment = new THE_HULK.Environment();
        Console.Clear();
        System.Console.WriteLine("Welcome to [H]avana [U]niversity [L]anguage for [K]ompilers:");;
        Console.WriteLine("Start using HULK :) ");

        while (true)
        {
            try
            {
                Console.Write(">");
                string input = Console.ReadLine()!;

                if (input == string.Empty)
                    throw new Exception("Error 404: Not Text Found");

                var Lexer = new Lexer(input);
                List<Token> tokens = Lexer.Tokenizer();

                Stopwatch crono = new Stopwatch();

                AbstractSyntaxTree buildAST = new AbstractSyntaxTree(tokens, GlobalEnvironment);

                Expression AST = buildAST.Parse();

                if (AST is not null)
                {
                    AST.Evaluate(GlobalEnvironment);
                    Console.WriteLine(AST.value);
                }
            }

            catch (Exception)
            {
                Console.WriteLine("Error 404");
                continue;
            }
        }
    }
}