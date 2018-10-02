using KlausurtrainerPlugin.DefaultResults;
using KlausurtrainerPlugin.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KlausurtrainerPlugin
{
    public abstract class ExerciseDefinition<T> : IExerciseDefinition
    {
        public abstract string Name { get; }

        public abstract string FileName { get; }

        public virtual string Chapter => string.Empty;

        public virtual int Order => 0;

        protected T Values { get; set; }

        protected Random Rnd { get; set; }

        public Dictionary<Cell, AnswerCell> AnswerCells { get; protected set; }

        protected Dictionary<Cell, IResult> Solutions { get; set; }

        public Dictionary<Cell, AnswerCellStatus> GetAnswerDetails(Dictionary<Cell, string> inputs)
        {
            GenerateSolutions();
            Dictionary<Cell, AnswerCellStatus> status = new Dictionary<Cell, AnswerCellStatus>();
            foreach (var item in inputs)
            {
                bool success = Solutions.TryGetValue(item.Key, out IResult result);
                if (!success) throw new Exception($"No answer cell is defined for {item.Key} in {Name}.");
                status.Add(item.Key, result.Validate(item.Value));
            }
            return status;
        }

        public virtual string[] GetExerciseDescription()
        {
            return new string[0];
        }

        public Dictionary<Cell, string> GetExpectedSolutions()
        {
            Dictionary<Cell, string> solutions = new Dictionary<Cell, string>();
            GenerateSolutions();
            foreach (var item in Solutions)
            {
                solutions.Add(item.Key, item.Value.GetSolution());
            }
            return solutions;
        }

        public object GetValues()
        {
            return Values;
        }

        public void SetValues(T values)
        {
            Values = values;
        }

        /// <summary>
        /// Instantiates the exercise for a given seed.
        /// The random gets instantiated with the hashcode of the lower invariant of the given seed.
        /// The exercise values are calculated and the answer cells get defined.
        /// </summary>
        /// <param name="seed"></param>
        public void Instantiate(string seed)
        {
            Rnd = new Random(seed.ToLowerInvariant().GetHashCode());
            AnswerCells = new Dictionary<Cell, AnswerCell>();
            Solutions = new Dictionary<Cell, IResult>();
            SetValues();
            DefineAnswerCells();
        }

        protected abstract void SetValues();
        protected abstract void GenerateSolutions();
        protected abstract void DefineAnswerCells();

        /// <summary>
        /// Add a answer cell.
        /// </summary>
        /// <param name="inputType">The input type of the answer cell</param>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="label">The label of this answer cell. Default value is null: No label will be displayed</param>
        protected void DefineAnswerCell(InputTypes inputType, int row, int column, string label = null, string[] options = null)
        {
            var cell = new Cell(row, column);
            if (AnswerCells.ContainsKey(cell)) throw new Exception($"There is more than on answer cell defined for {cell}.");
            AnswerCells.Add(new Cell(row, column), new AnswerCell(inputType, options) { Label = label });
        }

        /// <summary>
        /// Define an answer cell which only accepts integers. The input type is int.
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="label">The label of this answer cell. Default value is null: No label will be displayed</param>
        protected void DefineIntAnswerCell(int row, int column, string label = null)
        {
            DefineAnswerCell(InputTypes.Int, row, column, label);
        }

        /// <summary>
        /// Define an answer cell which only accepts strings. The input type is string.
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="label">The label of this answer cell. Default value is null: No label will be displayed</param>
        protected void DefineStringAnswerCell(int row, int column, string label = null)
        {
            DefineAnswerCell(InputTypes.String, row, column, label);
        }

        /// <summary>
        /// Define an answer cell which only accepts decimals. The input type is decimal.
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="label">The label of this answer cell. Default value is null: No label will be displayed</param>
        protected void DefineDecimalAnswerCell(int row, int column, string label = null)
        {
            DefineAnswerCell(InputTypes.Decimal, row, column, label);
        }

        /// <summary>
        /// Define a checkbox as the answer cell. The input type is TrueFalse
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="label">The label of this answer cell. Default value is null: No label will be displayed</param>
        protected void DefineTrueFalseAnswerCell(int row, int column, string label = null)
        {
            DefineAnswerCell(InputTypes.TrueFalse, row, column, label);
        }

        /// <summary>
        /// Add a answer cell for a multiple choice question
        /// </summary>
        /// <param name="options">The set of options for the multiple choice question.</param>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="label">The label of this answer cell. Default value is null: No label will be displayed</param>
        protected void DefineMultipleChoiceAnswer(string[] options, int row, int column, string label = null)
        {
            if (options?.Length < 1)
            {
                throw new ArgumentException($"Define a set of options for the multiple choice cell {row}/{column} on exercise {Name}");
            }
            DefineAnswerCell(InputTypes.MultipleChoice, row, column, label, options);
        }

        /// <summary>
        /// Add a label to the answer cells. The user cannot enter any answers. It's only a label.
        /// </summary>
        protected void DefineLabel(string label, int row, int column)
        {
            AnswerCells.Add(new Cell(row, column), new AnswerCell(InputTypes.Label) { Label = label });
        }

        /// <summary>
        /// Add a solution to any answer cell
        /// </summary>
        /// <param name="result">The result that validates the input for this answer cell</param>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        protected void AddSolution(IResult result, int row, int column)
        {
            AddSolution(result, new Cell(row, column));
        }

        /// <summary>
        /// Add a solution to any answer cell
        /// </summary>
        /// <param name="result">The result that validates the input for this answer cell</param>
        /// <param name="cell">The cell to which this solution should be added</param>
        protected void AddSolution(IResult result, Cell cell)
        {
            Solutions[cell] = result;
        }

        /// <summary>
        /// Add an integer solution to the answer cell
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        protected void AddIntegerSolution(int row, int column, int expectedValue)
        {
            IResult result = new IntResult(expectedValue);
            AddSolution(result, row, column);
        }

        /// <summary>
        /// Add a solution which can either be an integer or null to the answer cell
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        protected void AddIntegerOrNullSolution(int row, int column, int? expectedValue)
        {
            IResult result = new IntOrNullResult(expectedValue);
            AddSolution(result, row, column);
        }

        /// <summary>
        /// Add a decimal solution to the answer cell
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        /// <param name="epsilon">The tolerance range for the input. See the decimal result for a detailed description</param>
        protected void AddDecimalSolution(int row, int column, decimal expectedValue, decimal epsilon = 0)
        {
            IResult result = new DecimalResult(expectedValue, epsilon);
            AddSolution(result, row, column);
        }

        /// <summary>
        /// Add a decimal solution to the answer cell
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        /// <param name="epsilon">The tolerance range for the input. See the decimal result for a detailed description</param>
        protected void AddDecimalOrNullSolution(int row, int column, decimal? expectedValue, decimal epsilon = 0)
        {
            IResult result = new DecimalOrNullResult(expectedValue, epsilon);
            AddSolution(result, row, column);
        }

        /// <summary>
        /// Add an integer solution to the answer cell
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        /// <param name="compareMode">Defines how the expected value and input are compared (case sensitivity)</param>
        protected void AddStringSolution(int row, int column, string expectedValue, StringCompareModes compareMode = StringCompareModes.Exact)
        {
            IResult result = new StringResult(expectedValue, compareMode);
            AddSolution(result, row, column);
        }

        /// <summary>
        /// Add an integer solution to the answer cell
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        /// <param name="compareMode">Defines how the expected value and input are compared (case sensitivity)</param>
        protected void AddStringOrNullSolution(int row, int column, string expectedValue, StringCompareModes compareMode = StringCompareModes.Exact)
        {
            IResult result = new StringOrNullResult(expectedValue, compareMode);
            AddSolution(result, row, column);
        }

        /// <summary>
        /// Add a true or false solution to the answer cell
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        protected void AddTrueOrFalseSolution(int row, int column, bool expectedValue)
        {
            IResult result = new TrueOrFalseResult(expectedValue);
            AddSolution(result, row, column);
        }

        /// <summary>
        /// Add a fraction result. If the answer cell is defined as a fraction the solution must be a fraction as well.
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        /// <param name="denominator">For displaying the correct solution. Has no effect on the validation</param>
        /// <param name="numerator">For displaying the correct solution. Has no effect on the validation</param>
        /// <param name="epsilon">The input is correct as long as the difference between the input and expected value is not greater than this epsilon</param>
        protected void AddFractionSolution(int row, int column, decimal expectedValue, decimal epsilon = 0.01m, int? numerator = null, int? denominator = null)
        {
            IResult result = new FractionResult(expectedValue, epsilon, numerator, denominator);
            AddSolution(result, row, column);
        }
    }
}