using KlausurtrainerPlugin;
using KlausurtrainerPlugin.Enum;
using KlausurtrainerPlugin.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace KlausurtrainerPluginTests.HelperTests
{
    [TestClass]
    public class AnswerHelperTests
    {
        [TestMethod]
        public void TestCorrect()
        {
            var dict = new Dictionary<Cell, AnswerCellStatus>()
            {
                { new Cell(0,0), AnswerCellStatus.Correct }
            };

            Assert.AreEqual(AnswerStatus.Correct, dict.GetStatus());
        }

        [TestMethod]
        public void TestIncorrect()
        {
            var dict = new Dictionary<Cell, AnswerCellStatus>()
            {
                { new Cell(0,0), AnswerCellStatus.Correct },
                { new Cell(0,1), AnswerCellStatus.Incorrect },
                { new Cell(0,2), AnswerCellStatus.Correct }
            };

            Assert.AreEqual(AnswerStatus.Incorrect, dict.GetStatus());
        }

        [TestMethod]
        public void TestSyntaxError()
        {
            var dict = new Dictionary<Cell, AnswerCellStatus>()
            {
                { new Cell(0,0), AnswerCellStatus.Correct },
                { new Cell(0,1), AnswerCellStatus.SyntaxError },
                { new Cell(0,2), AnswerCellStatus.Correct }
            };

            Assert.AreEqual(AnswerStatus.SyntaxError, dict.GetStatus());
        }

        [TestMethod]
        public void TestSyntaxErrorBecauseOfMissing()
        {
            var dict = new Dictionary<Cell, AnswerCellStatus>()
            {
                { new Cell(0,0), AnswerCellStatus.Correct },
                { new Cell(0,1), AnswerCellStatus.Missing },
                { new Cell(0,2), AnswerCellStatus.Correct }
            };

            Assert.AreEqual(AnswerStatus.SyntaxError, dict.GetStatus());
        }

        [TestMethod]
        public void TestNotValidated()
        {
            var dict = new Dictionary<Cell, AnswerCellStatus>()
            {
                { new Cell(0,0), AnswerCellStatus.Correct },
                { new Cell(0,1), AnswerCellStatus.NotValidated },
                { new Cell(0,2), AnswerCellStatus.Correct }
            };

            Assert.AreEqual(AnswerStatus.NotValidated, dict.GetStatus());
        }
    }
}