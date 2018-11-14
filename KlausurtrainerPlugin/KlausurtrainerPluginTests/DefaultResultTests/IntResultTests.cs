using KlausurtrainerPlugin.DefaultResults;
using KlausurtrainerPlugin.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KlausurtrainerPluginTests.DefaultResultTests
{
    [TestClass]
    public class IntResultTests
    {
        private static readonly IntResult Result = new IntResult(3);

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
        public void Missing()
        {
            Assert.AreEqual(AnswerCellStatus.Missing, Result.Validate(""));
            Assert.AreEqual(AnswerCellStatus.Missing, Result.Validate(null));
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
        }
    }
}