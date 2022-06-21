using System;
using System.Collections.Generic;
using UnityEngine;

public enum ASTNodeType {
    Error,
    IntrinsicCall,
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

public class CallIntrinsicASTNode : ASTNode {
    public Token intrinsic_name;
    public ASTNode[] arguments;

    public CallIntrinsicASTNode(Token intrinsic_name, ASTNode[] arguments) : base(ASTNodeType.IntrinsicCall) {
        this.intrinsic_name = intrinsic_name;
        this.arguments = arguments;
    }

    public override string ToString() {
        return "CallIntrinsic AST Node " + intrinsic_name.Lexeme;
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

public static class FunctionTable {
    public static Dictionary<string, Func<float[], float>> inbuilt_functions;

    static FunctionTable() {
        inbuilt_functions = new Dictionary<string, Func<float[], float>>();
        // Lambdas for float[] to float conversion. Because some intrinsics may want more params
        // Just basically functions that can be declared inline as parameters directly
        inbuilt_functions.Add("sin", (float[] input) => Mathf.Sin(input[0]));
        inbuilt_functions.Add("cos", (float[] input) => Mathf.Cos(input[0]));
        inbuilt_functions.Add("tan", (float[] input) => Mathf.Tan(input[0]));
        inbuilt_functions.Add("mod", (float[] input) => Mathf.Abs(input[0]));
        inbuilt_functions.Add("log", (float[] input) => Mathf.Log(input[0]));
    }
}
