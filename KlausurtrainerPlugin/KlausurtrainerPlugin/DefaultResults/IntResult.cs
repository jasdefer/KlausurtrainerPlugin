using KlausurtrainerPlugin.Enum;

namespace KlausurtrainerPlugin.DefaultResults
{
    public class IntResult : IResult
    {
        /// <summary>
        /// The expected value
        /// </summary>
        public int Expected { get; }

        /// <summary>
        /// Create a new integer result
        /// </summary>
        /// <param name="expectedValue">The expected value for this result</param>
        public IntResult(int expectedValue)
        {
            Expected = expectedValue;
        }

        /// <summary>
        /// Get a string of the expected result
        /// </summary>
        public string GetSolution()
        {
            return Expected.ToString();
        }

        /// <summary>
        /// Validate the users input and return the status of the input
        /// </summary>
        public AnswerCellStatus Validate(string input)
        {
            //Check for missing values
            if (string.IsNullOrWhiteSpace(input))
            {
                return AnswerCellStatus.Missing;
            }

            //Try to convert the input to an integer
            if (int.TryParse(input, out int answer))
            {
                //Since the answer is an integer, compare it with the excpected result
                if (answer == Expected)
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