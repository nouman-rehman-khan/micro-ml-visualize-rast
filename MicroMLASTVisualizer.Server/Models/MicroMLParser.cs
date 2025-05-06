using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MicroMLASTVisualizer.Models
{
    // AST Node types
    public abstract class ASTNode
    {
        public abstract string NodeType { get; }
    }

    public class NumberNode : ASTNode
    {
        public double Value { get; set; }
        public override string NodeType => "Number";

        public NumberNode(double value)
        {
            Value = value;
        }
    }

    public class VariableNode : ASTNode
    {
        public string Name { get; set; }
        public override string NodeType => "Variable";

        public VariableNode(string name)
        {
            Name = name;
        }
    }

    public class BinaryOpNode : ASTNode
    {
        public string Operator { get; set; }
        public ASTNode Left { get; set; }
        public ASTNode Right { get; set; }
        public override string NodeType => "BinaryOp";

        public BinaryOpNode(string op, ASTNode left, ASTNode right)
        {
            Operator = op;
            Left = left;
            Right = right;
        }
    }

    public class FunctionNode : ASTNode
    {
        public string ParameterName { get; set; }
        public ASTNode Body { get; set; }
        public override string NodeType => "Function";

        public FunctionNode(string paramName, ASTNode body)
        {
            ParameterName = paramName;
            Body = body;
        }
    }

    public class ApplicationNode : ASTNode
    {
        public ASTNode Function { get; set; }
        public ASTNode Argument { get; set; }
        public override string NodeType => "Application";

        public ApplicationNode(ASTNode func, ASTNode arg)
        {
            Function = func;
            Argument = arg;
        }
    }

    public class LetNode : ASTNode
    {
        public string VariableName { get; set; }
        public ASTNode Value { get; set; }
        public ASTNode InExpression { get; set; }
        public override string NodeType => "Let";

        public LetNode(string varName, ASTNode value, ASTNode inExpr)
        {
            VariableName = varName;
            Value = value;
            InExpression = inExpr;
        }
    }

    public class IfNode : ASTNode
    {
        public ASTNode Condition { get; set; }
        public ASTNode ThenBranch { get; set; }
        public ASTNode ElseBranch { get; set; }
        public override string NodeType => "If";

        public IfNode(ASTNode condition, ASTNode thenBranch, ASTNode elseBranch)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }
    }

    // Token class for lexical analysis
    public class Token
    {
        public string Type { get; set; }
        public string Value { get; set; }

        public Token(string type, string value)
        {
            Type = type;
            Value = value;
        }
    }

    
}