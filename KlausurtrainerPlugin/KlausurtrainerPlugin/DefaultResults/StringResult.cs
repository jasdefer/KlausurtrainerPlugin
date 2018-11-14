using KlausurtrainerPlugin.Enum;

namespace KlausurtrainerPlugin.DefaultResults
{
    public class StringResult : IResult
    {
        public string Expected { get; }
        public StringCompareModes CompareMode { get; set; }

        /// <summary>
        /// Create a new result expecting a string input
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="compareModus">Defines how the expected value and input are compared (case sensitivity)</param>
        public StringResult(string expected, StringCompareModes compareModus)
        {
            Expected = expected;
            CompareMode = compareModus;
        }

        public string GetSolution()
        {
            return Expected;
        }

        public AnswerCellStatus Validate(string input)
        {
            //Check for missing input
            if (string.IsNullOrEmpty(input))
            {
                return AnswerCellStatus.Missing;
            }

            //If the comparison should be case invariant set both string to a lower case word
            string expected = Expected;
            if (CompareMode == StringCompareModes.CaseInvariant)
            {
                expected = expected.ToLowerInvariant();
                input = input.ToLowerInvariant();
            }

            //Compare the input with the expected value
            if (expected == input)
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