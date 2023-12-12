# [H]avana [U]niversity [L]anguage for [K]ompilers

![Este es una imagen](E:/University/THE_HULK/Images/screenshot.9.jpg)

>Esta es una imagen de como luce la interfaz grafica.

Este proyecto se ha divido en dos partes principales, una libreria de clases llamada `THE_HULK` que es la encargada de almacenar todo el motor de procesamiento y una aplicacion de consola llamada `BRUNCE_BANNER`
 
![Este es una imagen](E:/University/THE_HULK/Images/BRUCE_BANNER.jpeg)

>Interfaz grafica.   

![Este es una imagen](E:/University/THE_HULK/Images/THE_HULK.jpeg)

>Libreria de clases.

## Que es Hulk?

HULK es un lenguaje de programación imperativo, funcional, estática y fuertemente tipado. Casi todas las instrucciones en HULK son expresiones.En particular, el subconjunto de HULK a implementar se compone solamente de expresiones que pueden escribirse en una línea.  

Todas las instrucciones en HULK terminan en `;`

## THE_HULK

Entrando en la libreria de clases podemos ver que se divide en dos partes fundamentales:

-Lexer
-Parser

A lo largo de esta exposicion entraremos en detalles en cada uno de ellos.

### Lexer

Dentro de la clase `Lexer` hay una pequenna descripcion de como funciona un Lexer.

> A Lexer, short for lexical analyzer, is a fundamental component of a compiler or interpreter. 
Its main purpose is to break down the source code into smaller units called tokens. 
Tokens are the building blocks of a programming language and represent meaningful elements such as keywords, identifiers, operators, literals, and punctuation.
The Lexer scans the source code character by character and groups them into tokens based on predefined rules and patterns.
It identifies the lexical structure of the code, ignoring whitespace and comments, and produces a stream of tokens that can be further processed by the parser.
In the provided code excerpt, the Lexer class represents the implementation of a Lexer. 
It takes the source code as input and provides a Tokenize method to tokenize the code. 
The Tokenize method uses a loop to iterate over each character of the source code and identifies 
the current character as a token.

Basicamente a modo de resumen el `Lexer` se encarga de procesar el codigo ingresado por el usuario y convertirlo en tokens, los cuales mas adelantes seran procesados por el `Parser` para dar un resultado.

El metodo de Lexing utilizado en THE_HULK es un metodo del tipo `LL(1)` *[L]ef to Right - [L]eftmost - [N]umber of operations*. Basado en ir analizando caracter a caracter el input del usuario mirando siempre el caracter siguiente.

```cs
public class Lexer
{

    public string imput;

    private int currentPos;

    public Lexer(string input)
    {
        this.imput = input;
        currentPos = 0;
    }
}
```

El lexer solo contiene dos propiedades esenciales, `input` utilizada para procesar el input realizado por el usuario y `currentPos` para navegar a travez de los caracteres del input.

Luego, la clase contiene un metodo principal `Tokenizer`, encargado de devolver una lista de tokens. Analicemos dicho metodo:

```cs
public List<Token> Tokenizer()
    {
        // List of tokens
        List<Token> tokens = new();

        while (currentPos < imput.Length)
        {
            // Current character
            char currentChar = imput[currentPos];

            // Ignore whitespace
            if (char.IsWhiteSpace(currentChar))
            {
                currentPos++;
                continue;
            }

            // Add identifier or keyword
            else if (IsLetter(currentChar))
            {
                tokens.Add(UnkwnonType());
            }

            // Add string
            else if (currentChar == '"')
            {
                tokens.Add(String());
            }

            // Add number
            else if (char.IsDigit(currentChar))
            {
                tokens.Add(Number());
            }

            // Add operator
            else if (IsOperator(currentChar))
            {
                tokens.Add(Operator());
            }

            // Add punctuator mark
            else if (IsMark(currentChar))
            {
                tokens.Add(Puntuation_Marks());
            }

            // Add unknown token
            else
            {
                tokens.Add(new TokenCommon(TokenKind.Unknown, currentChar.ToString()));
                System.Console.WriteLine($"! LEXICAL ERROR: \"{tokens.Last()}\" isn't a valid token.");
                currentPos++;
            }
        }

        // Add EOF token
        if (tokens.Last().GetTokenName() != ";")
        {
            System.Console.WriteLine("! SYNTAX ERROR: expression must end with \";\".");
            throw new Exception();
        }

        return tokens;
    }
```

