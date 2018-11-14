using KlausurtrainerPlugin.DefaultResults;
using KlausurtrainerPlugin.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KlausurtrainerPluginTests.DefaultResultTests
{
    [TestClass]
    public class StringOrNullResultTests
    {
        private static readonly StringOrNullResult Result = new StringOrNullResult("Aa", StringCompareModes.CaseInvariant);
        private static readonly StringOrNullResult NullResult = new StringOrNullResult(null, StringCompareModes.CaseInvariant);

        [TestMethod]
        public void Expected()
        {
            Assert.AreEqual("Aa", Result.Expected);
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
        public void NullExpected()
        {
            Assert.AreEqual(null, NullResult.Expected);
            Assert.AreEqual("", NullResult.GetSolution());
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
            Assert.AreEqual(AnswerCellStatus.Incorrect, NullResult.Validate("aaa"));
            Assert.AreEqual(AnswerCellStatus.Incorrect, NullResult.Validate("aa"));
            Assert.AreEqual(AnswerCellStatus.Incorrect, NullResult.Validate("AA"));
            Assert.AreEqual(AnswerCellStatus.Incorrect, NullResult.Validate("aA"));
        }
    }
}