namespace THE_HULK;

#region Explanation
/*
An Abstract Syntax Tree (AST) is a hierarchical representation of the structure of source code in a programming language.
 It is commonly used in the process of compiling code. 
 The AST captures the syntactic structure of the code, abstracting away details like whitespace and comments.

In the context of compiling, the AST is typically generated during the parsing phase. 
It helps in analyzing and transforming the code by providing a structured representation
 that is easier to work with than raw source code.

Each node in the AST represents a construct in the code, such as a function, variable declaration, or an expression. 
The nodes are connected in a tree-like structure, where the root represents the entire program 
and the child nodes represent the nested constructs.

The AST is useful for performing various compiler tasks, such as type checking, optimization, and code generation.
 It allows compilers to analyze the code's structure, identify errors, and generate efficient executable code.

Overall, the AST serves as an intermediate representation of the code that facilitates the compilation process
 by providing a structured and abstract view of the source code.

*/

#endregion
public class AbstractSyntaxTree
{
    //procceced tokens in the lexer previusly
    List<Token> tokens;
    int currentTokenIndex;
    Token currentToken;
    private Environment publicEnvironment;


    // ===>>> The constructor of the class <<<===
    public AbstractSyntaxTree(List<Token> tokens, Environment _publicEnvironment)
    {
        this.tokens = tokens;
        publicEnvironment = _publicEnvironment;
        currentTokenIndex = 0;
        currentToken = tokens[currentTokenIndex];
    }

    public Expression Parse()
    {
        if (tokens.Count <= 1)
        {
            System.Console.WriteLine("Error 404, not enough arguments founded");
            throw new Exception();
        }

        Expression ast = ParseExpressionLv1(publicEnvironment);

        if (currentToken.Kind != TokenKind.Semicolon)
        {
            System.Console.WriteLine($"! SYNTAX ERROR: operator or expression is missing after \"{currentToken}\"");
            throw new Exception();
        }
        return ast;
    }



    private Expression ParseExpressionLv1(Environment environment)
    {
        Expression nodeLeft = ParseExpressionLv2(environment);

        while (IsALevel1Operator(currentToken.Kind))
        {
            TokenKind operation = currentToken.Kind;
            Eat();
            Expression nodeRight = ParseExpressionLv2(environment);
            nodeLeft = ParseBinaryExpression(nodeLeft, operation, nodeRight);
        }

        Expression node = nodeLeft;
        return node;
    }

    private Expression ParseExpressionLv2(Environment environment)
    {
        Expression nodeLeft = ParseExpressionLv3(environment);

        while (IsALevel2Operator(currentToken.Kind))
        {
            TokenKind operation = currentToken.Kind;
            Eat();
            Expression nodeRight = ParseExpressionLv3(environment);
            nodeLeft = ParseBinaryExpression(nodeLeft, operation, nodeRight);
        }

        Expression node = nodeLeft;
        return node;
    }

    private Expression ParseExpressionLv3(Environment environment)
    {
        Expression nodeLeft = ParseExpressionLv4(environment);

        while (IsALevel3Operator(currentToken.Kind))
        {
            TokenKind operation = currentToken.Kind;
            Eat();
            Expression rightNode = ParseExpressionLv4(environment);
            nodeLeft = ParseBinaryExpression(nodeLeft, operation, rightNode);
        }

        Expression node = nodeLeft;
        return node;
    }

    private Expression ParseExpressionLv4(Environment environment)
    {
        Expression nodeLeft = ParseExpressionLv5(environment);

        while (IsALevel4Operator(currentToken.Kind))
        {
            TokenKind operation = currentToken.Kind;
            Eat();
            Expression nodeRight = ParseExpressionLv5(environment);
            nodeLeft = ParseBinaryExpression(nodeLeft, operation, nodeRight);
        }

        Expression node = nodeLeft;
        return node;
    }

    private Expression ParseExpressionLv5(Environment environment)
    {
        Expression nodeLeft = ParseMinimalExpression(environment);

        while (IsALevel5Operator(currentToken.Kind))
        {
            TokenKind operation = currentToken.Kind;
            Eat();
            Expression nodeRight = ParseMinimalExpression(environment);
            nodeLeft = ParseBinaryExpression(nodeLeft, operation, nodeRight);
        }

        Expression node = nodeLeft;
        return node;
    }