Para procesar la manera de tokenizar separamos en casos:
1. Si lo que encontramos es un espacio en blanco, continuamos. (currenPos++)

2. Si el caracter sobre el que estamos es una letra llamamos a la funcion `UnkwnonType` que se encargara de procesarlo:

```cs
private Token UnkwnonType()
    {

        string unkwnon = "";

        while (currentPos < imput.Length && (IsLetterOrDigit(imput[currentPos]) || imput[currentPos] == '_'))
        {
            unkwnon += imput[currentPos];
            currentPos++;
        }

        if (IsKeyWord(unkwnon))
        {
            return KeyWord(unkwnon);
        }

        else
        {
            return new TokenCommon(TokenKind.Identifier, unkwnon);
        }
    }
```
>Leemos caracter a caracter mientras sea digito, letra o `_`. Cuando el caracter deje de ser uno de los anteriores paramos para comprobar si es keyword, en caso de serlo se annade a los tokens como Keyword, en caso de no serlo se annade como una variable.

3. Si el caracter en que estamos es `"` pasamos a procesar un string con la funcion `String`.

```cs
private Token String()
    {
        currentPos++;
        string temporal = "";

        while (currentPos < imput.Length && imput[currentPos] != '"')
        {
            temporal += imput[currentPos];
            currentPos++;
        }

        Next();
        return new TokenData(TokenKind.String, temporal);
    }
```
>Se annade todo el texto entre comillas y se devuelve como un token.

4. Si el caracter encontrado es un numero pasamos a procesarlo con la funcion `Number`

```cs
private Token Number()
    {

        string number = "";

        while ((currentPos < imput.Length) && (char.IsDigit(imput[currentPos]) || imput[currentPos] == '.'))
        {
            number += imput[currentPos];

            if (IsLetter(LookAhead(1)))
            {
                Console.WriteLine($"! LEXICAL ERROR: \"{number + LookAhead(1)}\" isn't a valid token.");
                throw new Exception();
            }

            currentPos++;
        }

        return new TokenData(TokenKind.Number, Double.Parse(number));
    }
```
>Siempre se verifica que el numero no contenga algo distinto a digito numero o a un `.`, en caso de suceder se lanza una exepcion.

5. Si el caracter es un operador pasamos a tokenizarlo.

```cs
private Token Operator()
    {
        char _operator = imput[currentPos];
        switch (_operator)
        {
            case '+':
                Next();
                return new TokenCommon(TokenKind.Sum, _operator.ToString());

                ...
```

>Se implementa un `switch case` con los tipos de operadores prosibles y se reconoce.

6. Posteriormente pasamos al ultimo de los casos que es reconocer un simbolo de puntuacion:

```cs
private Token Puntuation_Marks()
    {
        char punctuator = imput[currentPos];
        switch (punctuator)
        {
            case '"':
                Next();
                return new TokenCommon(TokenKind.Quote, punctuator.ToString());

                ...
```

>Se procesa de manera omologa al paso anterior.

7. Si entramos en este if podemos afirmar que el usuario ha escrito un input no admitido por el leguage Hulk y pasamos a lanzar una exepcion.

8. Para finalizar y comprobar que se haya terminado la instruccion se comprueba que el ultimo de los tokens sea `;` el EOF, en caso de no serlo se lanza una exepcion.

#### Tokens

Primero implementamos una clase abstracta que nos servira como plantilla para procesar de manera efectiva cada token.

```cs
public abstract class Token
{
    public TokenKind Kind { get; set; }

    public Token(TokenKind kind)
    {
        Kind = kind;
    }

    public TokenKind GetTokenKind() => Kind;
    
    public abstract string GetTokenName();

    public abstract object GetTokenValue();

    public override string ToString() => $"{Kind}";
}
```
  
Como se puede obervar la clase Token contiene la propiedad `Kind` del tipo `TokenKind`:

Para la identificacion del tipado de un token utilizamos un `enum` que contenga todos los tipos posibles de tokens, haciendo mas facil el reconocimiento de un token.

