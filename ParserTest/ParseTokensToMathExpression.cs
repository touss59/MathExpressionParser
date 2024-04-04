using Parser.Tokenization;
using LaYumba.Functional;
using Parser;
using Parser.Expression;

namespace ParserTest
{
    internal class ParseTokensToMathExpression
    {
        [TestCaseSource(nameof(MathExprWithExpectedResult))]
        public void ParseValidTokens_Should_Work((List<IToken>, double) mathExprWithExpectedResult)
        {
            var exp = ParserLogic.ParseExpr(mathExprWithExpectedResult.Item1);

            exp.Match(
                () => Assert.Fail("Should return a valid result"),
                (res) => Assert.That(res.Eval(), Is.EqualTo(mathExprWithExpectedResult.Item2))
            );
        }

        [TestCaseSource(nameof(UnValidMathExpr))]
        public void ParseInvalidTokens_Should_Fail(List<IToken> tokens)
        {
            var exp = ParserLogic.ParseExpr(tokens);

            exp.Match(
                () => Assert.Pass("Should return None, tokens input isn't valid"),
                _ => Assert.Fail("Should not return a valid result")
            );
        }

        private static IEnumerable<(List<IToken>, double)> MathExprWithExpectedResult()
        {
            // 1 = 1
            yield return ([new Number(1)], 1);

            // 23 ++ 6
            yield return ([new Number(23), new Add(), new Add(), new Number(6)], 29);

            // 1 - - 1 = 2
            yield return ([new Number(1), new Minus(), new Minus(), new Number(1)], 2);

            // - 1 = -1
            yield return ([new Minus(), new Number(1)], -1);

            // 12 + 2 = 14
            yield return ([new Number(12), new Add(), new Number(2)], 14);

            // 12 + 2 * 3 = 18
            yield return ([new Number(12), new Add(), new Number(2), new Multiply(), new Number(3)], 18);

            // 12 * 2 + 3 = 27
            yield return ([new Number(12), new Multiply(), new Number(2), new Add(), new Number(3)], 27);

            // 12 * (2 + 3) = 60
            yield return (
            [
                new Number(12),
                new Multiply(),
                new OpenParenthesis(),
                new Number(2),
                new Add(),
                new Number(3),
                new CloseParenthesis()
            ], 60);
            //
            // // 12 * (2 + 3) * 2 = 120
            yield return (
            [
                new Number(12),
                new Multiply(),
                new OpenParenthesis(),
                new Number(2),
                new Add(),
                new Number(3),
                new CloseParenthesis(),
                new Multiply(),
                new Number(2)
            ], 120);
            //
            // // 12 * (2 + 3) * 2 + 3 = 123
            yield return (
            [
                new Number(12),
                new Multiply(),
                new OpenParenthesis(),
                new Number(2),
                new Add(),
                new Number(3),
                new CloseParenthesis(),
                new Multiply(),
                new Number(2),
                new Add(),
                new Number(3)
            ], 123);
            //
            // // 12 * (2 + 3) * 2 + 3 * 2 = 126
            yield return (
            [
                new Number(12),
                new Multiply(),
                new OpenParenthesis(),
                new Number(2),
                new Add(),
                new Number(3),
                new CloseParenthesis(),
                new Multiply(),
                new Number(2),
                new Add(),
                new Number(3),
                new Multiply(),
                new Number(2)
            ], 126);
            //
            // // 12 * (2 + 3) * 2 + 3 * 2 + 3 = 129
            yield return (
            [
                new Number(12),
                new Multiply(),
                new OpenParenthesis(),
                new Number(2),
                new Add(),
                new Number(3),
                new CloseParenthesis(),
                new Multiply(),
                new Number(2),
                new Add(),
                new Number(3),
                new Multiply(),
                new Number(2),
                new Add(),
                new Number(3)
            ], 129);
            //
            // // 6 * 6 * 6 * 12 + 34 + (6 * 2 + 1) * 4 = 2678
            yield return (
            [
                new Number(6),
                new Multiply(),
                new Number(6),
                new Multiply(),
                new Number(6),
                new Multiply(),
                new Number(12),
                new Add(),
                new Number(34),
                new Add(),
                new OpenParenthesis(),
                new Number(6),
                new Multiply(),
                new Number(2),
                new Add(),
                new Number(1),
                new CloseParenthesis(),
                new Multiply(),
                new Number(4)
            ], 2678);
            //
            // // 7 * 12 + 3 * 6 + 9 + (6 * 2 + 1) * 4 = 163
            yield return (
            [
                new Number(7),
                new Multiply(),
                new Number(12),
                new Add(),
                new Number(3),
                new Multiply(),
                new Number(6),
                new Add(),
                new Number(9),
                new Add(),
                new OpenParenthesis(),
                new Number(6),
                new Multiply(),
                new Number(2),
                new Add(),
                new Number(1),
                new CloseParenthesis(),
                new Multiply(),
                new Number(4)
            ], 163);
        }

        private static IEnumerable<List<IToken>> UnValidMathExpr()
        {
            // 12 ** 23 = 18
            yield return [new Number(12), new Multiply(), new Multiply(), new Number(2), new Number(3)];
            yield return [new UnknownToken()];

        }
    }
}