    private Expression ParseMinimalExpression(Environment privateEnvironment)
    {
        switch (currentToken.Kind)
        {
            // ===>>> Data Tokens <<<===
            case TokenKind.Number:
                Number num = new((double)currentToken.GetTokenValue());
                Eat();
                return num;

            case TokenKind.LeftParenthesis:
                Eat();
                Expression expression = ParseExpressionLv1(privateEnvironment);
                WatchFor(TokenKind.RightParenthesis);
                return expression;

            case TokenKind.String:
                String str = new((string)currentToken.GetTokenValue());
                Eat();
                return str;

            case TokenKind.TrueKeyWord:
                Boolean _bool = new(true);
                Eat();
                return _bool;

            case TokenKind.FalseKeyWord:
                _bool = new Boolean(false);
                Eat();
                return _bool;
            // ===>>> End of Data Tokens <<<===

            // ===>>> Let In Expression <<<===
            case TokenKind.LetKeyWord:
                return ParseLet_in(privateEnvironment);
            // ===>>> End of Let In Expression <<<===

            // ===>>> Conditional Expressions <<<===
            case TokenKind.IfKeyWord:
                return ParseIf(privateEnvironment);

            case TokenKind.ElseKeyWord:
                System.Console.WriteLine($"! SYNTAX ERROR: if_else structure isnt correct");
                throw new Exception();
            // ===>>> End of Conditional Expressions <<<===

            // ===>>> Unary Expressions <<<===

            // ===>>> Negative Numbers <<<=== 
            case TokenKind.Difference:
                Eat();
                if (NextToken().Kind == TokenKind.Number)
                {
                    Negation negativeNumber = new Negation(ParseExpressionLv3(privateEnvironment));
                    negativeNumber.SemantiCheck(negativeNumber.node);
                    return negativeNumber;
                }
                else
                {
                    Negation negativeNumber = new Negation(ParseMinimalExpression(privateEnvironment));
                    negativeNumber.SemantiCheck(negativeNumber.node);
                    return negativeNumber;
                }
            // ===>>> End of Negative Numbers <<<===

            // ===>>> Negative Boolean Expressions <<<===
            case TokenKind.Not:
                Eat();
                if (NextToken().Kind == TokenKind.Number)
                {
                    Not notExpression = new Not(ParseExpressionLv1(privateEnvironment));
                    notExpression.SemantiCheck(notExpression.node);
                    return notExpression;
                }
                else
                {
                    Not notExpression = new Not(ParseMinimalExpression(privateEnvironment));
                    notExpression.SemantiCheck(notExpression.node);
                    return notExpression;
                }
            // ===>>> End of Negative Boolean Expressions <<<===



            // ===>>> Variables and Functions <<<===
            case TokenKind.Identifier:
                if (IsParseInFunction(currentToken))
                {
                    return ParseIn(privateEnvironment);
                }
                if (NextToken().Kind == TokenKind.LeftParenthesis)
                {
                    return IndexFunction(privateEnvironment);
                }
                Variable variable = new(currentToken.GetTokenName());
                variable.SemantCheck(privateEnvironment);
                Eat();
                return variable;
            case TokenKind.FunctionKeyWord:
                ParseFunction();
                return null!;
            // ===>>> End of Variables and Functions <<<===



            // ===>>> Syntax Error <<<===
            default:
                System.Console.WriteLine($"! SYNTAX ERROR: unexpected token \"{currentToken}\" after \"{tokens[currentTokenIndex - 1]}\" at index: {currentTokenIndex}.");
                throw new Exception();

        }
    }

    Expression ParseIf(Environment privateEnvironment)
    {
        Eat();

        If_Else If = new(null!, null!, null!);

        If.condition = ParseExpressionLv1(privateEnvironment);

        if (currentToken.Kind == TokenKind.ElseKeyWord)
        {
            System.Console.WriteLine($"! SYNTAX ERROR: if_else expression isnt complete.");
            throw new Exception();
        }

        If.nodeLeft = ParseExpressionLv1(privateEnvironment);

        WatchFor(TokenKind.ElseKeyWord);

        If.nodeRight = ParseExpressionLv1(privateEnvironment);
        
        return If;
    }

    Expression ParseLet_in(Environment privateEnvironment)
    {
        Eat();

        Let_In letIn = new(null!, privateEnvironment.CreateChild());

        IndexVarible(letIn.environment!);

        WatchFor(TokenKind.InKeyWord);

        letIn.parameter = ParseExpressionLv1(letIn.environment!);

        return letIn;
    }

    private void ParseFunction()
    {
        Eat();
        WatchFor(TokenKind.Identifier);

        string name = PreviousToken().GetTokenName();
        FunctionStructure parameters = new FunctionStructure();

        WatchFor(TokenKind.LeftParenthesis);
        IndexVars(parameters.VariablesOfTheFunction!);
        WatchFor(TokenKind.RightParenthesis);
        WatchFor(TokenKind.Arrow);

        for (int i = currentTokenIndex; i < tokens.Count; i++)
            parameters.tokens!.Add(tokens[i]);

        publicEnvironment.functions.Add(name, parameters);
        Eat(tokens.Count - currentTokenIndex - 1);
    }