```cs
public enum TokenKind
{
    Identifier, // Variables

    // ===>>> KeyWords <<<===
    IfKeyWord, // if
    InKeyWord, // in
    LetKeyWord, // let
    ElseKeyWord, // else
    FunctionKeyWord, // function
    // ===>>> End of KeyWords <<<===

    // ===>>> Data Types <<<===
    String, // "string"
    Number, // 1.0
    TrueKeyWord, // true
    FalseKeyWord, // false
    // ===>>> End of Data Types <<<===

    // ===>>> Numeric Operators <<<===
    Sum, // +
    Power, // ^
    Modulo, // %
    Product, // *
    Quotient, // /
    Difference, // -
    // ===>>> End of Numeric Operators <<<===

    // ===>>> Logical Operators <<<===
    Or, // ||
    Not, // !
    And, // &&
    UnEqual, // !=
    EqualTo, // ==
    LessThan, // <
    EqualEqual, // ==
    GreaterThan, // >
    Concatenation, // @
    LessOrEqualThan, // <=
    GreaterOrEqualThan, // >=
    // ===>>> End of Logical Operators <<<===

    // ===>>> Symbols <<<===
    End, // ;
    Arrow, // >
    Quote, // "
    Comma, // ,
    Colon, // :
    Semicolon, // ;
    LeftParenthesis, // (
    RightParenthesis, // )
    // ===>>> End of Symbols <<<===

    // ===>>> Comments <<<===
    EndOfFile, // End of File \n
    Unknown, // Unknown
    // ===>>> End of Comments <<<===

}
```

Luego pasamos a entrar en los tres tipos de tokens basicos que podemos encontrar en Hulk.

>(Se pudo habler implementado una unica clase generica para la siguiente tarea)

##### TokenCommon

```cs
public class TokenCommon : Token
{
    public string symbol { get; set; }

    public TokenCommon(TokenKind kind, string _symbol) : base(kind)
    {
        symbol = _symbol;
    }
    public override string GetTokenName() => symbol;

    //This tokens has no value.
    public override object GetTokenValue() => throw new NotImplementedException();

    public override string ToString() => $"{base.Kind}: {symbol}";
}
```
>Dentro de esta clase estan comprendidos principalmente los operadores y simbolos de puntuacion.

##### TokenData

```cs
public class TokenData : Token
{
    public object value { get; set; }

    public TokenData(TokenKind kind, object _value) : base(kind)
    {
        this.value = _value;
    }

    public override string GetTokenName() => value.ToString()!;

    public override object GetTokenValue() => value;

    public override string ToString() => $"{base.Kind}: {value}";
}
```
>Procesamos los principales elementos soportados por el lenguaje: `number`, `string`, `bool` y las variables declaradas por el usuario.

##### TokenKeyword

```cs
public class TokenKeyword : Token
{
    public TokenKeyword(TokenKind kind) : base(kind)
    {
        Kind = kind;
    }

    public override string GetTokenName() => Kind.ToString();

    public override object GetTokenValue() => throw new NotImplementedException();

}
```
>Esta clase se utiliza para procesar las `keywords` del lenguaje Hulk.


### Parser
El parser como mencionado anteriormente es el encargado de procesar los token para devolver un resultado. Para parsear el input del usuario necesitaremos construir un arbol de sintaxis abstracta y resolverlo, pero antes de construirlo necesitaremos contruir antes las estructuras necesarias. Comencemos implementando las expresiones. 

#### Expressions

```cs
public abstract class Expression
{
    public abstract ExpressionKind Kind { get; set; }

    public abstract object? value { get; set; }

    public virtual Environment? environment { get; set; }

    public Expression(Environment _environment)
    {
        environment = _environment;
    }

    public virtual void Evaluate(Environment _environment) { return; }

    public override string ToString() => $"{value}";
    
    public abstract object? GetValue();
}
```

>Anteriormente mencionamos que casi todo en Hulk es una expresion, dado el caso al igual que en los tokens implementaremos una plantilla basica para todo tipo de expresion.

>Igualmente que como vimos en token la clase `Expression` cuenta con la propiedad `Kind` del tipo `ExpressionKind`.

```cs
public enum ExpressionKind
{
    Number, // Doubles
    Bool, // Booleans
    String, // Strings
    Temp, // Variables
}
```

A continuacion pasamos a implementar las plantillas para los tipos de expresiones que mas usaremos.

**BasicExpression**

```cs
public abstract class BasicExpression : Expression
{
    public override Environment? environment { get; set; }
    public BasicExpression(Environment _environment) : base(_environment) { }

    public override string ToString() => $"{value}";
}
```
>Esta sera la plantilla de las expresiones mas basicas del lenguaje, como lo son los string, bools, numeros, o expresiones como log, sin, cos, etc.

