using System.Globalization;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum TokenType {
    Error,
    EOF,
    Ident,
    Number,
    Equal,
    Plus,
    Minus,
    Star,
    Slash,
    Carat,
    OpenParen,
    CloseParen,
    Comma,
    X,
    E,
    PI,
}

public enum Precedence {
    None,
    Term, // + -
    Factor, // * /
    Power,
    Call,
    Full,
}

public struct Token {
    public TokenType Type;
    public string Lexeme;
    public object Value;

    public Token(TokenType type, string lexeme, object value) {
        Type = type;
        Lexeme = lexeme;
        Value = value;
    }

    public override string ToString() {
        return base.ToString() + " Type: " + Type + " Lexeme: " + Lexeme + " Value: " + Value;
    }
}

public class Lexer {
    private string current_source;
    private int current_index;

    public Lexer(string source) {
        current_source = source;
        current_index = 0;
    }

    public void Reset(string source) {
        current_source = source;
        current_index = 0;
    }

    private char Current() {
        if (current_index == current_source.Length) return '\0';
        return current_source[current_index];
    }

    private char Peek() {
        if (current_index == current_source.Length - 1) return '\0';
        return current_source[current_index + 1];
    }

    public Token LexToken() {
        while (char.IsWhiteSpace(Current())) current_index++;
        if (current_index == current_source.Length) return new Token(TokenType.EOF, "", null);
        switch (Current()) {
            case '+': current_index++; return new Token(TokenType.Plus, "+", null);
            case '-': current_index++; return new Token(TokenType.Minus, "-", null);
            case '*': current_index++; return new Token(TokenType.Star, "*", null);
            case '/': current_index++; return new Token(TokenType.Slash, "/", null);
            case '^': current_index++; return new Token(TokenType.Carat, "^", null);
            case '(': current_index++; return new Token(TokenType.OpenParen, "(", null);
            case ')': current_index++; return new Token(TokenType.CloseParen, ")", null);
            case '=': current_index++; return new Token(TokenType.Equal, "=", null);
            case ',': current_index++; return new Token(TokenType.Comma, ",", null);
            case 'x': {
                if (!char.IsLetterOrDigit(Peek())) {
                    current_index++;
                    return new Token(TokenType.X, "x", null);
                } else goto default;
            }
            case 'p': {
                if (Peek() == 'i') {
                    current_index++;
                    current_index++;
                    return new Token(TokenType.PI, "pi", null);
                } else goto default;
            }
            case 'e': {
                if (!char.IsLetterOrDigit(Peek())) {
                    current_index++;
                    return new Token(TokenType.E, "e", null);
                } else goto default;
            }
            default: {
                TokenType type = TokenType.Error;
                int start_index = current_index;
                object value = null; 
                if (char.IsDigit(Current())) {
                    current_index++;
                    while (char.IsDigit(Current()))
                        current_index++;
                    if (Current() == '.') current_index++;
                    while (char.IsDigit(Current()))
                        current_index++;
                    
                    string substr = current_source.Substring(start_index, current_index - start_index);
                    type = TokenType.Number;
                    float.TryParse(substr, NumberStyles.Any, CultureInfo.InvariantCulture, out float v);
                    value = v;
                } else if (Current() == '.') {
                    current_index++;
                    while (char.IsDigit(Current()))
                        current_index++;
                    string substr = current_source.Substring(start_index + 1, current_index - start_index - 1);
                    type = TokenType.Number;
                    float.TryParse("0." + substr, NumberStyles.Any, CultureInfo.InvariantCulture, out float v);
                    value = v;
                } else if (char.IsLetter(Current())) {
                    current_index++;
                    while (char.IsLetterOrDigit(Current()))
                        current_index++;
                    type = TokenType.Ident;
                }
                return new Token(type, current_source.Substring(start_index, current_index - start_index), value);
            }
        }
    }
}

public class Parser {
    private Lexer lexer;
    private Token prev;
    private Token curr;
    private Token next;
    public bool errored;
    private List<string> function_context_variables;

    public Parser(Lexer source_lexer) {
        lexer = source_lexer;
        errored = false;
        Advance(); // Fill in next
        Advance(); // Fill in curr
    }

    public void Reset(Lexer source_lexer) {
        lexer = source_lexer;
        errored = false;
        Advance(); // Fill in next
        Advance(); // Fill in curr    
    }

    private void Error(string message) {
        errored = true;
        Debug.Log(message);
    }

    private Token Advance() {
        prev = curr;
        curr = next;
        next = lexer.LexToken();
        return prev;
    }

    private bool Match(TokenType type) {
        if (curr.Type == type) {
            Advance();
            return true;
        }
        return false;
    }

