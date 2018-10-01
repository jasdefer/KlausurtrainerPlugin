using KlausurtrainerPlugin.Enum;
using System;
using System.Globalization;

namespace KlausurtrainerPlugin.DefaultResults
{
    public class DecimalResult : IResult
    {
        public decimal Expected { get; }
        public decimal Epsilon { get; }

        /// <summary>
        /// Create a new decimal result
        /// </summary>
        /// <param name="expected">The expected value of the result</param>
        /// <param name="epsilon">The input is correct as long as the difference between the input and expected value is not greater than this epsilon</param>
        public DecimalResult(decimal expected, decimal epsilon)
        {
            Expected = expected;
            Epsilon = epsilon;
        }

        public string GetSolution()
        {
            return Expected.ToString();
        }

        public AnswerCellStatus Validate(string input)
        {
            //Check for missing values
            if (string.IsNullOrWhiteSpace(input))
            {
                return AnswerCellStatus.Missing;
            }
            input = input.Replace(",", ".");
            //Try to convert the input to a decimal
            if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal answer))
            {
                //Since the answer is an decimal, compare it with the excpected result.
                decimal delta = Math.Abs(Expected - answer);

                //The input is correct if the answer is close enough
                if (delta <= Epsilon)
                {
                    return AnswerCellStatus.Correct;
                }
                else
                {
                    return AnswerCellStatus.Incorrect;
                }
            }
            else
            {
                //The input is not an integer
                return AnswerCellStatus.SyntaxError;
            }
        }
    }
}