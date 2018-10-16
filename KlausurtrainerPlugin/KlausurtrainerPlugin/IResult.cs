using KlausurtrainerPlugin.Enum;

namespace KlausurtrainerPlugin
{
    public interface IResult
    {
        /// <summary>
        /// Get the expected solution for this result.
        /// </summary>
        /// <returns>Returns a string which delivers a correct answer for this result.</returns>
        string GetSolution();

        /// <summary>
        /// Validate an input string.
        /// </summary>
        /// <param name="input">The input which is validated</param>
        /// <returns>Returns the status of the given input.</returns>
        AnswerCellStatus Validate(string input);
    }
}