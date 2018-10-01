using KlausurtrainerPlugin.Enum;
using System;

namespace KlausurtrainerPlugin.DefaultResults
{
    public class FractionResult : IResult
    {
        public decimal Expected { get; }
        public decimal Epsilon { get; }
        public int? Numerator { get; }
        public int? Denominator { get; }

        /// <summary>
        /// Create a new decimal result which is expressed as a fraction
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="epsilon">The input is correct as long as the difference between the input and expected value is not greater than this epsilon</param>
        /// <param name="numerator">For displaying the correct solution. Has no effect on the validation</param>
        /// <param name="denominator">For displaying the correct solution. Has no effect on the validation</param>
        public FractionResult(decimal expected, decimal epsilon = 0.01m, int? numerator = null, int? denominator = null)
        {
            Expected = expected;
            Numerator = numerator;
            Denominator = denominator;
            Epsilon = epsilon;
        }



        public string GetSolution()
        {
            //Display the declared numerator and denominator if defined
            if (Numerator != null && Denominator != null)
            {
                return $"{Numerator}/{Denominator}";
            }
            else
            {
                //It is difficult to find the smallest possible numerator and dominator for any given number
                return $"{Expected}/1";
            }
        }

        public AnswerCellStatus Validate(string input)
        {
            //check for missing values
            if (string.IsNullOrEmpty(input))
            {
                return AnswerCellStatus.Missing;
            }

            //Check if the input is a wks NUMERATOR/DENOMINATOR
            string[] values = input.Split('/');
            if (values.Length != 2)
            {
                return AnswerCellStatus.SyntaxError;
            }

            //Parse the numerator
            bool success = int.TryParse(values[0], out int numerator);
            if (!success)
            {
                return AnswerCellStatus.SyntaxError;
            }


            //Parse the denominator
            success = int.TryParse(values[1], out int denominator);
            if (!success)
            {
                return AnswerCellStatus.SyntaxError;
            }

            //Calculate the answer
            decimal answer = numerator / (decimal)denominator;

            //Compare the answer with the expected value. It is correct if the answer is close enough to the expected value
            if (Math.Abs(answer - Expected) <= Epsilon)
            {
                return AnswerCellStatus.Correct;
            }
            else
            {
                return AnswerCellStatus.Incorrect;
            }
        }
    }
}