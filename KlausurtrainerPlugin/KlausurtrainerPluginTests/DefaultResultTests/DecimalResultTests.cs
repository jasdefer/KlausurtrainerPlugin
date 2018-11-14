using KlausurtrainerPlugin.DefaultResults;
using KlausurtrainerPlugin.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KlausurtrainerPluginTests.DefaultResultTests
{
    [TestClass]
    public class DecimalResultTests
    {
        private static readonly DecimalResult Result = new DecimalResult(3.5m, 0.1m);

        [TestMethod]
        public void Expected()
        {
            Assert.AreEqual(3.5m, Result.Expected);
            Assert.AreEqual("3.5", Result.GetSolution());
        }

        [TestMethod]
        public void SyntaxError()
        {
            Assert.AreEqual(AnswerCellStatus.SyntaxError, Result.Validate("a"));
        }

        [TestMethod]
        public void Missing()
        {
            Assert.AreEqual(AnswerCellStatus.Missing, Result.Validate(""));
            Assert.AreEqual(AnswerCellStatus.Missing, Result.Validate(null));
        }

        [TestMethod]
        public void Correct()
        {
            Assert.AreEqual(AnswerCellStatus.Correct, Result.Validate("3.5"));
            Assert.AreEqual(AnswerCellStatus.Correct, Result.Validate("3.4"));
            Assert.AreEqual(AnswerCellStatus.Correct, Result.Validate("3.6"));
            Assert.AreEqual(AnswerCellStatus.Correct, Result.Validate("3,5"));
            Assert.AreEqual(AnswerCellStatus.Correct, Result.Validate("3,4"));
            Assert.AreEqual(AnswerCellStatus.Correct, Result.Validate("3,6"));
        }

        [TestMethod]
        public void Incorrect()
        {
            Assert.AreEqual(AnswerCellStatus.Incorrect, Result.Validate("-1"));
            Assert.AreEqual(AnswerCellStatus.Incorrect, Result.Validate("0"));
            Assert.AreEqual(AnswerCellStatus.Incorrect, Result.Validate("3.61"));
            Assert.AreEqual(AnswerCellStatus.Incorrect, Result.Validate("3,39"));
        }
    }
}