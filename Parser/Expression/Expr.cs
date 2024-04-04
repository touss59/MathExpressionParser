using Parser.Tokenization;

namespace Parser.Expression
{
    public readonly struct TermAddopExpr(IExpr term, IAdopt addopt, IExpr expr) : IExpr
    {
        public double Eval()
        {
            return addopt.Calculate(term, expr);
        }
    }

    public readonly struct PowMulopTerm(IExpr pow, IMulop mulop, IExpr term) : IExpr
    {
        public double Eval()
        {
            return mulop.Calculate(pow, term);
        }
    }

    public readonly struct FactorPow(IExpr factor, IExpr pow) : IExpr
    {
        public double Eval()
        {
            return Math.Pow(factor.Eval(), pow.Eval());
        }
    }

    public readonly struct FactorialWrapper(IExpr expr) : IExpr
    {
        public double Eval()
        {
            return Enumerable.Range(1, (int)expr.Eval()).Aggregate(1, (acc, x) => acc * x);
        }
    }

    public readonly struct MinusWrapper(IExpr expr) : IExpr
    {
        public double Eval()
        {
            return -expr.Eval();
        }
    }

    public readonly struct SinWrapper(IExpr expr) : IExpr
    {
        public double Eval()
        {
            return Math.Sin(expr.Eval());
        }
    }

    public readonly struct CosWrapper(IExpr expr) : IExpr
    {
        public double Eval()
        {
            return Math.Cos(expr.Eval());
        }
    }

    public readonly struct Number(int value) : IExpr, IToken
    {
        public double Eval()
        {
            return value;
        }
    }

    public struct Pi : IExpr, IToken
    {
        public double Eval()
        {
            return Math.PI;
        }
    }
}
