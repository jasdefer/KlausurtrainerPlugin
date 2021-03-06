﻿using KlausurtrainerPlugin;
using KlausurtrainerPlugin.Enum;
using System.Collections.Generic;

namespace KlausurtrainerPluginTests.ValidationTests.Exercises
{
    public class ExerciseWithLabel : IExerciseDefinition
    {
        private static readonly Cell CELL = new Cell(0, 0);

        public string Name => "Name";

        public string FileName => "Name.cshtml";

        public int Order => 1;

        public string Chapter => "Chapter";

        public Dictionary<Cell, AnswerCell> AnswerCells => new Dictionary<Cell, AnswerCell>() { { CELL, new AnswerCell(InputTypes.Int)},{ new Cell(1,0), new AnswerCell(InputTypes.Label) } };

        public Dictionary<Cell, AnswerCellStatus> GetAnswerDetails(Dictionary<Cell, string> inputs)
        {
            var input = inputs[CELL];
            AnswerCellStatus status = AnswerCellStatus.Correct;
            if (string.IsNullOrEmpty(input))
            {
                status = AnswerCellStatus.Missing;
            }
            else if(!int.TryParse(input, out int result))
            {
                status = AnswerCellStatus.SyntaxError;
            }
            else if (result != 1)
            {
                status = AnswerCellStatus.Incorrect;
            }
            return new Dictionary<Cell, AnswerCellStatus>() { { CELL, status } };
        }

        public string[] GetExerciseDescription()
        {
            return new string[] { "Description" };
        }

        public Dictionary<Cell, string> GetExpectedSolutions()
        {
            return new Dictionary<Cell, string>() { { CELL, "1" } };
        }

        public object GetValues()
        {
            return 1;
        }

        public void Instantiate(string seed)
        {
            
        }
    }
}