>Estas expresiones no contienen nodos puesto que ellas son los nodos en si.

**UnaryExpression**

```cs
public abstract class UnaryExpression : Expression
{
    public abstract Expression node { get; set; }
    public UnaryExpression(Expression _node) : base(null!)
    {
        node = _node;
    }

    public abstract void SemantiCheck(Expression _node);
}
```
>Esta sera la plantilla encargada de ser implementada por las operaciones unarias del lenguaje, como lo es la negacion.

>Este tipo de operaciones contiene un solo nodo, puesto que son unarias.

>Toda clase que herede de esta clase tendra que implementar la funcion `SemantiCheck` para asegurarse que la expresion este bien compuesta semanticamente.

**BinaryExpression**

```cs
public abstract class BinaryExpression : Expression
{
    public Expression nodeLeft;
    public Expression nodeRight;
    public TokenKind Operator;
    public override Environment? environment { get; set; }

    public BinaryExpression(TokenKind _operator, Expression _nodeLeft, Expression _nodeRight) : base(null!)
    {
        nodeLeft = _nodeLeft;
        nodeRight = _nodeRight;
        Operator = _operator;
    }

    public virtual void SemantiCheck(Expression nodeLeft, TokenKind operator_, Expression nodeRight)
    {
        if ((nodeLeft!.Kind != ExpressionKind.Number && nodeLeft.Kind != ExpressionKind.Temp) || (nodeRight!.Kind != ExpressionKind.Number && nodeRight.Kind != ExpressionKind.Temp))
        {
            System.Console.WriteLine($"! SEMANTIC ERROR: \"{operator_}\" cannot be used between \"{nodeLeft.Kind}\" and \"{nodeRight!.Kind}\".");
            throw new Exception();
        }
    }
}
```
>Esta plantilla funciona de manera omologa a `UnaryExpression` lo que contiene un nodo extra y el chequeo de semantica se verifica sobre que los dos nodos sean del mismo tipado.

##### BasicExpression

Entrando en las expresiones mas basicas analizaremos `print` pero el resto de las clases se comportan de forma semejante

```cs
public class Print : BasicExpression
{
    public override ExpressionKind Kind { get; set; }

    public override object? value { get; set; }

    List<Expression> parameters;

    public Print(List<Expression> _parameters, Environment _environment) : base(_environment)
    {
        this.parameters = _parameters;
        Kind = ExpressionKind.Temp;
        if (_parameters.Count != 1)
        {
            System.Console.WriteLine($"! SEMANTIC ERROR: function \"cos\" needs 1 parameter(s), but {_parameters.Count} were given.");
            throw new Exception();
        }
    }

    public override void Evaluate(Environment _environment)
    {
        parameters.First().Evaluate(_environment);
        value = parameters.First().value;

        System.Console.WriteLine(value);
    }

    public override object? GetValue() => value;
}
```
>Todas las expresiones basicas tienen un paramentro `paramenter` o `parameters` porque expresiones mas simples como cos, sin, reciben un solo parametro.

>Luego tenemos un chequeo semantico para comprobar que que la cantidad de parametros con los que se llamo sean los mismo que necesita la expresion

>Un ejemplo basico de lo anterior es que la funcion `Logarithm` necesita exactamente dos parametros para ejecutarse de manera efectiva, luego si se llama con una cantidad distinta a 2 podemos afirmar que existe un error semantico.

>Luego implementa el metodo`Evaluate` que en el caso dado es imprimir el resultado de la(s) expresion(es) dada(s).

**A continuacion ejemplos de las funciones**

![Este es una imagen](E:/University/THE_HULK/Images/sin.jpg)

>Ejemplo del metodo `Sin`.

![Este es una imagen](E:/University/THE_HULK/Images/cos.jpg)

>Ejemplo del metodo `Cos`.

![Este es una imagen](E:/University/THE_HULK/Images/log.jpg)

>Ejemplo del metodo `Logarithm`.

![Este es una imagen](E:/University/THE_HULK/Images/exp.jpg)

>Ejemplo del metodo `Exponential`.

![Este es una imagen](E:/University/THE_HULK/Images/sqrt.jpg)

>Ejemplo del metodo `SquareRoot`.


