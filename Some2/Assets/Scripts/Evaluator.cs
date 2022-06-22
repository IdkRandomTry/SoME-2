using System;
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
}

public enum Precedence {
    None,
    Power,
    Call,
    Factor,
    Term,
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
                    value = float.Parse(substr);
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
        errored = true;
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
        switch (curr.Type) {
            case TokenType.Number:
                Advance();
                return new NumberASTNode(prev);
            
            case TokenType.Plus:
            case TokenType.Minus:
                Advance();
                Token op = prev;
                ASTNode operand = ParsePrefixExpr();
                return new UnaryASTNode(op, operand);
            
            case TokenType.OpenParen:
                Advance();
                ASTNode inner = ParseExpression();
                Match(TokenType.CloseParen);
                return inner;

            case TokenType.Ident:
                Advance();
                Token name = prev;
                Match(TokenType.OpenParen);
                List<ASTNode> args = new List<ASTNode>();
                while (!Match(TokenType.CloseParen)) {
                    args.Add(ParseExpression());
                    if (!Match(TokenType.Comma)) {
                        Eat(TokenType.CloseParen);
                        break;
                    }
                }
                return new CallIntrinsicASTNode(name, args.ToArray());

            case TokenType.X:
                Advance();
                return new XASTNode(prev);
            
            case TokenType.E:
                Advance();
                return new EASTNode(prev);
        }
        errored = true;
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
        errored = true;
        return null;
    }

    public ASTNode ParseExpression(Precedence prec = Precedence.Full) {
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
            if (GetPrec(op) <= prec) {
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

    public void Dump(ASTNode ast, StringBuilder builder, int indent = 0) {
        for (int i = 0; i < indent; i++) builder.Append("\t");
        builder.Append(ast.ToString()).Append("\n");
        switch (ast.type) {
            case ASTNodeType.IntrinsicCall:
                foreach (ASTNode iter in ((CallIntrinsicASTNode)ast).arguments)
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
                CallIntrinsicASTNode intrinsic_call = (CallIntrinsicASTNode) ast;
                List<float> args_evaluated = new List<float>();
                foreach (ASTNode arg_node in intrinsic_call.arguments)
                    args_evaluated.Add(Evaluate(arg_node, x_val));
                Func<float[], float> intrinsic = null;
                FunctionTable.inbuilt_functions.TryGetValue(intrinsic_call.intrinsic_name.Lexeme, out intrinsic);
                return intrinsic(args_evaluated.ToArray());
            }

            case ASTNodeType.X: {
                return x_val;
            }

            case ASTNodeType.E: {
                return 2.718281828459045f;
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
