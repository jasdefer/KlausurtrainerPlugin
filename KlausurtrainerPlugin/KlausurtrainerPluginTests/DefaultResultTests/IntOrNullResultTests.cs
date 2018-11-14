using KlausurtrainerPlugin.DefaultResults;
using KlausurtrainerPlugin.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KlausurtrainerPluginTests.DefaultResultTests
{
    [TestClass]
    public class IntOrNullResultTests
    {
        private static readonly IntOrNullResult NullResult = new IntOrNullResult(null);
        private static readonly IntOrNullResult Result = new IntOrNullResult(3);

        [TestMethod]
        public void Expected()
        {
            Assert.AreEqual(3, Result.Expected);
            Assert.AreEqual("3", Result.GetSolution());
        }

        [TestMethod]
        public void SyntaxError()
        {
            Assert.AreEqual(AnswerCellStatus.SyntaxError, Result.Validate("a"));
            Assert.AreEqual(AnswerCellStatus.SyntaxError, Result.Validate("3.5"));
            Assert.AreEqual(AnswerCellStatus.SyntaxError, Result.Validate("3,5"));
        }

        [TestMethod]
        public void Correct()
        {
            Assert.AreEqual(AnswerCellStatus.Correct, Result.Validate("3"));
        }

        [TestMethod]
        public void Incorrect()
        {
            Assert.AreEqual(AnswerCellStatus.Incorrect, Result.Validate("-1"));
            Assert.AreEqual(AnswerCellStatus.Incorrect, Result.Validate("0"));
            Assert.AreEqual(AnswerCellStatus.Incorrect, Result.Validate(""));
            Assert.AreEqual(AnswerCellStatus.Incorrect, Result.Validate(null));
        }

        [TestMethod]
        public void NullExpected()
        {
            Assert.AreEqual(null, NullResult.Expected);
            Assert.AreEqual(string.Empty, NullResult.GetSolution());
        }

        [TestMethod]
        public void NullSyntaxError()
        {
            Assert.AreEqual(AnswerCellStatus.SyntaxError, NullResult.Validate("a"));
            Assert.AreEqual(AnswerCellStatus.SyntaxError, NullResult.Validate("3.5"));
            Assert.AreEqual(AnswerCellStatus.SyntaxError, NullResult.Validate("3,5"));
        }

        [TestMethod]
        public void NullCorrect()
        {
            Assert.AreEqual(AnswerCellStatus.Correct, NullResult.Validate(""));
            Assert.AreEqual(AnswerCellStatus.Correct, NullResult.Validate(null));
        }

        [TestMethod]
        public void NullIncorrect()
        {
            Assert.AreEqual(AnswerCellStatus.Incorrect, NullResult.Validate("-1"));
            Assert.AreEqual(AnswerCellStatus.Incorrect, NullResult.Validate("0"));
        }
    }
}