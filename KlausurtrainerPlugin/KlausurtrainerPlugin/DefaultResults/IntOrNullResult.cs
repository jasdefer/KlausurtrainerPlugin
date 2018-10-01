using KlausurtrainerPlugin.Enum;

namespace KlausurtrainerPlugin.DefaultResults
{
    public class IntOrNullResult : IResult
    {
        /// <summary>
        /// The expected result
        /// </summary>
        public int? Expected { get; }

        /// <summary>
        /// Create a new result where the input is either an integer or null
        /// </summary>
        public IntOrNullResult(int? expected)
        {
            Expected = expected;
        }

        public string GetSolution()
        {
            return Expected != null ? Expected.ToString() : null;
        }

        /// <summary>
        /// Checks if the given input is equal to the expected value.
        /// </summary>
        public AnswerCellStatus Validate(string input)
        {
            //Check if the correct solution is null
            if (Expected == null)
            {
                //The answer is correct if the input is null
                if (string.IsNullOrEmpty(input))
                {
                    return AnswerCellStatus.Correct;
                }
                else
                {
                    //The input is wrong, because it is not null. Still display a syntax error to prevent leaking any hints.
                    if (!int.TryParse(input, out int answer))
                    {
                        //the input is not an integer
                        return AnswerCellStatus.SyntaxError;
                    }
                    else
                    {
                        //The input is an integer, but it is wronge, because it is not null
                        return AnswerCellStatus.Incorrect;
                    }
                }
            }
            else
            {
                //Treat as a normal IntResult, because the requested value is not null
                IntResult intResult = new IntResult((int)Expected);
                return intResult.Validate(input);
            }
        }
    }
}