##### UnaryExpression

Entrando en las expresiones unarias podemos analizar `Not` para entender su funcionamineto.

```cs
public class Not : UnaryExpression
{
    public override Expression node { get; set; }
    public override ExpressionKind Kind { get; set; }
    public override object? value { get; set; }
    public override Environment? environment { get; set; }

    public Not(Expression _node) : base(_node)
    {
        Kind = ExpressionKind.Bool;
        this.node = _node;
    }

    public override void SemantiCheck(Expression node)
    {
        if (this.node.Kind != ExpressionKind.Bool && this.node.Kind != ExpressionKind.Temp)
        {
            System.Console.WriteLine($"! SEMANTIC ERROR: operator \"{TokenKind.Not}\" cannot be applied to \"{this.node.Kind}\".");
            throw new Exception();
        }
    }

    public override void Evaluate(Environment _environment)
    {
        node.Evaluate(environment!);
        value = !(bool)node.GetValue()!;
    }

    public override object? GetValue() => value;
}
```
>En esta clase se realiza un chequeo semantico para comprobar que sea un elemento booleano sobre el cual se realizara la operacion de negacion, en caso de no serlo lanzamos una exepcion.

>De no haber errores semanticos procedemos a evaluar el cual solo invertira el valor sobre el cual fue operado.

![Este es una imagen](E:/University/THE_HULK/Images/not.jpg)

>Ejemplo de negacion

##### BinaryExpression

En el caso las operaciones binarias existen 3 tipos en las que las podemos dividir. Miremos cada una de ellas.

###### BooleanExpression

Adentrandonos en las expresiones booleanas tomaremos como ejemplo principal `GreaterOrEqualThan`, el resto de las operaciones booleanas se comportan de manera semejante.

```cs
public class GreaterOrEqualThan : BinaryExpression
{
    public GreaterOrEqualThan(TokenKind _operator, Expression nodeLeft, Expression nodeRight) :
    base(_operator, nodeLeft, nodeRight)
    {
        Kind = ExpressionKind.Bool;
    }


    public override ExpressionKind Kind { get; set; }
    public override object? value { get; set; }



    public override void Evaluate(Environment _environment)
    {
        nodeLeft.Evaluate(_environment);
        nodeRight.Evaluate(_environment);

        if (nodeLeft.Kind != ExpressionKind.Number || nodeRight.Kind != ExpressionKind.Number)
        {
           System.Console.WriteLine($"! SEMANTIC ERROR: \"{Operator}\" cannot be used between \"{nodeLeft.Kind}\" and \"{nodeRight.Kind}\".");
            throw new Exception();
        }

        value = (double)nodeLeft.GetValue()! >= (double)nodeRight.GetValue()!;
    }

    public override object? GetValue() => value;
}
```

>Esta clase cuenta con un metodo principal que se encarga de evaluar la accion, en caso del usuario estar intentando comparar elementos que no sean numeros con este operador se procede a lanzar una exepcion.

![Este es una imagen](E:/University/THE_HULK/Images/And.jpg)

>Ejemplo del metodo `And`.

![Este es una imagen](E:/University/THE_HULK/Images/Or.jpg)

>Ejemplo del metodo `Or`.

![Este es una imagen](E:/University/THE_HULK/Images/GreaterThan.jpg)

>Ejemplo del metodo `GreaterThan`.

![Este es una imagen](E:/University/THE_HULK/Images/LessThan.jpg)

>Ejemplo del metodo `LessThan`.

![Este es una imagen](E:/University/THE_HULK/Images/GreaterOrEqualThan.jpg)

>Ejemplo del metodo `GreaterOrEqualThan`.

![Este es una imagen](E:/University/THE_HULK/Images/LessOrEqualThan.jpg)

>Ejemplo del metodo `LessOrEqualThan`.

![Este es una imagen](E:/University/THE_HULK/Images/EqualTo.jpg)

>Ejemplo del metodo `EqualTo`.

###### NumericExpression

En el caso de las expresiones numericas tomaremos como ejemplo principal `Sum`, el resto de las operaciones numericas se comportan de manera semejante.

