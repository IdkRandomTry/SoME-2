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
    OpenParen,
    CloseParen,
}

public enum Precedence {
    None,
    Call, // TBA
    Factor,
    Term,
    Full,
}

public class Token {
    public TokenType Type;
    public string Lexeme;
    public object Value;

    public Token(TokenType type, string lexeme, object value) {
        Type = type;
        Lexeme = lexeme;
        Value = value;
    }
}

public class Lexer {
    private string current_source;
    private int current_index;

    public Lexer(string source) {
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
        if (current_index == current_source.Length - 1) return new Token(TokenType.EOF, null, null);
        switch (Current()) {
            case '+': current_index++; return new Token(TokenType.Plus, "+", null);
            case '-': current_index++; return new Token(TokenType.Minus, "-", null);
            case '*': current_index++; return new Token(TokenType.Star, "*", null);
            case '/': current_index++; return new Token(TokenType.Slash, "/", null);
            case '(': current_index++; return new Token(TokenType.OpenParen, "(", null);
            case ')': current_index++; return new Token(TokenType.CloseParen, ")", null);
            case '=': current_index++; return new Token(TokenType.Equal, "=", null);
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
                    while (char.IsLetter(Current()))
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

    public Parser(Lexer source_lexer) {
        lexer = source_lexer;
        Advance(); // Fill in next
        Advance(); // Fill in curr
    }

    private Token Advance() {
        prev = curr;
        curr = next;
        next = lexer.LexToken();
        return prev;
    }

    private Precedence GetPrec(Token token) {
        switch (token.Type) {
            case TokenType.Plus: case TokenType.Minus: return Precedence.Term;
            case TokenType.Star: case TokenType.Slash: return Precedence.Factor;
            case TokenType.OpenParen: return Precedence.Call;
        }
        return Precedence.None;
    }

    public ASTNode ParseExpression(Precedence prec) {
        ASTNode left = Parse();
        Token op = Advance();
        Precedence new_prec = GetPrec(op);

        // TODO(voxel): precedence parsing

        return left;
    }

    public ASTNode Parse() {
        switch (curr.Type) {
            case TokenType.Ident: {
                if (next.Type == TokenType.OpenParen) {
                    // TODO(voxel): soon:tm:
                } else {
                    Advance();
                    return new VariableASTNode(prev);
                }
            } break;

            case TokenType.Number: {
                ParseExpression(Precedence.None);
            } break;

            case TokenType.Error: case TokenType.EOF:
            case TokenType.Equal: case TokenType.Star:
            case TokenType.Slash:
                Debug.Log("TODO: Implement Better Error Handling!!");
            break;
        }
        return null;
    }
}

public class Evaluator {
    private Lexer lexer;
    private Parser parser;

    public Evaluator(string s) {
        lexer = new Lexer(s);
        parser = new Parser(lexer);
    }

    public float Evaluate() {
        return 0;
    }
}
