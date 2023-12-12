using System.Diagnostics;
using System.Formats.Asn1;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using THE_HULK;

namespace Bruce_Banner;
public class Interpreter
{
    static void Main()
    {
        Run();
        // Testin();
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
        // int count = 0;
        // int maxCount = 20000;

        THE_HULK.Environment PublicEnvironment = new THE_HULK.Environment();
        Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine("Welcome to [H]avana [U]niversity [L]anguage for [K]ompilers:"); ;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Start using HULK :) ");
        Console.ResetColor();

        while (true)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("==>>");
                Console.ResetColor();

                string input = Console.ReadLine()!;

                if (input == string.Empty)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine("Error 404: Not Text Found");
                    Console.ResetColor();
                }

                var Lexer = new Lexer(input);
                List<Token> tokens = Lexer.Tokenizer();

                AbstractSyntaxTree parseAST = new AbstractSyntaxTree(tokens, PublicEnvironment);

                Expression AST = parseAST.Parse();

                if (AST is not null)
                {
                    AST.Evaluate(PublicEnvironment);

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    System.Console.Write("==>>");
                    Console.ResetColor();

                    System.Console.WriteLine(AST.value);
                }
            }

            catch (Exception)
            {
                // System.Console.WriteLine("Error 404: Error found(Contradiction)");
                continue;
            }
        }
    }

    //Testin'
    public static void Testin()
    {
        THE_HULK.Environment PublicEnvironment = new THE_HULK.Environment();
        string[] inputs =
        {
            // @"function f(x) => let a = 
            //                             (let b = 
            //                                     (let c = 1 in c) 
            //                                                     in b) 
            //                                                         in a + x;",
            // "f(10);",

            // "function fib(n) => if(n>1) fib(n-1) + fib(n-2) else 1;",
            // "print(fib(-3));"
            // "1/0;"
            
            //"let a = 42 in if (a % 2 == 0) print(\"Even\") else print(\"odd\");"

            // "print(7 + (let x = 2 in x * x));"

            // "function Sum(x) => Sum(x - 1);",
            // "let a = 1 in Sum(a);"
        };
        foreach (var input in inputs)
        {
            Lexer lexer = new Lexer(input);
            List<Token> tokens = lexer.Tokenizer();

            AbstractSyntaxTree parseAST = new AbstractSyntaxTree(tokens, PublicEnvironment);
            Expression AST = parseAST.Parse();

            if (AST is not null)
            {
                AST.Evaluate(PublicEnvironment);
                // System.Console.WriteLine(AST.value); 
            }
        }
        
    }
}