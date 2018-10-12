using KlausurtrainerPlugin;
using KlausurtrainerPlugin.Enum;
using System;
using System.Collections.Generic;

namespace KlausurtrainerPluginTests.ValidationTests.Exercises
{
    public class ExceptionExercise : IExerciseDefinition
    {
        private static readonly Cell CELL = new Cell(0, 0);
        public string Name => "Name";

        public string FileName => "Name.cshtml";

        public int Order => 1;

        public string Chapter => "Chapter";

        public Dictionary<Cell, AnswerCell> AnswerCells => throw new Exception();

        public Dictionary<Cell, AnswerCellStatus> GetAnswerDetails(Dictionary<Cell, string> inputs)
        {
            throw new Exception();
        }

        public string[] GetExerciseDescription()
        {
            throw new Exception();
        }

        public Dictionary<Cell, string> GetExpectedSolutions()
        {
            throw new Exception();
        }

        public object GetValues()
        {
            throw new Exception();
        }

        public void Instantiate(string seed)
        {
            throw new Exception();
        }
    }
}