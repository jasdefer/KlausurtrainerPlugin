using KlausurtrainerPlugin.DefaultResults;
using KlausurtrainerPlugin.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KlausurtrainerPluginTests.DefaultResultTests
{
    [TestClass]
    public class StringResultTests
    {
        private static readonly StringResult Result = new StringResult("Aa", StringCompareModes.CaseInvariant);
        private static readonly StringResult ExactResult = new StringResult("Aa", StringCompareModes.Exact);

        [TestMethod]
        public void Expected()
        {
            Assert.AreEqual("Aa", Result.Expected);
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
            Assert.AreEqual(AnswerCellStatus.Correct, Result.Validate("aa"));
            Assert.AreEqual(AnswerCellStatus.Correct, Result.Validate("AA"));
            Assert.AreEqual(AnswerCellStatus.Correct, Result.Validate("aA"));
            Assert.AreEqual(AnswerCellStatus.Correct, Result.Validate("Aa"));
        }

        [TestMethod]
        public void Incorrect()
        {
            Assert.AreEqual(AnswerCellStatus.Incorrect, Result.Validate("-1"));
            Assert.AreEqual(AnswerCellStatus.Incorrect, Result.Validate("0"));
            Assert.AreEqual(AnswerCellStatus.Incorrect, Result.Validate("aaa"));
        }

        [TestMethod]
        public void ExactExpected()
        {
            Assert.AreEqual("Aa", ExactResult.Expected);
        }

        [TestMethod]
        public void ExactMissing()
        {
            Assert.AreEqual(AnswerCellStatus.Missing, ExactResult.Validate(""));
            Assert.AreEqual(AnswerCellStatus.Missing, ExactResult.Validate(null));
        }

        [TestMethod]
        public void ExactCorrect()
        {
            Assert.AreEqual(AnswerCellStatus.Correct, ExactResult.Validate("Aa"));
        }

        [TestMethod]
        public void ExactIncorrect()
        {
            Assert.AreEqual(AnswerCellStatus.Incorrect, ExactResult.Validate("-1"));
            Assert.AreEqual(AnswerCellStatus.Incorrect, ExactResult.Validate("0"));
            Assert.AreEqual(AnswerCellStatus.Incorrect, ExactResult.Validate("aaa"));
            Assert.AreEqual(AnswerCellStatus.Incorrect, ExactResult.Validate("aa"));
            Assert.AreEqual(AnswerCellStatus.Incorrect, ExactResult.Validate("AA"));
            Assert.AreEqual(AnswerCellStatus.Incorrect, ExactResult.Validate("aA"));
        }
    }
}