    private void IndexVars(List<string> argsVariables)
    {
        WatchFor(TokenKind.Identifier);

        argsVariables.Add(PreviousToken().GetTokenName());

        if (currentToken.Kind == TokenKind.Comma)
        {
            Eat();
            IndexVars(argsVariables);
        }
    }



    private Function IndexFunction(Environment privateEnvironment)
    {
        string functId = currentToken.GetTokenName();

        FunctionInvocation function = new FunctionInvocation(functId, new List<Expression>(), publicEnvironment);

        function.SemantiCheck(privateEnvironment);

        Eat(2);

        GetArgs(privateEnvironment, function.variablesOfTheFunction);

        function.CheckVariablesCount(privateEnvironment);

        WatchFor(TokenKind.RightParenthesis);

        return function;
    }

    private Expression ParseIn(Environment privateEnvironment)
    {
        List<Expression> parameters = new List<Expression>();

        string functId = currentToken.GetTokenName();

        Eat();

        WatchFor(TokenKind.LeftParenthesis);

        GetArgs(privateEnvironment, parameters);

        WatchFor(TokenKind.RightParenthesis);

        switch (functId)
        {
            case "print":
                Print print = new Print(parameters, privateEnvironment);
                return print;
            case "sin":
                Sin sin = new Sin(parameters, privateEnvironment);
                return sin;
            case "cos":
                Cos cos = new Cos(parameters, privateEnvironment);
                return cos;
            case "exp":
                Exponential exp = new Exponential(parameters, privateEnvironment);
                return exp;
            case "sqrt":
                SquareRoot sqrt = new SquareRoot(parameters, privateEnvironment);
                return sqrt;
            default:
                Logarithm log = new Logarithm(parameters, privateEnvironment);
                return log;
        }
    }

    private void GetArgs(Environment privateEnvironment, List<Expression> parameters)
    {
        parameters.Add(ParseExpressionLv1(privateEnvironment));
        if (currentToken.Kind == TokenKind.Comma)
        {
            Eat();
            GetArgs(privateEnvironment, parameters);
        }
    }



    private void Eat()
    {
        currentTokenIndex++;

        if (currentTokenIndex < tokens.Count)
        {
            currentToken = tokens[currentTokenIndex];
        }

        else return;
    }
    private void Eat(int positions)
    {
        currentTokenIndex += positions;

        if (currentTokenIndex < tokens.Count)
        {
            currentToken = tokens[currentTokenIndex];
        }
    }


    private Token NextToken() => tokens[currentTokenIndex + 1];

    private Token PreviousToken() => tokens[currentTokenIndex - 1];

    private void WatchFor(TokenKind expected)
    {
        if (currentToken.Kind != expected)
        {
            System.Console.WriteLine($"! SYNTAX ERROR: unexpected token: \"{currentToken}\" at index: {currentTokenIndex} expected: \"{expected}\".");
            throw new Exception();
        }

        Eat();
    }

