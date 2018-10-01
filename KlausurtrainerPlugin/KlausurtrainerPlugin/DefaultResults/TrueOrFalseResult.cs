using KlausurtrainerPlugin.Enum;

namespace KlausurtrainerPlugin.DefaultResults
{
    public class TrueOrFalseResult : IResult
    {
        public bool Expected { get; }

        /// <summary>
        /// Create a result which expects either true or false
        /// </summary>
        /// <param name="expected">The expected result</param>
        public TrueOrFalseResult(bool expected)
        {
            Expected = expected;
        }

        public string GetSolution()
        {
            return Expected.ToString();
        }

        public AnswerCellStatus Validate(string input)
        {
            //Check for missing input
            if (string.IsNullOrEmpty(input))
            {
                return AnswerCellStatus.Missing;
            }

            //Convert the input to a bool
            bool success = bool.TryParse(input, out bool answer);
            //Check for syntax errors
            if (!success)
            {
                return AnswerCellStatus.SyntaxError;
            }

            //Compare the answer with the expected value
            if (answer == Expected)
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