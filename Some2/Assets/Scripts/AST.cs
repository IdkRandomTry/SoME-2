public enum ASTNodeType {
    Error,
    Variable,
    Number,
    Unary,
    Binary,
    X,
    E,
}

public abstract class ASTNode {
    public ASTNodeType type;

    protected ASTNode(ASTNodeType type) {
        this.type = type;
    }
}

public class VariableASTNode : ASTNode {
    public Token var_token;

    public VariableASTNode(Token token) : base(ASTNodeType.Variable) {
        this.var_token = token;
    }
}

public class XASTNode : ASTNode {
    public Token token;

    public XASTNode(Token token) : base(ASTNodeType.X) {
        this.token = token;
    }
    
    public override string ToString() {
        return "X AST Node " + token.Lexeme;
    }
}

public class EASTNode : ASTNode {
    public Token token;

    public EASTNode(Token token) : base(ASTNodeType.E) {
        this.token = token;
    }
    
    public override string ToString() {
        return "E AST Node e";
    }
}

public class NumberASTNode : ASTNode {
    public Token number_token;

    public NumberASTNode(Token token) : base(ASTNodeType.Number) {
        number_token = token;
    }
    
    public override string ToString() {
        return "Number AST Node " + number_token.Lexeme;
    }
}

public class UnaryASTNode : ASTNode {
    public Token op;
    public ASTNode operand;

    public UnaryASTNode(Token op, ASTNode operand) : base(ASTNodeType.Unary) {
        this.op = op;
        this.operand = operand;
    }
    
    public override string ToString() {
        return "Unary AST Node " + op.Lexeme + "    (" + operand.ToString() + ")";
    }
}

public class BinaryASTNode : ASTNode {
    public ASTNode left;
    public Token op;
    public ASTNode right;

    public BinaryASTNode(ASTNode left, Token op, ASTNode right) : base(ASTNodeType.Binary) {
        this.left = left;
        this.op = op;
        this.right = right;
    }
    
    public override string ToString() {
        return "Binary AST Node (" + left.ToString() + ")    " + op.Lexeme + "    (" + right.ToString() + ")";
    }
}
