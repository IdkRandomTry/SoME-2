public abstract class ASTNode {
    // Nothing here, just markup for subtyping because Unions dont exist in C# :(
}

public class VariableASTNode : ASTNode {
    private Token var_token;

    public VariableASTNode(Token token) {
        this.var_token = token;
    }
}

public class NumberASTNode : ASTNode {
    private Token number_token;

    public NumberASTNode(Token token) {
        number_token = token;
    }
}