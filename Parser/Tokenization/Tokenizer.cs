using Parser.Expression;

namespace Parser.Tokenization
{
    public class Tokenizer
    {
        public static List<IToken> GetTokens(string expr)
        {
            return MapCharsToTokens(expr.ToCharArray());
        }

        private static List<IToken> MapCharsToTokens(char[] chars)
        {
            return chars switch
            {
                ['+', .. var rest] => Combine([new Add()], rest),
                ['-', .. var rest] => Combine([new Minus()], rest),
                ['*', .. var rest] => Combine([new Multiply()], rest),
                ['%', .. var rest] => Combine([new Modulus()], rest),
                ['/', .. var rest] => Combine([new Divide()], rest),
                ['(', .. var rest] => Combine([new OpenParenthesis()], rest),
                [')', .. var rest] => Combine([new CloseParenthesis()], rest),
                ['π', .. var rest] => Combine([new Pi()], rest),
                ['!', .. var rest] => Combine([new Factorial()], rest),
                ['^', .. var rest] => Combine([new Pow()], rest),
                ['s', 'i', 'n', .. var rest] => Combine([new Sin()], rest),
                ['c', 'o', 's', .. var rest] => Combine([new Cos()], rest),
                [' ', .. var rest] => MapCharsToTokens(rest),
                [var c, ..] when char.IsDigit(c) => DealWithDigits(chars),
                [_, .. var rest] => Combine([new UnknownToken()], rest),
                _ => []
            };
        }

        private static List<IToken> DealWithDigits(char[] tokens)
        {
            var number = int.Parse(string.Join("", tokens.TakeWhile(char.IsDigit)));
            return Combine([new Number(number)], tokens.SkipWhile(char.IsDigit).ToArray());
        }

        private static List<IToken> Combine(IEnumerable<IToken> left, char[] rest) => left.Concat(MapCharsToTokens(rest)).ToList();
    }
}
