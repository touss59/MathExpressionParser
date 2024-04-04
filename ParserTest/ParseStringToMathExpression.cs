using LaYumba.Functional;
using Parser;
using Parser.Tokenization;

namespace ParserTest
{
    internal class ParseStringToMathExpression
    {
        [TestCase("12", 12)]
        [TestCase("3!!", 720)]
        [TestCase(" -12 * 3 + 1", -35)]
        [TestCase("14%6", 2)]
        [TestCase("π*2", 6.2831853071795862)]
        [TestCase("1 + + 1", 2)]
        [TestCase("1 - - 2", 3)]
        [TestCase("1 - - -1", 0)]
        [TestCase("1 * + - 1", -1)]
        [TestCase("12+   6", 18)]
        [TestCase("2^2", 4)]
        [TestCase("2^2!", 4)]
        [TestCase("8! * 12", 483840)]
        [TestCase("4!*3", 72)]
        [TestCase("2 * 4! * 3", 144)]
        [TestCase("12+6-3", 15)]
        [TestCase("12 * (6 - 3)", 36)]
        [TestCase("-sin((45-35)!)", -0.26392232267495169)]
        [TestCase("12  * ( 2 + 3) * 2+3", 123)]
        [TestCase(("(1+1) + + (1+1)"), 4)]
        [TestCase(("(((1+1))*2) + + (1+1)"), 6)]
        [TestCase(("10000 * 1000"), 10000000)]
        [TestCase("2 * 3 + 4 - 5 / 2", 7.5)]
        [TestCase("sin(2 * 3 + 4 - 5 / 2)", 0.9379999767747389)]
        [TestCase("(((2 * 3) - 4) + ((5 * (6 - 7)) / (8 - 2))) * ((9 - 1)+((10 + 2) / (11 - 5)))", 11.666666666666664)]
        public void ParseValidStringExpr_Should_Work(string expr, double expectedResult)
        {
            var exp = ParserLogic.ParseExpr(Tokenizer.GetTokens(expr));
            exp.Match(
                () => Assert.Fail("Should return a valid result"),
                (res) =>Assert.That(res.Eval(), Is.EqualTo(expectedResult)));
        }

        [TestCase("12 * * 23")]
        [TestCase("12 / * 23")]
        [TestCase("(1+1) * * (1+1)")]
        [TestCase("^!")]
        [TestCase("^1!")]
        [TestCase("12^")]
        [TestCase("sin(0)^")]
        [TestCase("-+1^")]
        [TestCase("1 == 1")]
        [TestCase("hello world")]
        [TestCase("3!2!")]
        public void ParseInvalidString_Should_Fail(string expr)
        {
            var exp = ParserLogic.ParseExpr(Tokenizer.GetTokens(expr));
            exp.Match(
                () => Assert.Pass("Should return None, this expression isn't valid"),
                (res) => Assert.Fail("Should return None, this expression isn't valid")
            );
        }
    }
}
