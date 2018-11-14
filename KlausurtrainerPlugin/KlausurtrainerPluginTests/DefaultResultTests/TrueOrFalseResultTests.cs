using KlausurtrainerPlugin.DefaultResults;
using KlausurtrainerPlugin.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KlausurtrainerPluginTests.DefaultResultTests
{
    [TestClass]
    public class TrueOrFalseResultTests
    {
        private static readonly TrueOrFalseResult Result = new TrueOrFalseResult(false);

        [TestMethod]
        public void Expected()
        {
            Assert.AreEqual(false, Result.Expected);
            Assert.AreEqual("False", Result.GetSolution());
        }

        [TestMethod]
        public void SyntaxError()
        {
            Assert.AreEqual(AnswerCellStatus.SyntaxError, Result.Validate("a"));
            Assert.AreEqual(AnswerCellStatus.SyntaxError, Result.Validate("3"));
            Assert.AreEqual(AnswerCellStatus.SyntaxError, Result.Validate("3,5"));
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
            Assert.AreEqual(AnswerCellStatus.Correct, Result.Validate("false"));
            Assert.AreEqual(AnswerCellStatus.Correct, Result.Validate("False"));
            Assert.AreEqual(AnswerCellStatus.Correct, Result.Validate("FALSE"));

        }

        [TestMethod]
        public void Incorrect()
        {
            Assert.AreEqual(AnswerCellStatus.Incorrect, Result.Validate("true"));
            Assert.AreEqual(AnswerCellStatus.Incorrect, Result.Validate("True"));
            Assert.AreEqual(AnswerCellStatus.Incorrect, Result.Validate("TRUE"));
        }
    }
}