    private bool Eat(TokenType type) {
        if (curr.Type == type) {
            Advance();
            return true;
        }
        Error("Token expected " + type.ToString() + " but got " + curr.Type.ToString());
        return false;
    }

    private Precedence GetPrec(Token token) {
        switch (token.Type) {
            case TokenType.Plus: case TokenType.Minus: return Precedence.Term;
            case TokenType.Star: case TokenType.Slash: return Precedence.Factor;
            case TokenType.Carat: return Precedence.Power;
            case TokenType.OpenParen: return Precedence.Call;
        }
        return Precedence.None;
    }

    public ASTNode ParsePrefixExpr() {
        ASTNode ret = null;
        switch (curr.Type) {
            case TokenType.Number:
                Advance();
                ret =  new NumberASTNode(prev);
                break;
            
            case TokenType.Plus:
            case TokenType.Minus:
                Advance();
                Token op = prev;
                ASTNode operand = ParsePrefixExpr();
                ret =  new UnaryASTNode(op, operand);
                break;
            
            case TokenType.OpenParen:
                Advance();
                ASTNode inner = ParseExpression();
                Match(TokenType.CloseParen);
                ret =  inner;
                break;

            case TokenType.Ident:
                Advance();
                Token name = prev;

                if (Match(TokenType.OpenParen)) {
                    List<ASTNode> args = new List<ASTNode>();
                    while (!Match(TokenType.CloseParen)) {
                        args.Add(ParseExpression());
                        if (!Match(TokenType.Comma)) {
                            Eat(TokenType.CloseParen);
                            break;
                        }
                    }

                    if (!FunctionTable.intrinsic_functions.ContainsKey(name.Lexeme)) {
                        // User defined function
                        if (Match(TokenType.Equal)) {
                            ASTNode value = ParseExpression();
                            List<string> var_names = new List<string>();
                            foreach (ASTNode node in args) {
                                if (node.type != ASTNodeType.Ident) {
                                    Error("One or more arguments in function definition " + name.Lexeme + " is not an identifier");
                                } else var_names.Add(((IdentASTNode)node).ident.Lexeme);
                            }
                            FunctionTable.user_defined_functions[name.Lexeme] = new CustomFunction(value, var_names.Count, var_names.ToArray());
                            ret =  new NumberASTNode(new Token(TokenType.Number, "0", 0)); // Is this good?
                            break;
                        } else {
                            // Is an actual call here
                            if (!FunctionTable.user_defined_functions.ContainsKey(name.Lexeme)) {
                                Error("Unknown function called " + name.Lexeme);
                            } else if (FunctionTable.user_defined_functions[name.Lexeme].arity != args.Count) {
                                Error("Wrong amount of arguments passed to the function" + name.Lexeme + ". Expected " +
                                    FunctionTable.user_defined_functions[name.Lexeme].arity + " got " + args.Count);
                            }
                            ret =  new CallASTNode(name, args.ToArray());
                            break;
                        }
                    } else if (FunctionTable.intrinsic_functions[name.Lexeme].arity != args.Count) {
                        Error("Wrong amount of arguments passed to the function" + name.Lexeme + ". Expected " +
                            FunctionTable.user_defined_functions[name.Lexeme].arity + " got " + args.Count);
                    }
                    ret =  new IntrinsicCallASTNode(name, args.ToArray());
                    break;
                } else {
                    ret =  new IdentASTNode(name);
                    break;
                }
            
            case TokenType.X:
                Advance();
                ret =  new XASTNode(prev);
                break;
            
            case TokenType.E:
                Advance();
                ret =  new EASTNode(prev);
                break;
                
            case TokenType.PI:
                Advance();
                ret =  new PIASTNode(prev);
                break;
        }

        if (ret != null) {
            if (curr.Type != TokenType.EOF && curr.Type != TokenType.Plus && curr.Type != TokenType.Minus && curr.Type != TokenType.Star
                        && curr.Type != TokenType.Slash && curr.Type != TokenType.Carat && curr.Type != TokenType.CloseParen) {
                ASTNode right = ParsePrefixExpr();
                return new BinaryASTNode(ret, new Token(TokenType.Star, "*", null), right);
            } else {
                return ret;
            }
        }

        Error("Unknown Expression starting with the token " + curr.Type.ToString());
        return null;
    }

    public ASTNode ParseInfixExpr(Token op, Precedence prec, ASTNode lhs) {
        switch (op.Type) {
            case TokenType.Plus:
            case TokenType.Minus:
            case TokenType.Star:
            case TokenType.Slash:
            case TokenType.Carat:
                ASTNode rhs = ParseExpression(prec);
                return new BinaryASTNode(lhs, op, rhs);
        }
        Error("Unknown Binary Expression for the token " + op);
        return null;
    }

