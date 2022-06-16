public enum ASTNodeType {
    Error,
    Variable,
    Number,
    Unary,
    Binary,
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

public class NumberASTNode : ASTNode {
    public Token number_token;

    public NumberASTNode(Token token) : base(ASTNodeType.Number) {
        number_token = token;
    }
}

public class UnaryASTNode : ASTNode {
    public Token op;
    public ASTNode operand;

    public UnaryASTNode(Token op, ASTNode operand) : base(ASTNodeType.Unary) {
        this.op = op;
        this.operand = operand;
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
}
