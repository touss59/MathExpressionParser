using LaYumba.Functional;
using Parser.Expression;
using Parser.Tokenization;

namespace Parser;

public static class ParserLogic
{
    public static Option<IExpr> ParseExpr(IList<IToken> tokens)
    {
        return ParseTermAdoptExpr(tokens).OrElse(() => ParseTerm(tokens));
    }

    private static Option<IExpr> ParseTerm(IList<IToken> tokens)
    {
        return ParsePowMulopTerm(tokens).OrElse(() => ParsePow(tokens.ToArray()));
    }

    private static Option<IExpr> ParsePow(IList<IToken> tokens)
    {
        return ParseFactorPow(tokens).OrElse(() => ParseFactor(tokens.ToArray()));
    }

    private static Option<IExpr> ParseTermAdoptExpr(IList<IToken> tokens)
    {
        var parts = GetParts(tokens, t => t is IAdopt);

        return parts.Bind(tuple => ParseTerm(tuple.left).
            Bind(exp => ParseExpr(tuple.right).
                Map(term => (IExpr)new TermAddopExpr(exp, (IAdopt)tuple.middle, term))
            ));
    }

    private static Option<IExpr> ParsePowMulopTerm(IList<IToken> tokens)
    {
        var parts = GetParts(tokens, t => t is IMulop);

        return parts.Bind((tuple) => ParsePow(tuple.left).
            Bind(leftTerm => ParseTerm(tuple.right).
                 Map(rightTerm => (IExpr)new PowMulopTerm(leftTerm, (IMulop)tuple.middle, rightTerm))
             ));
    }

    private static Option<IExpr> ParseFactorPow(IList<IToken> tokens)
    {
        var parts = GetParts(tokens, t => t is Pow);

        return parts.Bind(tuple => ParseFactor(tuple.left).
            Bind(factor => ParsePow(tuple.right).
                Map(pow => (IExpr)new FactorPow(factor, pow))
            ));
    }

    private static Option<IExpr> ParseFactor(IToken[] tokens)
    {
        return tokens switch
        {
            [Add _, .. var factor] => ParseFactor(factor),
            [Minus _, .. var factor] => ParseFactor(factor).Map(expr => (IExpr)new MinusWrapper(expr)),
            [.. var leftTokens, Factorial _] => ParseFactor(leftTokens).Map(expr => (IExpr)new FactorialWrapper(expr)),

            [Sin _, OpenParenthesis _, .. var inner, CloseParenthesis _] => ParseExpr(inner).Map(expr => (IExpr)new SinWrapper(expr)),
            [Cos _, OpenParenthesis _, .. var inner, CloseParenthesis _] => ParseExpr(inner).Map(expr => (IExpr)new CosWrapper(expr)),
            [OpenParenthesis _, .. var inner, CloseParenthesis _] => ParseExpr(inner),

            [Number number] => number,
            [Pi pi] => pi,
            _ => F.None
        };
    }

    private static Option<(IToken[] left, IToken middle, IToken[] right)> GetParts(IList<IToken> tokens, Predicate<IToken> validate)
    {
        var parenthesisBalance = 0;
        var index = -1;
        var validToken = tokens.Find(token =>
        {
            parenthesisBalance = token switch
            {
                OpenParenthesis => ++parenthesisBalance,
                CloseParenthesis => --parenthesisBalance,
                _ => parenthesisBalance
            };

            index++;
            return validate(token) && parenthesisBalance == 0 && index > 0;
        });

        return validToken.Map(token => (
            tokens.Take(index).ToArray(),
            tokens[index],
            tokens.Skip(index + 1).ToArray()));
    }
}