```cs
public class Sum : BinaryExpression
{
    public override ExpressionKind Kind { get; set; }

    public override object? value { get; set; }

    public Sum(TokenKind _operator, Expression nodeLeft, Expression nodeRight) :
    base(_operator, nodeLeft, nodeRight)
    {
        Kind = ExpressionKind.Number;
    }

    public override void Evaluate(Environment _environment)
    {
        nodeLeft!.Evaluate(_environment);
        nodeRight!.Evaluate(_environment);

        if (nodeLeft.Kind != ExpressionKind.Number || nodeRight.Kind != ExpressionKind.Number)
        {
            System.Console.WriteLine($"! SEMANTIC ERROR: \"{Operator}\" cannot be used between \"{nodeLeft.Kind}\" and \"{nodeRight.Kind}\".");
            throw new Exception();
        }

        value = (double)nodeLeft.value! + (double)nodeRight.value!;
    }

    public override object? GetValue() => value;
}
```

>El metodo principal de la clase es `Evaluate` el cual comprueba que el usuario este intentando sumar dos numeros, de no ser el caso lanza una exepcion, de ser el caso devuelve la suma de ambos numeros que guarda como nodos izquierdo y derecho respectivamente.

![Este es una imagen](E:/University/THE_HULK/Images/Sum.jpg)

>Ejemplo del metodo `Sum`.

![Este es una imagen](E:/University/THE_HULK/Images/Difference.jpg)

>Ejemplo del metodo `Difference`.

![Este es una imagen](E:/University/THE_HULK/Images/Modulo.jpg)

>Ejemplo del metodo `Modulo`.

![Este es una imagen](E:/University/THE_HULK/Images/Quotient.jpg)

>Ejemplo del metodo `Quotient`.

![Este es una imagen](E:/University/THE_HULK/Images/Product.jpg)

>Ejemplo del metodo `Product`.

![Este es una imagen](E:/University/THE_HULK/Images/Power.jpg)

>Ejemplo del metodo `Power`.

**Error:** Dado que la division por 0 no esta definida en el metodo `Evaluate` de la clase dada se debe hacer una verificacion.

![Este es una imagen](E:/University/THE_HULK/Images/QuotientCero.jpg)

>Ejemplo del error de la division por 0.

###### OtherExpression

En este caso solo tenemos una operacion de muestra, que seria el caso de `@` la concatenacion, pero este apartado queda abierto para la implementacion de nuevas expresiones binarias que no impliquen elementos boobleanos en su totalidad o numeros en su totalidad.

```cs
public class Concatenation : BinaryExpression
{
    public Concatenation(TokenKind _operator, Expression nodeLeft, Expression nodeRight) :
     base(_operator, nodeLeft, nodeRight)
    {
        Kind = ExpressionKind.String;
    }

    public override ExpressionKind Kind { get; set; }
    public override object? value { get; set; }

    public override void Evaluate(Environment _environment)
    {
        nodeLeft!.Evaluate(_environment);
        nodeRight!.Evaluate(_environment);
        
        value = nodeLeft.GetValue()!.ToString()! + nodeRight.GetValue()!.ToString()!;
    }

    public override object? GetValue() => value;
}
```

>El funcionamiento es de forma omologa al de la suma.

#### HULK Expressions

Luego de haber observado las expresiones basicas del lenguaje procedamos a analizar las expresiones endemicas de este.

##### Let_In

En HULK es posible declarar variables usando la expresión `let-in`. En general, una expresión `let-in` consta de una o más declaraciones de variables, y un cuerpo, que puede ser cualquier expresión donde además se pueden utilizar las variables declaradas en el `let`. Fuera de una expresión `let-in` las variables dejan de existir. El valor de retorno de una expresión `let-in` es el valor de retorno del cuerpo.

```cs
public class Let_In : Expression
{
    public override Environment? environment { get; set; }
    public Expression parameter;
    public override ExpressionKind Kind { get; set; }
    public Dictionary<string, Expression> Variables { get; set; }
    public override object? value { get; set; }

    public Let_In(Expression _parameter, Environment _environment) : base(_environment)
    {
        Variables = new Dictionary<string, Expression>();
        Kind = ExpressionKind.Temp;
        parameter = _parameter;
    }

    public override void Evaluate(Environment _environment)
    {
        parameter.Evaluate(environment!);
        value = parameter.GetValue();

        //make sure the type of element u are using
            //! SEMANTIC ERROR: "Sum" cannot be used between "Number" and "Temp".
        if (value is bool) { Kind = ExpressionKind.Bool; } 
        if (value is string) { Kind = ExpressionKind.String; }
        if (value is double) { Kind = ExpressionKind.Number; }
    }

    public override object? GetValue() => value;
}
```
>Implementacion de la clase `Let_In`

