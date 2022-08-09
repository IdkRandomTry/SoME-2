using System;
using System.Collections.Generic;
using UnityEngine;

public enum ASTNodeType {
    Error,
    Call,
    IntrinsicCall,
    Number,
    Unary,
    Binary,
    Ident,
    X,
    E,
    PI,
}

public abstract class ASTNode {
    public ASTNodeType type;

    protected ASTNode(ASTNodeType type) {
        this.type = type;
    }
}

public class IdentASTNode : ASTNode {
    public Token ident;

    public IdentASTNode(Token ident) : base(ASTNodeType.Ident) {
        this.ident = ident;
    }
    
    public override string ToString() {
        return "Ident AST Node " + ident.Lexeme;
    }
}

public class CallASTNode : ASTNode {
    public Token func_name;
    public ASTNode[] arguments;

    public CallASTNode(Token func_name, ASTNode[] arguments) : base(ASTNodeType.Call) {
        this.func_name = func_name;
        this.arguments = arguments;
    }

    public override string ToString() {
        return "Call AST Node " + func_name.Lexeme;
    }
}

public class IntrinsicCallASTNode : ASTNode {
    public Token func_name;
    public ASTNode[] arguments;

    public IntrinsicCallASTNode(Token func_name, ASTNode[] arguments) : base(ASTNodeType.IntrinsicCall) {
        this.func_name = func_name;
        this.arguments = arguments;
    }

    public override string ToString() {
        return "IntrinsicCall AST Node " + func_name.Lexeme;
    }
}

public class PIASTNode : ASTNode {
    public Token token;

    public PIASTNode(Token token) : base(ASTNodeType.PI) {
        this.token = token;
    }
    
    public override string ToString() {
        return "PI AST Node pi";
    }
}

public class XASTNode : ASTNode {
    public Token token;

    public XASTNode(Token token) : base(ASTNodeType.X) {
        this.token = token;
    }
    
    public override string ToString() {
        return "X AST Node x";
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
        return "Unary AST Node " + op.Lexeme;
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
        return "Binary AST Node " + op.Lexeme;
    }
}

public struct Function {
    public Func<float[], float> function;
    public int arity;

    public Function(Func<float[], float> function, int arity) {
        this.function = function;
        this.arity = arity;
    }
}

public struct CustomFunction {
    public ASTNode function;
    public int arity;
    public string[] var_names;

    public CustomFunction(ASTNode function, int arity, string[] var_names) {
        this.function = function;
        this.arity = arity;
        this.var_names = var_names;
    }
}

public static class FunctionTable {
    public static Dictionary<string, Function> intrinsic_functions;
    public static Dictionary<string, CustomFunction> user_defined_functions;

    static FunctionTable() {
        intrinsic_functions = new Dictionary<string, Function>();
        user_defined_functions = new Dictionary<string, CustomFunction>();
        // Lambdas for float[] to float conversion. Because some intrinsics may want more params
        // Just basically functions that can be declared inline as parameters directly
        intrinsic_functions.Add("sin", new Function((float[] input) => Mathf.Sin(input[0]), 1));
        intrinsic_functions.Add("cos", new Function((float[] input) => Mathf.Cos(input[0]), 1));
        intrinsic_functions.Add("tan", new Function((float[] input) => Mathf.Tan(input[0]), 1));
        intrinsic_functions.Add("mod", new Function((float[] input) => Mathf.Abs(input[0]), 1));
        intrinsic_functions.Add("abs", new Function((float[] input) => Mathf.Abs(input[0]), 1));
        intrinsic_functions.Add("log", new Function((float[] input) => {
            // Slightly hacky way to get the line to extend downwards
            if (input[0] <= 0) return -10;
            return Mathf.Log(input[0]);
        }, 1));
        intrinsic_functions.Add("min", new Function((float[] input) => Mathf.Min(input[0], input[1]), 2));
        intrinsic_functions.Add("max", new Function((float[] input) => Mathf.Max(input[0], input[1]), 2));
    }
}
