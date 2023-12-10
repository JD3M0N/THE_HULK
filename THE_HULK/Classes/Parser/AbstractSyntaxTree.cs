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
            Console.WriteLine("Error 404, not enough arguments founded");
            throw new Exception();
        }

        Expression ast = PaseExpressionLv1(publicEnvironment);

        if (currentToken.Kind != TokenKind.Semicolon)
        {
            Console.WriteLine($"! SYNTAX ERROR: operator or expression is missing after \"{currentToken}\"");
            throw new Exception();
        }
        return ast;
    }



    private Expression PaseExpressionLv1(Environment environment)
    {
        Expression nodeLeft = PaseExpressionLv2(environment);

        while (IsALevel1Operator(currentToken.Kind))
        {
            TokenKind operation = currentToken.Kind;
            Eat();
            Expression nodeRight = PaseExpressionLv2(environment);
            nodeLeft = ParseBinaryExpression(nodeLeft, operation, nodeRight);
        }

        Expression node = nodeLeft;
        return node;
    }

    private Expression PaseExpressionLv2(Environment environment)
    {
        Expression nodeLeft = PaseExpressionLv3(environment);

        while (IsALevel2Operator(currentToken.Kind))
        {
            TokenKind operation = currentToken.Kind;
            Eat();
            Expression nodeRight = PaseExpressionLv3(environment);
            nodeLeft = ParseBinaryExpression(nodeLeft, operation, nodeRight);
        }

        Expression node = nodeLeft;
        return node;
    }

    private Expression PaseExpressionLv3(Environment environment)
    {
        Expression nodeLeft = PaseExpressionLv4(environment);

        while (IsALevel3Operator(currentToken.Kind))
        {
            TokenKind operation = currentToken.Kind;
            Eat();
            Expression rightNode = PaseExpressionLv4(environment);
            nodeLeft = ParseBinaryExpression(nodeLeft, operation, rightNode);
        }

        Expression node = nodeLeft;
        return node;
    }

    private Expression PaseExpressionLv4(Environment environment)
    {
        Expression nodeLeft = PaseExpressionLv5(environment);

        while (IsALevel4Operator(currentToken.Kind))
        {
            TokenKind operation = currentToken.Kind;
            Eat();
            Expression nodeRight = PaseExpressionLv5(environment);
            nodeLeft = ParseBinaryExpression(nodeLeft, operation, nodeRight);
        }

        Expression node = nodeLeft;
        return node;
    }

    private Expression PaseExpressionLv5(Environment environment)
    {
        Expression nodeLeft = BuildAtom(environment);

        while (IsALevel5Operator(currentToken.Kind))
        {
            TokenKind operation = currentToken.Kind;
            Eat();
            Expression nodeRight = BuildAtom(environment);
            nodeLeft = ParseBinaryExpression(nodeLeft, operation, nodeRight);
        }

        Expression node = nodeLeft;
        return node;
    }

    private Expression BuildAtom(Environment privateEnvironment)
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
                Expression expression = PaseExpressionLv1(privateEnvironment);
                Expect(TokenKind.RightParenthesis);
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
                return ParseLetInStructure(privateEnvironment);
            // ===>>> End of Let In Expression <<<===

            // ===>>> Conditional Expressions <<<===
            case TokenKind.IfKeyWord:
                return PaseConditionalExpression(privateEnvironment);

            case TokenKind.ElseKeyWord:
                Console.WriteLine($"! SYNTAX ERROR: if_else structure isnt correct");
                throw new Exception();
            // ===>>> End of Conditional Expressions <<<===

            // ===>>> Unary Expressions <<<===

                // ===>>> Negative Numbers <<<=== 
            case TokenKind.Difference:
                Eat();
                if (NextToken().Kind == TokenKind.Number)
                {
                    Negation negativeNumber = new Negation(PaseExpressionLv3(privateEnvironment));
                    negativeNumber.CheckNodeSemantic(negativeNumber.node);
                    return negativeNumber;
                }
                else
                {
                    Negation negativeNumber = new Negation(BuildAtom(privateEnvironment));
                    negativeNumber.CheckNodeSemantic(negativeNumber.node);
                    return negativeNumber;
                }
                // ===>>> End of Negative Numbers <<<===

            // ===>>> Negative Boolean Expressions <<<===
            case TokenKind.Not:
                Eat();
                if (NextToken().Kind == TokenKind.Number)
                {
                    Not notExpression = new Not(PaseExpressionLv1(privateEnvironment));
                    notExpression.CheckNodeSemantic(notExpression.node);
                    return notExpression;
                }
                else
                {
                    Not notExpression = new Not(BuildAtom(privateEnvironment));
                    notExpression.CheckNodeSemantic(notExpression.node);
                    return notExpression;
                }
            // ===>>> End of Negative Boolean Expressions <<<===



            // ===>>> Variables and Functions <<<===
            case TokenKind.Identifier:
                if (IsParseInFunction(currentToken)) { return PaseInFunctionCall(privateEnvironment); }
                if (NextToken().Kind == TokenKind.LeftParenthesis) { return IndexFunction(privateEnvironment); }
                Variable variable = new(currentToken.GetTokenName());
                variable.CheckSemantic(privateEnvironment);
                Eat();
                return variable;
            case TokenKind.FunctionKeyWord:
                ParseFunction();
                return null!;
            // ===>>> End of Variables and Functions <<<===

            

            // ===>>> Syntax Error <<<===
            default:
                Console.WriteLine($"! SYNTAX ERROR: unexpected token \"{currentToken}\" after \"{tokens[currentTokenIndex - 1]}\" at index: {currentTokenIndex}.");
                throw new Exception();
                
        }
    }

    Expression PaseConditionalExpression(Environment privateEnvironment)
    {
        Eat();
        If_Else conditionalExpression = new(null!, null!, null!);
        conditionalExpression.condition = PaseExpressionLv1(privateEnvironment);
        if (currentToken.Kind == TokenKind.ElseKeyWord)
        {
            Console.WriteLine($"! SYNTAX ERROR: if_else expression isnt complete.");
            throw new Exception();
        }
        conditionalExpression.nodeLeft = PaseExpressionLv1(privateEnvironment);
        Expect(TokenKind.ElseKeyWord);
        conditionalExpression.rightNode = PaseExpressionLv1(privateEnvironment);
        return conditionalExpression;
    }

    Expression ParseLetInStructure(Environment privateEnvironment)
    {
        Eat();
        Let_In letInExpression = new(null!, privateEnvironment.MakeChild());
        IndexVarible(letInExpression.environment!);
        Expect(TokenKind.InKeyWord);
        letInExpression.argument = PaseExpressionLv1(letInExpression.environment!);
        return letInExpression;
    }

    private void ParseFunction()
    {
        Eat();
        Expect(TokenKind.Identifier);

        string functionName = PreviousToken().GetTokenName();
        FunctionStructure arguments = new FunctionStructure();

        Expect(TokenKind.LeftParenthesis);
        IndexVars(arguments.VariablesOfTheFunction!);
        Expect(TokenKind.RightParenthesis);
        Expect(TokenKind.Arrow);

        for (int i = currentTokenIndex; i < tokens.Count; i++)
            arguments.tokens!.Add(tokens[i]);

        publicEnvironment.functions.Add(functionName, arguments);
        Eat(tokens.Count - currentTokenIndex - 1);
    }

    private void IndexVars(List<string> argsVariables)
    {
        Expect(TokenKind.Identifier);
        argsVariables.Add(PreviousToken().GetTokenName());
        if (currentToken.Kind == TokenKind.Comma)
        {
            Eat();
            IndexVars(argsVariables);
        }
    }



    private Function IndexFunction(Environment localScope)
    {
        string functId = currentToken.GetTokenName();
        FunctionInvocation foo = new FunctionInvocation(functId, new List<Expression>(), publicEnvironment);
        foo.CheckSemantic(localScope);
        Eat(2);
        GetArgs(localScope, foo.variablesOfTheFunction);
        foo.CheckVariablesCount(localScope);
        Expect(TokenKind.RightParenthesis);
        return foo;
    }

    private Expression PaseInFunctionCall(Environment localScope)
    {
        List<Expression> arguments = new List<Expression>();
        string functId = currentToken.GetTokenName();
        Eat();
        Expect(TokenKind.LeftParenthesis);
        GetArgs(localScope, arguments);
        Expect(TokenKind.RightParenthesis);

        switch (functId)
        {
            case "print":
                Print print = new Print(arguments, localScope);
                return print;
            case "sin":
                SinNode sin = new SinNode(arguments, localScope);
                return sin;
            case "cos":
                CosNode cos = new CosNode(arguments, localScope);
                return cos;
            case "exp":
                Exponential exp = new Exponential(arguments, localScope);
                return exp;
            case "sqrt":
                SquareRoot sqrt = new SquareRoot(arguments, localScope);
                return sqrt;
            default:
                Logarithm log = new Logarithm(arguments, localScope);
                return log;
        }
    }

    private void GetArgs(Environment localScope, List<Expression> arguments)
    {
        arguments.Add(PaseExpressionLv1(localScope));
        if (currentToken.Kind == TokenKind.Comma)
        {
            Eat();
            GetArgs(localScope, arguments);
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

    private void Expect(TokenKind expected)
    {
        if (currentToken.Kind != expected)
        {
            Console.WriteLine($"! SYNTAX ERROR: unexpected token: \"{currentToken}\" at index: {currentTokenIndex} expected: \"{expected}\".");
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
                andNode.CheckNodesSemantic(nodeLeft, _operator, nodeRight);
                nodeLeft = andNode;
                break;

            case TokenKind.Or:
                BinaryExpression orNode = new Or(_operator, nodeLeft, nodeRight);
                orNode.CheckNodesSemantic(nodeLeft, _operator, nodeRight);
                nodeLeft = orNode;
                break;

            case TokenKind.GreaterOrEqualThan:
                BinaryExpression greatherOrEqualsNode = new GreaterOrEqualThan(_operator, nodeLeft, nodeRight);
                greatherOrEqualsNode.CheckNodesSemantic(nodeLeft, _operator, nodeRight);
                nodeLeft = greatherOrEqualsNode;
                break;

            case TokenKind.GreaterThan:
                BinaryExpression greatherThanNode = new GreaterThan(_operator, nodeLeft, nodeRight);
                greatherThanNode.CheckNodesSemantic(nodeLeft, _operator, nodeRight);
                nodeLeft = greatherThanNode;
                break;

            case TokenKind.LessOrEqualThan:
                BinaryExpression lessOrEqualsNode = new LessOrEqualThan(_operator, nodeLeft, nodeRight);
                lessOrEqualsNode.CheckNodesSemantic(nodeLeft, _operator, nodeRight);
                nodeLeft = lessOrEqualsNode;
                break;

            case TokenKind.LessThan:
                BinaryExpression lesserThanNode = new LessThan(_operator, nodeLeft, nodeRight);
                lesserThanNode.CheckNodesSemantic(nodeLeft, _operator, nodeRight);
                nodeLeft = lesserThanNode;
                break;

            case TokenKind.EqualTo:
                BinaryExpression equalsTo = new EqualTo(_operator, nodeLeft, nodeRight);
                equalsTo.CheckNodesSemantic(nodeLeft, _operator, nodeRight);
                nodeLeft = equalsTo;
                break;

            case TokenKind.Sum:
                BinaryExpression additionNode = new Sum(_operator, nodeLeft, nodeRight);
                additionNode.CheckNodesSemantic(nodeLeft, _operator, nodeRight);
                nodeLeft = additionNode;
                break;

            case TokenKind.Difference:
                BinaryExpression substractionNode = new Difference(_operator, nodeLeft, nodeRight);
                substractionNode.CheckNodesSemantic(nodeLeft, _operator, nodeRight);
                nodeLeft = substractionNode;
                break;

            case TokenKind.Concatenation:
                BinaryExpression concatenationNode = new Concatenation(_operator, nodeLeft, nodeRight);
                nodeLeft = concatenationNode;
                break;

            case TokenKind.Product:
                BinaryExpression multiplicationNode = new Product(_operator, nodeLeft, nodeRight);
                multiplicationNode.CheckNodesSemantic(nodeLeft, _operator, nodeRight);
                nodeLeft = multiplicationNode;
                break;

            case TokenKind.Quotient:
                BinaryExpression divisionNode = new Quotient(_operator, nodeLeft, nodeRight);
                divisionNode.CheckNodesSemantic(nodeLeft, _operator, nodeRight);
                nodeLeft = divisionNode;
                break;

            case TokenKind.Modulo:
                BinaryExpression modulusNode = new Modulo(_operator, nodeLeft, nodeRight);
                modulusNode.CheckNodesSemantic(nodeLeft, _operator, nodeRight);
                nodeLeft = modulusNode;
                break;

            case TokenKind.Power:
                BinaryExpression powerNode = new Power(_operator, nodeLeft, nodeRight);
                powerNode.CheckNodesSemantic(nodeLeft, _operator, nodeRight);
                nodeLeft = powerNode;
                break;
        }

        return nodeLeft;
    }

    void IndexVarible(Environment letInEnvironment)
    {
        Expect(TokenKind.Identifier);
        string varName = PreviousToken().GetTokenName();
        Expect(TokenKind.EqualEqual);
        letInEnvironment.variables.Add(varName, PaseExpressionLv1(letInEnvironment));


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