using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KlausurtrainerPlugin.ExerciseValidation
{
    public class TimeValidation
    {
        public const int ITERATIONS = 10;
        public static readonly TimeSpan MAX_INSTANTIATE = TimeSpan.FromMilliseconds(50);
        public static readonly TimeSpan MAX_SOLUTION = TimeSpan.FromMilliseconds(200);

        /// <summary>
        /// Validate a set of exercises and test, if the calculation of each one is quick enough.
        /// Each exercise is tested with <see cref="ITERATIONS"/> different seeds.
        /// </summary>
        /// <param name="exercises">The set of exercises which are tested.</param>
        /// <returns>Returns a list of errors or an empty list, if all exercises are valid.</returns>
        public static async Task<List<string>> GetErrors(IEnumerable<IExerciseDefinition> exercises)
        {
            var errors = new List<string>();

            foreach (var exercise in exercises)
            {
                try
                {
                    for (int i = 0; i < ITERATIONS; i++)
                    {
                        var error = await GetError(exercise, i.ToString());
                        if (!string.IsNullOrEmpty(error))
                        {
                            errors.Add(error);
                            //Cancel the test, to prevent long waiting times
                            i = ITERATIONS;
                        }
                    }
                }
                catch (Exception e)
                {
                    errors.Add($"{exercise.Name} throws an excepetion: {e.Message}.");
                }
            }

            return errors;
        }

        /// <summary>
        /// Validate if the exercise calculates quick enough.
        /// </summary>
        /// <param name="exercise">The exercises which is validated.</param>
        /// <param name="seed">The seed with which the exercise is validated.</param>
        /// <returns>Returns an error or an empty string if the exercise is valid for the given seed.</returns>
        public static async Task<string> GetError(IExerciseDefinition exercise, string seed)
        {
            //Validate the instantiation
            var task = Task.Run(() => exercise.Instantiate(seed));
            if(await Task.WhenAny(task, Task.Delay(MAX_INSTANTIATE)) != task)
            {
                return $"{exercise.Name} took too long to instantiate for the seed {seed}.";
            }

            task = Task.Run(() => exercise.GetExpectedSolutions());
            if (await Task.WhenAny(task, Task.Delay(MAX_SOLUTION)) != task)
            {
                return $"{exercise.Name} took too long to calculate the solution.";
            }

            //No errors detected
            return string.Empty;
        }
    }
}