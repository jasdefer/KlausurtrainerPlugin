using KlausurtrainerPlugin.Enum;

namespace KlausurtrainerPlugin.DefaultResults
{
    public class StringOrNullResult : IResult
    {
        public string Expected { get; }
        public StringCompareModes CompareMode { get; set; }

        /// <summary>
        /// Create a new result expecting a string or null input
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="compareMode">Defines how the expected value and input are compared (case sensitivity)</param>
        public StringOrNullResult(string expected, StringCompareModes compareMode)
        {
            Expected = expected;
            CompareMode = compareMode;
        }

        public string GetSolution()
        {
            return Expected;
        }

        public AnswerCellStatus Validate(string input)
        {
            if (Expected == null)
            {
                if (input == null)
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
                //The input is not null and can be treated as a normal StringResult
                StringResult stringResult = new StringResult(Expected, CompareMode);
                return stringResult.Validate(input);
            }
        }
    }
}