Adentremonos en la implementacion de esta clase mediante un ejemplo:

```cs
print(7 + (let x = 2 in x * x));
```

Omitiendo la primera parte que se explicara posteriormente llegamos a la indentificacion del Let in en el parser. Una vez hecho esto procedemos a llamar a la funcion `ParseLet_in`

```cs
Expression ParseLet_in(Environment privateEnvironment)
    {
        Eat();

        Let_In letIn = new(null!, privateEnvironment.CreateChild());

        IndexVarible(letIn.environment!);

        WatchFor(TokenKind.InKeyWord);

        letIn.parameter = ParseExpressionLv1(letIn.environment!);

        return letInExpression;
    }
```

>Este metodo se encargara de indexar la variable con el valor otorgado por el usario.

Analicemos el metodo `IndexVariable`

```cs
void IndexVarible(Environment letInEnvironment)
    {
        WatchFor(TokenKind.Identifier);

        string name = PreviousToken().GetTokenName();

        WatchFor(TokenKind.EqualEqual);

        letInEnvironment.variables.Add(name, ParseExpressionLv1(letInEnvironment));


        if (currentToken.Kind == TokenKind.Comma)
        {
            Eat();
            IndexVarible(letInEnvironment);
        }
    }
```
>Primeramente nos aseguramos mediante el metodo `WatchFor` que luego de un let lo que venga sea un identificador (una variable). Guardamos el nombre asignado por el usuario.

>Posteriormente verificamos que tenga la instruccion de asignacion `=`

>Para finalizar se indexa en el estado local del let in parseando el miembro derecho de la asignacion `=` (environment)(posteriormente se explicara detalladamente lo que es el environment).

Una vez concluidos estos pasos, se procede a regresar en la recursion del metodo `ParseLet_in`.

>Procedemos a hacer el `WatchFor` para encontrar la keyword `in` que indicara posteriormente la instruccion a realizar.

>Procedemos a parsear el miembro derecho del `in`.

>Para finalmente retornar una Expresion del tipo `Let_in`

Este caso en particular retorna 11.
##### If_else

Las condiciones en HULK se implementan con la expresión `if-else`, que recibe una expresión booleana entre paréntesis, y dos expresiones para el cuerpo del `if` y el `else` respectivamente. Siempre deben incluirse ambas partes. Como `if-else` es una expresión, se puede usar dentro de otra expresión (al estilo del operador ternario en C#):

```cs
public class If_Else : Expression
{
    public Expression condition;
    public Expression nodeLeft;
    public Expression nodeRight;

    public If_Else(Expression _condition, Expression _nodeLeft, Expression _nodeRight) : base(null!)
    {
        condition = _condition;
        nodeLeft = _nodeLeft;
        nodeRight = _nodeRight;
        Kind = ExpressionKind.Temp;
    }

    public override ExpressionKind Kind { get; set; }
    public override object? value { get; set; }
    public override Environment? environment { get; set; }

    public override void Evaluate(Environment _environment)
    {
        condition!.Evaluate(_environment);

        if (condition.value is true)
        {
            nodeLeft.Evaluate(_environment);
            value = nodeLeft.value;
            Kind = nodeLeft.Kind;
        }
        else
        {
            nodeRight.Evaluate(_environment);
            value = nodeRight!.GetValue();
            Kind = nodeRight.Kind;
        }
    }

    public override object? GetValue() => value;
}
```
>Implementacion de la clase `If_Else`

Analicemos la clase mediante el ejemplo:

```cs
let a = 42 in if (a % 2 == 0) print("Even") else print("odd");
```

Procesamos la primera parte como acabamos de observar en el `Let_in`. Una vez en el parse tengamos procesado todo el `let` y nos encontremos en el `in` pasamos a parsear el miembro derecho del in, donde nos encontraremos con el metodo `ParseIf`.

```cs
Expression PaseIf(Environment privateEnvironment)
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
```
>Se parsea la condicion con el metodo `ParseExpressionLv1` el cual explicaremos detalladamente mas adelante, el cual se encarga de ir parseando recursivamente los elementos dentro de la condicional `If`.

>Se eplica como queda parseado de una mejor manera mediante un diagrama.