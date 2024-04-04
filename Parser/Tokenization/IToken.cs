using Parser.Expression;

namespace Parser.Tokenization
{
    public interface IToken { }

    public interface IOperator : IToken
    {
        public double Calculate(IExpr left, IExpr right);
    }

    public interface IAdopt : IOperator { }

    public interface IMulop : IOperator { }
}
