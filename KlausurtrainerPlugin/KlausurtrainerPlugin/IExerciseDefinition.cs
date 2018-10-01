using KlausurtrainerPlugin.Enum;
using System.Collections.Generic;

namespace KlausurtrainerPlugin
{
    public interface IExerciseDefinition
    {
        /// <summary>
        /// The name of the exercise. It must be unique in a course.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The name of the view file for this exercise. The view is a *.cshtml or *.html file
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Instantiate the exercise for a given seed. Depending on the seed the exercise values should change.
        /// </summary>
        void Instantiate(string seed);

        /// <summary>
        /// A brief description of the exercise available for the course instructor.
        /// </summary>
        string[] GetExerciseDescription();

        /// <summary>
        /// The order of the exercise within the course
        /// </summary>
        int Order { get; }

        /// <summary>
        /// The chapter of this exercise in the course
        /// </summary>
        string Chapter { get; }

        /// <summary>
        /// Get the answer cells to inform the user where what kind of answer is expected.
        /// </summary>
        Dictionary<Cell, AnswerCell> AnswerCells { get; }

        /// <summary>
        /// Get the exercise values which change with the seed. The <paramref name="Instantiate"/> method will always be called before this method. 
        /// </summary>
        object GetValues();

        /// <summary>
        /// Get the expected solutions for this exercise which change with the seed. The <paramref name="Instantiate"/> method will always be called before this method. 
        /// </summary>
        /// <returns></returns>
        Dictionary<Cell, string> GetExpectedSolutions();

        /// <summary>
        /// Get the state of an answer for each answer cell depending on the current seed. The <paramref name="Instantiate"/> method will always be called before this method. 
        /// </summary>
        /// <param name="inputs">The answer which is validated.</param>
        Dictionary<Cell, AnswerCellStatus> GetAnswerDetails(Dictionary<Cell, string> inputs);
    }
}