    Expression ParseBinaryExpression(Expression nodeLeft, TokenKind _operator, Expression nodeRight)
    {
        switch (_operator)
        {
            case TokenKind.And:
                BinaryExpression andNode = new And(_operator, nodeLeft, nodeRight);
                andNode.SemantiCheck(nodeLeft, _operator, nodeRight);
                nodeLeft = andNode;
                break;

            case TokenKind.Or:
                BinaryExpression orNode = new Or(_operator, nodeLeft, nodeRight);
                orNode.SemantiCheck(nodeLeft, _operator, nodeRight);
                nodeLeft = orNode;
                break;

            case TokenKind.GreaterOrEqualThan:
                BinaryExpression greatherOrEqualsNode = new GreaterOrEqualThan(_operator, nodeLeft, nodeRight);
                greatherOrEqualsNode.SemantiCheck(nodeLeft, _operator, nodeRight);
                nodeLeft = greatherOrEqualsNode;
                break;

            case TokenKind.GreaterThan:
                BinaryExpression greatherThanNode = new GreaterThan(_operator, nodeLeft, nodeRight);
                greatherThanNode.SemantiCheck(nodeLeft, _operator, nodeRight);
                nodeLeft = greatherThanNode;
                break;

            case TokenKind.LessOrEqualThan:
                BinaryExpression lessOrEqualsNode = new LessOrEqualThan(_operator, nodeLeft, nodeRight);
                lessOrEqualsNode.SemantiCheck(nodeLeft, _operator, nodeRight);
                nodeLeft = lessOrEqualsNode;
                break;

            case TokenKind.LessThan:
                BinaryExpression lesserThanNode = new LessThan(_operator, nodeLeft, nodeRight);
                lesserThanNode.SemantiCheck(nodeLeft, _operator, nodeRight);
                nodeLeft = lesserThanNode;
                break;

            case TokenKind.EqualTo:
                BinaryExpression equalsTo = new EqualTo(_operator, nodeLeft, nodeRight);
                equalsTo.SemantiCheck(nodeLeft, _operator, nodeRight);
                nodeLeft = equalsTo;
                break;

            case TokenKind.Sum:
                BinaryExpression additionNode = new Sum(_operator, nodeLeft, nodeRight);
                additionNode.SemantiCheck(nodeLeft, _operator, nodeRight);
                nodeLeft = additionNode;
                break;

            case TokenKind.Difference:
                BinaryExpression substractionNode = new Difference(_operator, nodeLeft, nodeRight);
                substractionNode.SemantiCheck(nodeLeft, _operator, nodeRight);
                nodeLeft = substractionNode;
                break;

            case TokenKind.Concatenation:
                BinaryExpression concatenationNode = new Concatenation(_operator, nodeLeft, nodeRight);
                nodeLeft = concatenationNode;
                break;

            case TokenKind.Product:
                BinaryExpression multiplicationNode = new Product(_operator, nodeLeft, nodeRight);
                multiplicationNode.SemantiCheck(nodeLeft, _operator, nodeRight);
                nodeLeft = multiplicationNode;
                break;

            case TokenKind.Quotient:
                BinaryExpression divisionNode = new Quotient(_operator, nodeLeft, nodeRight);
                divisionNode.SemantiCheck(nodeLeft, _operator, nodeRight);
                nodeLeft = divisionNode;
                break;

            case TokenKind.Modulo:
                BinaryExpression modulusNode = new Modulo(_operator, nodeLeft, nodeRight);
                modulusNode.SemantiCheck(nodeLeft, _operator, nodeRight);
                nodeLeft = modulusNode;
                break;

            case TokenKind.Power:
                BinaryExpression powerNode = new Power(_operator, nodeLeft, nodeRight);
                powerNode.SemantiCheck(nodeLeft, _operator, nodeRight);
                nodeLeft = powerNode;
                break;
        }

        return nodeLeft;
    }

    void IndexVarible(Environment letInEnvironment)
    {
        WatchFor(TokenKind.Identifier);
        string varName = PreviousToken().GetTokenName();
        WatchFor(TokenKind.EqualEqual);
        letInEnvironment.variables.Add(varName, ParseExpressionLv1(letInEnvironment));


        if (currentToken.Kind == TokenKind.Comma)
        {
            Eat();
            IndexVarible(letInEnvironment);
        }
    }

    bool IsALevel1Operator(TokenKind operation)
    {
        List<TokenKind> operators = new List<TokenKind>()
            {
                TokenKind.Or,
                TokenKind.And
            };

        return operators.Contains(operation);
    }

    bool IsALevel2Operator(TokenKind operation)
    {
        List<TokenKind> operators = new List<TokenKind>()
            {
                TokenKind.UnEqual,
                TokenKind.EqualTo,
                TokenKind.LessThan,
                TokenKind.GreaterThan,
                TokenKind.LessOrEqualThan,
                TokenKind.GreaterOrEqualThan
            };

        return operators.Contains(operation);
    }

    bool IsALevel3Operator(TokenKind operation)
    {
        List<TokenKind> operators = new List<TokenKind>()
            {
                TokenKind.Sum,
                TokenKind.Difference,
                TokenKind.Concatenation
            };

        return operators.Contains(operation);
    }

    bool IsALevel4Operator(TokenKind operation)
    {
        List<TokenKind> operators = new List<TokenKind>()
            {
                TokenKind.Modulo,
                TokenKind.Product,
                TokenKind.Quotient
            };

        return operators.Contains(operation);
    }

    bool IsALevel5Operator(TokenKind operation)
    {
        List<TokenKind> operators = new List<TokenKind>()
            {
                TokenKind.Power
            };

        return operators.Contains(operation);
    }

    bool IsParseInFunction(Token Identifier)
    {
        List<string> builtInFunctions = new()
        {
            "sin",
            "cos",
            "exp",
            "log",
            "sqrt",
            "print"
        };

        return builtInFunctions.Contains(Identifier.GetTokenName());
    }

}