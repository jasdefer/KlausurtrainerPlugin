using KlausurtrainerPlugin.Enum;
using System.Globalization;

namespace KlausurtrainerPlugin.DefaultResults
{
    public class DecimalOrNullResult : IResult
    {
        public decimal? Expected { get; set; }
        public decimal Epsilon { get; set; }

        public string GetSolution()
        {
            return Expected?.ToString();
        }

        /// <summary>
        /// Create a new result which expects a decimal value or null
        /// </summary>
        /// <param name="expected">The expected value or null</param>
        /// <param name="epsilon">The input is correct as long as the difference between the input and expected value is not greater than this epsilon</param>
        public DecimalOrNullResult(decimal? expected, decimal epsilon)
        {
            Expected = expected;
            Epsilon = epsilon;
        }

        public AnswerCellStatus Validate(string input)
        {
            if (Expected == null)
            {
                //The answer is correct if the input is null
                if (string.IsNullOrEmpty(input))
                {
                    return AnswerCellStatus.Correct;
                }
                else
                {
                    input = input.Replace(",", ".");
                    //The input is wrong, because it is not null. Still display a syntax error to prevent leaking any hints.
                    if (!decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal answer))
                    {
                        //the input is not a decimal
                        return AnswerCellStatus.SyntaxError;
                    }
                    else
                    {
                        //The input is a decimal, but it is wronge, because it is not null
                        return AnswerCellStatus.Incorrect;
                    }
                }
            }
            else
            {
                //The expected value is not null, so it can be treated like a normal DecimalResult
                DecimalResult decimalResult = new DecimalResult((decimal)Expected, Epsilon);
                return decimalResult.Validate(input);
            }
        }
    }
}