using Parser.Expression;

namespace Parser.Tokenization
{
    public struct Add : IAdopt
    {
        public double Calculate(IExpr left, IExpr right)
        {
            return left.Eval() + right.Eval();
        }
    }

    public struct Minus : IAdopt
    {
        public double Calculate(IExpr left, IExpr right)
        {
            return left.Eval() - right.Eval();
        }
    }

    public struct Multiply : IMulop
    {
        public double Calculate(IExpr left, IExpr right)
        {
            return left.Eval() * right.Eval();
        }
    }

    public struct Divide : IMulop
    {
        public double Calculate(IExpr left, IExpr right)
        {
            return left.Eval() / right.Eval();
        }
    }

    public struct Modulus : IMulop
    {
        public double Calculate(IExpr left, IExpr right)
        {
            return left.Eval() % right.Eval();
        }
    }

    public struct Pow : IOperator
    {
        public double Calculate(IExpr left, IExpr right)
        {
            return Math.Pow(left.Eval(), right.Eval());
        }
    }

    public struct OpenParenthesis : IToken { }
    public struct CloseParenthesis : IToken { }
    public struct Sin : IToken { }
    public struct Cos : IToken { }
    public struct Factorial : IToken { }
    public struct UnknownToken : IToken { }
}