    public ASTNode ParseExpression(Precedence prec = Precedence.None) {
        if (errored) return null;
        ASTNode left = ParsePrefixExpr();
        // @Error handling check node validity
        Precedence curr_prec = GetPrec(curr);

        if (curr_prec == Precedence.None) return left;
        Advance();
        Token op = prev;
        while (true) {
            if (errored) return null;
            if (GetPrec(op) == Precedence.None) break;
            if (GetPrec(op) >= prec) {
                left = ParseInfixExpr(op, GetPrec(op) + 1, left);
                // @Error handling check node validity
                op = prev;
            } else break;
        }

        return left;
    }
}

public class Evaluator {
    private Lexer lexer;
    private Parser parser;
    public bool errored;

    private Dictionary<string, ASTNode> current_function_context = new Dictionary<string, ASTNode>();

    public Evaluator(string s) {
        lexer = new Lexer(s);
        errored = false;
        parser = new Parser(lexer);
    }

    public void Reset(string s) {
        lexer.Reset(s);
        errored = false;
        parser.Reset(lexer);
    }

    private void Error(string message) {
        errored = true;
        Debug.Log(message);
    }

    public void Dump(ASTNode ast, StringBuilder builder, int indent = 0) {
        for (int i = 0; i < indent; i++) builder.Append("\t");
        builder.Append(ast.ToString()).Append("\n");
        switch (ast.type) {
            case ASTNodeType.IntrinsicCall:
                foreach (ASTNode iter in ((IntrinsicCallASTNode)ast).arguments)
                    Dump(iter, builder, indent + 1);
                break;
            case ASTNodeType.Unary:
                Dump(((UnaryASTNode)ast).operand, builder, indent + 1);
                break;
            case ASTNodeType.Binary:
                Dump(((BinaryASTNode)ast).left, builder, indent + 1);
                Dump(((BinaryASTNode)ast).right, builder, indent + 1);
                break;
        }
    }

    public void Dump(ASTNode ast) {
        StringBuilder builder = new StringBuilder();
        Dump(ast, builder);
        Debug.Log(builder.ToString());
    }

    public float Evaluate(ASTNode ast, float x_val) {
        if (ast == null || errored) return 0.0f;

        switch (ast.type) {
            case ASTNodeType.Ident: {
                IdentASTNode ident = (IdentASTNode) ast;
                if (current_function_context.ContainsKey(ident.ident.Lexeme)) {
                    return Evaluate(current_function_context[ident.ident.Lexeme], x_val);
                } else return 0.0f;
            }
            case ASTNodeType.Number: return (float) ((NumberASTNode)ast).number_token.Value;

            case ASTNodeType.Unary: {
                UnaryASTNode unary = (UnaryASTNode) ast;
                float op_value = Evaluate(unary.operand, x_val);
                
                if (unary.op.Type == TokenType.Plus) return op_value;
                else if (unary.op.Type == TokenType.Minus) return -op_value;
            } break;

            case ASTNodeType.Binary: {
                BinaryASTNode binary = (BinaryASTNode) ast;
                float left = Evaluate(binary.left, x_val);
                float right = Evaluate(binary.right, x_val);

                if (binary.op.Type == TokenType.Plus) return left + right;
                else if (binary.op.Type == TokenType.Minus) return left - right;
                else if (binary.op.Type == TokenType.Star) return left * right;
                else if (binary.op.Type == TokenType.Slash) return left / right;
                else if (binary.op.Type == TokenType.Carat) return Mathf.Pow(left, right);
            } break;

            case ASTNodeType.IntrinsicCall: {
                IntrinsicCallASTNode intrinsic_call = (IntrinsicCallASTNode) ast;
                List<float> args_evaluated = new List<float>();
                foreach (ASTNode arg_node in intrinsic_call.arguments)
                    args_evaluated.Add(Evaluate(arg_node, x_val));
                return FunctionTable.intrinsic_functions[intrinsic_call.func_name.Lexeme].function(args_evaluated.ToArray());
            }

            case ASTNodeType.Call: {
                CallASTNode call = (CallASTNode) ast;
                CustomFunction fn = FunctionTable.user_defined_functions[call.func_name.Lexeme];
                
                for (int i = 0; i < fn.arity; i++) {
                    current_function_context.Add(fn.var_names[i], call.arguments[i]);
                }
                
                float value = Evaluate(fn.function, x_val);
                current_function_context.Clear();
                return value;
            }

            case ASTNodeType.X: {
                return x_val;
            }

            case ASTNodeType.E: {
                return 2.718281828459045f;
            }

            case ASTNodeType.PI: {
                return Mathf.PI;
            }
        }
        return 0.0f;
    }

    public ASTNode Parse() {
        ASTNode node = parser.ParseExpression();
        errored = parser.errored;
        parser.errored = false;
        return node;
    }
}
