using KlausurtrainerPlugin;
using KlausurtrainerPlugin.ExerciseValidation;
using KlausurtrainerPluginTests.ValidationTests.Exercises;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KlausurtrainerPluginTests.ValidationTests
{
    [TestClass]
    public class ImplementationValidationTests
    {
        [TestMethod]
        public void ValidExercise()
        {
            var exercises = new IExerciseDefinition[]
            {
                new ValidExercise(),
                new ValidExercise(),
            };

            var errors = ImplementationValidation.GetErrors(exercises);
            Assert.AreEqual(0, errors.Count, string.Join(",", errors));
        }

        [TestMethod]
        public void Exception()
        {
            var exercises = new IExerciseDefinition[]
            {
                new ExceptionExercise(),
            };

            var errors = ImplementationValidation.GetErrors(exercises);
            Assert.AreEqual(1, errors.Count, string.Join(",", errors));
        }

        [TestMethod]
        public void ExerciseWithLabelTest()
        {
            var exercises = new IExerciseDefinition[]
            {
                new ExerciseWithLabel(),
            };

            var errors = ImplementationValidation.GetErrors(exercises);
            Assert.AreEqual(0, errors.Count, string.Join(",", errors));
        }
    }
}