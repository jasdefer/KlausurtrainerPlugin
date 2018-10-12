using KlausurtrainerPlugin;
using KlausurtrainerPlugin.ExerciseValidation;
using KlausurtrainerPluginTests.ValidationTests.Exercises;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace KlausurtrainerPluginTests.ValidationTests
{
    [TestClass]
    public class TimeValidationTests
    {
        [TestMethod]
        public async Task ValidExercise()
        {
            var exercises = new IExerciseDefinition[]
            {
                new ValidExercise()
            };

            var errors = await TimeValidation.GetErrors(exercises);
            Assert.AreEqual(0, errors.Count, string.Join(",", errors));
        }

        [TestMethod]
        public async Task SlowExercise()
        {
            var exercises = new IExerciseDefinition[]
            {
                new LongCalculationExercise(),
            };

            var errors = await TimeValidation.GetErrors(exercises);
            Assert.AreEqual(TimeValidation.ITERATIONS, errors.Count, string.Join(",", errors));
        }
    }
}