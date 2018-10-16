using KlausurtrainerPlugin.Enum;
using System;
using System.Collections.Generic;

namespace KlausurtrainerPlugin.ExerciseValidation
{
    public static class ImplementationValidation
    {
        private const int ITERATIONS = 100;

        /// <summary>
        /// Validate a set of exercises for their correct implementation of the <see cref="IExerciseDefinition"/> interface.
        /// Each exercise is tested with <see cref="ITERATIONS"/> different seeds.
        /// </summary>
        /// <param name="exercises">The set of exercises which are tested.</param>
        /// <returns>Returns a list of errors or an empty list, if all exercises are valid.</returns>
        public static List<string> GetErrors(IEnumerable<IExerciseDefinition> exercises)
        {
            var errors = new List<string>();

            foreach (var exercise in exercises)
            {
                try
                {
                    for (int i = 0; i < ITERATIONS; i++)
                    {
                        var error = GetError(exercise, i.ToString());
                        if (!string.IsNullOrEmpty(error)) errors.Add(error);
                    }
                }
                catch(Exception e)
                {
                    errors.Add($"{exercise.Name} throws an excepetion: {e.Message}.");
                }
            }

            return errors;
        }

        /// <summary>
        /// Validate if the exercise implemented the <see cref="IExerciseDefinition"/> interface correctly.
        /// Instantiation, answer cell, get expected solution and answer details are validated.
        /// </summary>
        /// <param name="exercise">The exercises which is validated.</param>
        /// <param name="seed">The seed with which the exercise is validated.</param>
        /// <returns>Returns an error or an empty string if the exercise is valid for the given seed.</returns>
        public static string GetError(IExerciseDefinition exercise, string seed)
        {
            //Validate the instantiation
            exercise.Instantiate(seed);

            //Validate the answer cells
            var answerCells = exercise.AnswerCells;
            if (answerCells == null || answerCells.Count < 1)
            {
                return $"{exercise.Name} does not generate any answer cells.";
            }

            //Validate the exercise values
            var values = exercise.GetValues();

            //Validate the expected solutions
            var solutions = exercise.GetExpectedSolutions();

            //Valdaite the answer cells
            var answerDetails = exercise.GetAnswerDetails(solutions);

            //Validate if the expected solutions are considered correct
            foreach (var answerDetail in answerDetails)
            {
                if(answerDetail.Value != Enum.AnswerCellStatus.Correct)
                {
                    return $"{exercise.Name}: The expected solutions is incorrect at {answerDetail.Key}.";
                }
            }

            //Check if there is a solution for each answer cell
            foreach (var answerCell in answerCells)
            {
                if (answerCell.Value.InputType!= InputTypes.Label &&
                    !solutions.ContainsKey(answerCell.Key))
                {
                    return $"{exercise.Name}: The answer cell {answerCell.Key} is not part of the solution.";
                }
            }

            //Check if there is an answer cell for each solution
            foreach (var solution in solutions)
            {
                if(!answerCells.ContainsKey(solution.Key))
                {
                    return $"{exercise.Name}: The solution {solution.Key} is not part of the answer cells.";
                }
            }

            //No errors detected
            return string.Empty;
        }
    }
}