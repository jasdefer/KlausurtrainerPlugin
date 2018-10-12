using KlausurtrainerPlugin;
using KlausurtrainerPlugin.ExerciseValidation;
using KlausurtrainerPluginTests.ValidationTests.Exercises;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KlausurtrainerPluginTests.ValidationTests
{
    [TestClass]
    public class NameValidationTests
    {
        [TestMethod]
        public void ValidExercise()
        {
            var exercises = new IExerciseDefinition[]
            {
                new ValidExercise()
            };

            var errors = NameValidation.GetErrors(exercises);
            Assert.AreEqual(0, errors.Count, string.Join(",", errors));
        }

        [TestMethod]
        public void DuplicateNames()
        {
            var exercises = new IExerciseDefinition[]
            {
                new ValidExercise(),
                new ValidExercise()
            };

            var errors = NameValidation.GetErrors(exercises);
            Assert.AreEqual(1, errors.Count, string.Join(",", errors));
        }
    }
}