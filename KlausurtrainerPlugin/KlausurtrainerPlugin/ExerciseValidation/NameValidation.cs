using System.Collections.Generic;

namespace KlausurtrainerPlugin.ExerciseValidation
{
    public static class NameValidation
    {
        /// <summary>
        /// The minimum number of characters required for the name of an exercise.
        /// </summary>
        public const int MIN_NAME_LENGTH = 3;

        /// <summary>
        /// The maximum number of characters allowed for the name of an exercise.
        /// </summary>
        public const int MAX_NAME_LENGTH = 128;

        /// <summary>
        /// Check if the names of a given set is valid.
        /// </summary>
        /// <param name="exercises">The set of exercises which should be validated.</param>
        /// <returns>Returns a list of errors. If the list is empty, the exercise names are valid.</returns>
        public static List<string> GetErrors(IEnumerable<IExerciseDefinition> exercises)
        {
            var errors = new List<string>();
            var names = new List<string>();
            foreach (var exercise in exercises)
            {
                if (names.Contains(exercise.Name))
                {
                    errors.Add($"{exercise.Name} is not unique.");
                }
                else
                {
                    names.Add(exercise.Name);
                }

                if (exercise.Name.Length < MIN_NAME_LENGTH) errors.Add($"{exercise.Name} is too short.");
                else if (exercise.Name.Length >= MAX_NAME_LENGTH) errors.Add($"{exercise.Name} is too long.");
            }

            return errors;
        }
    }
}