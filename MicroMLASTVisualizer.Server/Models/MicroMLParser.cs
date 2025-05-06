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

    // Parser class
    public class Parser
    {
        private List<Token> _tokens;
        private int _position;

        public ASTNode Parse(string code)
        {
            // Tokenize the input
            _tokens = Tokenize(code);
            _position = 0;

            // Parse expression
            return ParseExpression();
        }

        private List<Token> Tokenize(string code)
        {
            List<Token> tokens = new List<Token>();
            string pattern = @"\(|\)|[a-zA-Z_][a-zA-Z0-9_]*|[0-9]+(?:\.[0-9]+)?|\+|\-|\*|\/|=|==|!=|<|>|<=|>=|let|in|if|then|else|lambda|fun|->|;|\s+";

            var matches = Regex.Matches(code, pattern);
            foreach (Match match in matches)
            {
                string value = match.Value;

                // Skip whitespace
                if (Regex.IsMatch(value, @"^\s+$"))
                    continue;

                string type = DetermineTokenType(value);
                tokens.Add(new Token(type, value));
            }

            return tokens;
        }

        private string DetermineTokenType(string value)
        {
            if (Regex.IsMatch(value, @"^[0-9]+(?:\.[0-9]+)?$"))
                return "NUMBER";

            if (value == "let")
                return "LET";

            if (value == "in")
                return "IN";

            if (value == "if")
                return "IF";

            if (value == "then")
                return "THEN";

            if (value == "else")
                return "ELSE";

            if (value == "lambda" || value == "fun")
                return "LAMBDA";

            if (value == "->")
                return "ARROW";

            if (value == "(")
                return "LPAREN";

            if (value == ")")
                return "RPAREN";

            if (value == "+" || value == "-" || value == "*" || value == "/" ||
                value == "==" || value == "!=" || value == "<" || value == ">" ||
                value == "<=" || value == ">=" || value == "=")
                return "OPERATOR";

            if (value == ";")
                return "SEMICOLON";

            return "IDENTIFIER";
        }

        private Token CurrentToken()
        {
            if (_position >= _tokens.Count)
                return null;

            return _tokens[_position];
        }

        private void Advance()
        {
            _position++;
        }

        private ASTNode ParseExpression()
        {
            return ParseLetExpression();
        }

        private ASTNode ParseLetExpression()
        {
            if (CurrentToken()?.Type == "LET")
            {
                Advance(); // Consume 'let'

                if (CurrentToken()?.Type != "IDENTIFIER")
                    throw new Exception("Expected identifier after 'let'");

                string varName = CurrentToken().Value;
                Advance(); // Consume identifier

                if (CurrentToken()?.Type != "OPERATOR" || CurrentToken()?.Value != "=")
                    throw new Exception("Expected '=' after variable name in let expression");

                Advance(); // Consume '='

                ASTNode value = ParseExpression();

                if (CurrentToken()?.Type != "IN")
                    throw new Exception("Expected 'in' after value in let expression");

                Advance(); // Consume 'in'

                ASTNode inExpr = ParseExpression();

                return new LetNode(varName, value, inExpr);
            }

            return ParseIfExpression();
        }

        private ASTNode ParseIfExpression()
        {
            if (CurrentToken()?.Type == "IF")
            {
                Advance(); // Consume 'if'

                ASTNode condition = ParseExpression();

                if (CurrentToken()?.Type != "THEN")
                    throw new Exception("Expected 'then' after condition in if expression");

                Advance(); // Consume 'then'

                ASTNode thenBranch = ParseExpression();

                if (CurrentToken()?.Type != "ELSE")
                    throw new Exception("Expected 'else' after then branch in if expression");

                Advance(); // Consume 'else'

                ASTNode elseBranch = ParseExpression();

                return new IfNode(condition, thenBranch, elseBranch);
            }

            return ParseComparisonExpression();
        }

        
    }
}